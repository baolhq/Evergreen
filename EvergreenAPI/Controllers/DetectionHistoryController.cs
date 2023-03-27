using AutoMapper;
using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using EvergreenAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System.Security.Claims;
using System;
using static System.Net.WebRequestMethods;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using Newtonsoft.Json;


namespace EvergreenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetectionHistoryController : ControllerBase
    {
        private readonly HttpClient client = null;
        private readonly IDetectionHistoryRepository _detectionHistoryRepository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private IHostingEnvironment _environment;

        public DetectionHistoryController(IDetectionHistoryRepository detectionHistoryRepository, IMapper mapper, AppDbContext context, IHostingEnvironment environment)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);


            _detectionHistoryRepository = detectionHistoryRepository;
            _mapper = mapper;
            _context = context;
            _environment = environment;
        }

        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            var responseMessage = "";
            var postedFile = Request.Form.Files.FirstOrDefault();
            var accountId = Request.Form["uid"].FirstOrDefault();

            if (postedFile == null)
                return BadRequest("File is null or empty");

            string[] permittedExtensions = { ".jpg", ".png", ".jpeg" };
            var ext = Path.GetExtension(postedFile.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                return BadRequest("We only accept JPEG and PNG file");

            string path = Path.Combine(_environment.ContentRootPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = Path.GetFileName(postedFile.FileName);
            string uniqueFilePath = Path.Combine(path, fileName);
            string uniqueFileName = Path.GetFileNameWithoutExtension(uniqueFilePath);
            // Check if file name exist, use Windows style rename
            if (System.IO.File.Exists(uniqueFilePath))
            {
                int count = 1;

                string extension = Path.GetExtension(uniqueFilePath);
                string newFullPath = uniqueFilePath;

                while (System.IO.File.Exists(Path.Combine(path, newFullPath)))
                {
                    string tempFileName = string.Format("{0} ({1})", uniqueFileName, count++);
                    newFullPath = Path.Combine(path, tempFileName + extension);
                }
                uniqueFilePath = newFullPath;
                uniqueFileName = Path.GetFileNameWithoutExtension(uniqueFilePath);
            }

            using var stream = System.IO.File.Create(uniqueFilePath);
            await postedFile.CopyToAsync(stream);
            stream.Close();

            // Save image location to database
            _context.Images.Add(new Image { AltText = uniqueFileName, Url = uniqueFilePath });
            await _context.SaveChangesAsync();

            var history = await SaveHistory(new DetectionHistory
            {
                AccountId = int.Parse(accountId),
                Date = DateTime.Now,
                ImageName = uniqueFileName,
                ImageUrl = uniqueFilePath,
            });

            // Call Python API to detect disease
            var accuracies = await RetrieveAccuraciesFromApi(history, uniqueFilePath);

            responseMessage = $"{fileName} uploaded successfully";
            return Ok(responseMessage);
        }

        private async Task<List<DetectionAccuracy>> RetrieveAccuraciesFromApi(DetectionHistory history, string filepath)
        {
            var accList = new List<DetectionAccuracy>();
            var ApiBaseUrl = "http://127.0.0.1:8000/predict";
            var detectingDiseases = _context.Diseases.Where(
                d => d.Name == "Early Blight"
                || d.Name == "Septoria"
                || d.Name == "Yellow Curl" ||
                d.Name == "Healthy Leaf").ToList();
            var data = new List<PredictionDTO>();

            using (var multipartFormContent = new MultipartFormDataContent())
            {
                //Load the file and set the file's Content-Type header
                FileStream temp = null;
                try
                {
                    temp = System.IO.File.Open(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                var fileStreamContent = new StreamContent(temp);
                var ext = Path.GetExtension(filepath);
                var filename = Path.GetFileName(filepath);

                fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");

                //Add the file
                multipartFormContent.Add(fileStreamContent, name: "file", fileName: filename);

                //Send it
                var response = await client.PostAsync(ApiBaseUrl, multipartFormContent);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                var result = response.Content.ReadAsStringAsync().Result;
                data = JsonConvert.DeserializeObject<List<PredictionDTO>>(result);
            }

            for (var i = 0; i < detectingDiseases.Count; i++)
            {
                var acc = new DetectionAccuracy();
                acc.Accuracy = data[i].Probability;
                acc.DiseaseId = detectingDiseases[i].DiseaseId;
                acc.DetectionHistoryId = history.DetectionHistoryId;
                _context.DetectionAccuracies.Add(acc);
            }

            await _context.SaveChangesAsync();
            return accList;
        }

        /// <summary>
        /// Save image history
        /// </summary>
        /// <param name="history">The image to save into history</param>
        /// <returns></returns>
        private async Task<DetectionHistory> SaveHistory(DetectionHistory history)
        {
            if (history == null) return null;
            _context.DetectionHistories.Add(history);
            await _context.SaveChangesAsync();
            return history;
        }

        // For testing
        [HttpGet("{accountId}")]
        public IActionResult GetDetectionHistories(int accountId)
        {
            var detectionHistories =
                _mapper.Map<List<DetectionHistory>>(_detectionHistoryRepository.GetDetectionHistories(accountId));


            var result = new List<ExtractDetectionHistoriesDTO>();
            foreach (var detection in detectionHistories)
            {
                var accuracies = GetDetectionAccuracies(detection.DetectionHistoryId).Include(x => x.Disease);
                var highestAcc = accuracies.Max(x => x.Accuracy);
                var detectedDisease = accuracies.First(x => x.Accuracy == highestAcc);

                result.Add(new ExtractDetectionHistoriesDTO
                {
                    DetectionHistoryId = detection.DetectionHistoryId,
                    ImageName = detection.ImageName,
                    DetectedDisease = detectedDisease.Disease.Name,
                    Accuracy = (float)highestAcc,
                    ImageUrl = detection.ImageUrl
                });
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(result);
        }

        // For testing
        [HttpGet("Details/{detectionHistoryId}")]
        public IActionResult GetDetectionHistory(int detectionHistoryId)
        {
            if (!_detectionHistoryRepository.Exist(detectionHistoryId))
                return NotFound($"Detection history '{detectionHistoryId}' is not exists!!");

            var accuracies = GetDetectionAccuracies(detectionHistoryId)
                .Where(x => x.DetectionHistoryId == detectionHistoryId)
                .OrderByDescending(x => x.Accuracy)
                .Include(x => x.Disease)
                .Include(x => x.Disease.Medicine)
                .Include(x => x.Disease.Treatment)
                .ToList();

            return Ok(accuracies);
        }

        private IQueryable<DetectionAccuracy> GetDetectionAccuracies(int historyId)
        {
            //TODO: Disease is null now
            var result =
                from acc in _context.DetectionAccuracies
                join disease in _context.Diseases
                on acc.DiseaseId equals disease.DiseaseId
                orderby acc.Accuracy descending
                where acc.DetectionHistoryId == historyId
                select acc;

            return result;
        }
    }
}
