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
using System.Linq;

namespace EvergreenAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "User, Professor, Admin")]
    [ApiController]
    public class DetectionHistoryController : ControllerBase
    {
        private readonly IDetectionHistoryRepository _detectionHistoryRepository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public DetectionHistoryController(IDetectionHistoryRepository detectionHistoryRepository, IMapper mapper, AppDbContext context)
        {
            _detectionHistoryRepository = detectionHistoryRepository;
            _mapper = mapper;
            _context = context;
        }

        // For testing
        [AllowAnonymous]
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
                    ImageName = detection.ImageName,
                    DetectedDisease = detectedDisease.Disease.Name,
                    Accuracy = highestAcc
                });
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(result);
        }

        // For testing
        [AllowAnonymous]
        [HttpGet("historyId={detectionHistoryId}")]
        public IActionResult GetDetectionHistory(int detectionHistoryId)
        {
            if (!_detectionHistoryRepository.Exist(detectionHistoryId))
                return NotFound($"Detection history '{detectionHistoryId}' is not exists!!");

            var accuracies = GetDetectionAccuracies(detectionHistoryId)
                .Where(x => x.DetectionHistoryId == detectionHistoryId)
                .OrderByDescending(x => x.Accuracy)
                .Include(x => x.Disease);

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
