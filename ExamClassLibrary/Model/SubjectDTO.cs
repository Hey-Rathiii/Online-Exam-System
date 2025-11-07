using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamClassLibrary.Model
{
    public class SubjectDTO
    {
        public int statusFilter { get; set; }
        public int AdminID { get; set; }
        public string SubjectName { get; set; }
        public int CreatedBy { get; set; } 
        public int SubjectID { get; set; }
        
        public int subjectId { get; set; }
        public string updatedName { get; set; }

    }
}
