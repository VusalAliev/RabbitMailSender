﻿namespace RabbitMailSender.API.Models
{
    public class Mail
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string From { get; set; }
        public string To { get; set; }

    }
}
