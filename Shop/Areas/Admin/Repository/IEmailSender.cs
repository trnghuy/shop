﻿namespace Shop.Areas.Admin.Repository
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
