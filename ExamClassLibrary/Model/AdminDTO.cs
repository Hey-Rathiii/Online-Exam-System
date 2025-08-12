using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamLibrary.Model
{
    public class AdminDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string ContactNumber { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.MinValue;
        public bool IsActive { get; set; } = true;
        public Guid ActivationId { get; set; } = Guid.NewGuid();


    }
}
