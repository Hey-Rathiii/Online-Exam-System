using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamClassLibrary.Model
{
    public class SubjectDTO
    {
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }

        // Used for filtering in DAL (-1 = all)
        public int StatusFilter { get; set; } = -1;
    }

}
