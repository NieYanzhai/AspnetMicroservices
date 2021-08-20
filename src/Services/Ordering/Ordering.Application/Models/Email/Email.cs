using System.Collections.Generic;
using FluentEmail.Core.Models;

namespace Ordering.Application.Models.Email
{
    public class Email
    {
        public IEnumerable<Address> ToMailAddresses { get; set; }
        public IEnumerable<Address> CcMailAddresses { get; set; }
        public string Subject { get; set; }
        public string Template { get; set; }
    }
}