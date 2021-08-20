using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentEmail.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models.Email.EmailTemplatesModels;

namespace Ordering.Infrastructure.Email
{
    public class EmailService : IEmailService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<EmailService> logger;

        public EmailService(IServiceProvider serviceProvider, ILogger<EmailService> logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        public async Task<bool> SendMail(Ordering.Application.Models.Email.Email email)
        {
            using (var scope = this.serviceProvider.CreateScope())
            {
                var result = await scope.ServiceProvider.GetRequiredService<IFluentEmail>()
                    .To(email.ToMailAddresses)
                    .CC(email.CcMailAddresses)
                    .Subject(email.Subject)
                    // .Body("hello from fluenemail");
                    .UsingTemplate(email.Template, new DefaultMailTemplateModel
                    {
                        Name = "Nie",
                        Age = 15,
                        Users = new List<User>{
                            new User{FirstName = "Nie", LastName = "Yanzhai"},
                            new User {FirstName = "Ge", LastName = "Lijuan"}
                        }
                    })
                    .SendAsync();
                return result.Successful;
            }
        }
    }
}