using AutoMapper;
using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using EvergreenAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IMessageRepository _MessageRepository;
        private readonly IMapper _mapper;

        public MessageController(IMessageRepository MessageRepository, IMapper mapper)
        {
            _MessageRepository = MessageRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetMessages()
        {
            var Messages = _mapper.Map<List<MessageDTO>>(_MessageRepository.GetMessages());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Messages);
        }

        [HttpGet("{MessageId}")]
        public IActionResult GetMessage(int MessageId)
        {
            if (!_MessageRepository.MessageExist(MessageId))
                return NotFound($"Message Category '{MessageId}' is not exists!!");

            var Messages = _mapper.Map<MessageDTO>(_MessageRepository.GetMessageById(MessageId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Messages);
        }

        [HttpPost]
        [Authorize (Roles = "Admin")]
        public IActionResult CreateMessage([FromBody] MessageDTO MessageCreate)
        {
            if (MessageCreate == null)
                return BadRequest(ModelState);

            var Message = _MessageRepository.GetMessages()
                .Where(c => c.Content.Trim().ToUpper() == MessageCreate.Content.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (Message != null)
            {
                ModelState.AddModelError("", "It is already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var MessageMap = _mapper.Map<Message>(MessageCreate);

            if (!_MessageRepository.SaveMessage(MessageMap))
            {
                ModelState.AddModelError("", "Something was wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Create Success");
        }

        [HttpPut("{MessageId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateMessage(int MessageId, [FromBody] MessageDTO updatedMessage)
        {
            if (updatedMessage == null)
                return BadRequest(ModelState);

            if (MessageId != updatedMessage.MessageId)
                return BadRequest(ModelState);

            if (!_MessageRepository.MessageExist(MessageId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var MessageMap = _mapper.Map<Message>(updatedMessage);

            if (!_MessageRepository.UpdateMessage(MessageMap))
            {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Updated Success");
        }




        [HttpPut("{MessageId}/{username}")]
        [Authorize (Roles = "User")]
        public IActionResult UserUpdateMessage(int MessageId, string username, [FromBody] MessageDTO updatedMessage)
        {
            if (updatedMessage == null)
                return BadRequest(ModelState);

            if (MessageId != updatedMessage.MessageId)
                return BadRequest(ModelState);

            if (!_MessageRepository.MessageExist(MessageId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var MessageMap = _mapper.Map<Message>(updatedMessage);

            if (updatedMessage.Author.Username != username)
            {
                return BadRequest("Username does not match with message author");
            }

            if (!_MessageRepository.UpdateMessage(MessageMap))
            {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Updated Success");
        }

        [HttpDelete("{MessageId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteMessage(int MessageId)
        {
            if (!_MessageRepository.MessageExist(MessageId))
                return NotFound();

            var MessageToDelete = _MessageRepository.GetMessageById(MessageId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_MessageRepository.DeleteMessage(MessageToDelete))
            {
                ModelState.AddModelError("", "Something was wrong when delete");
                return StatusCode(500, ModelState);
            }
            return Ok("Delete Success");
        }

        [HttpDelete("{MessageId}/{username}")]
        [Authorize(Roles = "User")]
        public IActionResult UserDeleteMessage(int MessageId, string username)
        {
            if (!_MessageRepository.MessageExist(MessageId))
                return NotFound();

            var MessageToDelete = _MessageRepository.GetMessageById(MessageId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (MessageToDelete.Author.Username != username)
            {
                return BadRequest("Username does not match with message author");
            }

            if (!_MessageRepository.DeleteMessage(MessageToDelete))
            {
                ModelState.AddModelError("", "Something was wrong when delete");
                return StatusCode(500, ModelState);
            }
            return Ok("Delete Success");
        }
    }
}
