using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamClassLibrary.Model
{
    public class ExamDTO
    {
        public int subjectId { get; set; }
        public string subjectName { get; set; }
        public string examTitle { get; set; }
        public DateTime examDate { get; set; }
        public TimeSpan startTime { get; set; }
        public TimeSpan endTime { get; set; }
        public int durationMinutes { get; set; }
        public int statusFilter { get; set; } // 0 for All, 1 for Active, 2 for Inactive
        public int adminId { get; set; }
        public int examId { get; set; }
        public bool isActive { get; set; } = true;
    }
}
