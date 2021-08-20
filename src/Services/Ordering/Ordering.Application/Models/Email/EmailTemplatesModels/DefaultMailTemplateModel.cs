using System.Collections.Generic;

namespace Ordering.Application.Models.Email.EmailTemplatesModels
{
    public class DefaultMailTemplateModel
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}