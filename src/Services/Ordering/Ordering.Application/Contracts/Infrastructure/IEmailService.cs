using System.Threading.Tasks;
using Ordering.Application.Models.Email;

namespace Ordering.Application.Contracts.Infrastructure
{
    public interface IEmailService
    {
        Task<bool> SendMail(Email mail);
    }

    
}