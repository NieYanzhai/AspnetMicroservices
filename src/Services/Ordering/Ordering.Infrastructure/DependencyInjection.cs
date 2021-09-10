using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Infrastructure.Email;
using Ordering.Infrastructure.Persistence;
using Ordering.Infrastructure.RabbitMq;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddFluentEmail(configuration["EmailSettings:From"])
                .AddRazorRenderer()
                .AddSmtpSender(
                    host : configuration["EmailSettings:Host"],
                    port: int.Parse(configuration["EmailSettings:Port"]),
                    username: configuration["EmailSettings:User"],
                    password: configuration["EmailSettings:Password"]);

            services.AddDbContext<OrderContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("SqlConnectionString"));
            });

            services
                .AddScoped<IEmailService, EmailService>()
                .AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>))
                .AddScoped<IOrderRepository, OrderRepository>()
                .AddSingleton<IRabbitMQClientService, RabbitMQClientService>();                   

            return services;
        }
    }
}