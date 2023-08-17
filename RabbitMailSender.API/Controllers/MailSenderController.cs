using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMailSender.API.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace RabbitMailSender.API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class MailSenderController : ControllerBase
    {
        [HttpPost]
        public IActionResult SendEmail(Mail model)
        {
            var factory = new ConnectionFactory() { Uri = new Uri("amqps://ceahuxxx:0wx2TxaTAA_fp_PzC9qdJfBFR23p87z5@woodpecker.rmq.cloudamqp.com/ceahuxxx") };

            var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();

            channel.QueueDeclare("mailQueue", false, false, false);

            var mail = new Mail
            {
                Id = model.Id,
                Subject = model.Subject,
                Message = model.Message,
                From = model.From,
                To = model.To
            };

            var mailJson = JsonSerializer.Serialize(mail);
            var body = Encoding.UTF8.GetBytes(mailJson);

            channel.BasicPublish("", "mailQueue", body: body);

            return Ok($"Mail sent: {mailJson}");

        }
    }
}
