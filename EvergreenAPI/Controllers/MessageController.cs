using AutoMapper;
using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using EvergreenAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvergreenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        public MessageController(IMessageRepository messageRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }




        [HttpGet]
        public ActionResult GetMessages()
        {
            var p = _messageRepository.GetMessages();
            var m = _mapper.Map<IEnumerable<MessageDTO>>(p);
            return Ok(m);
        }




        [HttpGet("{id}")]
        public ActionResult GetMessageById(int id)
        {
            var p = _messageRepository.GetMessageById(id);
            var m = _mapper.Map<MessageDTO>(p);
            return Ok(m);
        }




        [HttpPost]
        public ActionResult<BlogDTO> SaveMessage(MessageDTO p)
        {
            var message = _mapper.Map<Message>(p);
            _messageRepository.SaveMessage(message);
            return Ok();
        }




        [HttpPut("{id}")]
        public ActionResult UpdateMessage(MessageDTO p)
        {
            var message = _mapper.Map<Message>(p);
            _messageRepository.UpdateMessage(message);
            return Ok();
        }






        [HttpDelete("{id}")]
        public ActionResult DeleteMessage(int id)
        {
            var message = _messageRepository.GetMessageById(id);
            if (message == null)
                return NotFound();
            _messageRepository.DeleteMessage(message);
            return NoContent();
        }
    }
}
