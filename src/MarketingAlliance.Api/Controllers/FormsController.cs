using MarketingAlliance.App.Service;
using MarketingAlliance.Core.Entities.Models;
using MarketingAlliance.Core.Entities.Request;
using MarketingAlliance.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace MarketingAlliance.Api.Controllers
{
    [ApiController]
    [Route("api/forms")]
    public class FormsController : ControllerBase
    {
        private readonly MarketingAllianceContext _context;
        private readonly MailService _mailService;
        private readonly string _from;
        private readonly string _to;
        private readonly string _password;
        private readonly string _feedbackSubject = "Обратная связь";
        private readonly string _partnershipApplicationSubject = "Заявка на партнерство";
        private readonly ILogger<FormsController> _logger;
        public FormsController(MarketingAllianceContext context, MailService mailService, ILogger<FormsController> logger)
        {
            _context = context;
            _mailService = mailService;
            _logger = logger;

            _from = Environment.GetEnvironmentVariable("MAIL_FROM");
            _to = Environment.GetEnvironmentVariable("MAIL_TO");
            _password = Environment.GetEnvironmentVariable("MAIL_PASSWORD");
        }

        [HttpPost("feedback")]
        [SwaggerOperation(Summary = "Отправка сообщения обратной связи")]
        [SwaggerResponse(200, "Успешный ответ")]
        [SwaggerResponse(400, "Некорректные данные")]
        [SwaggerResponse(500, "Ошибка при отправке сообщения")]
        public async Task<IActionResult> Feedback([FromBody] FeedbackMessageBody request)
        {
            try
            {
                var feedback = new FeedbackMessage
                {

                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Patronymic = request.Patronymic,
                    Question = request.Question,
                };

                await _context.FeedbackMessages.AddAsync(feedback);
                await _context.SaveChangesAsync();

                var message = feedback.ToMessage();
                await _mailService.Send(_from, _from, _password, new[] { _to }, _feedbackSubject, message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при отправке сообщения обратной связи");
                return StatusCode(500, ex.Message);
            }
            return Ok();
        }

        [HttpPost("partnership-application")]
        [SwaggerOperation(Summary = "Отправка заявки на партнерство")]
        [SwaggerResponse(200, "Успешный ответ")]
        [SwaggerResponse(400, "Некорректные данные")]
        [SwaggerResponse(500, "Ошибка при отправке сообщения")]
        public async Task<IActionResult> PartnershipApplication([FromBody] PartnershipApplicationBody request)
        {
            try
            {
                var partnershipApplication = new PartnershipApplication
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Patronymic = request.Patronymic,
                    Comment = request.Comment,
                    INN = request.INN,
                    Pharmacy = request.Pharmacy,
                    NumberOfRetailPoints = request.NumberOfRetailPoints,
                    ContactPhoneNumber = request.ContactPhoneNumber,
                };

                await _context.PartnershipApplications.AddAsync(partnershipApplication);
                await _context.SaveChangesAsync();

                var message = partnershipApplication.ToMessage();
                await _mailService.Send(_from, _from, _password, new[] { _to }, _partnershipApplicationSubject, message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok();
        }
    }
}