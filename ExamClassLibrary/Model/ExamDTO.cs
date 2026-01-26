    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace ExamClassLibrary.Model
    {
    public class ExamDTO
    {
        public int ExamID { get; set; }
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }   // since exists in DB
        public string ExamTitle { get; set; }
        public DateTime ExamDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int DurationMinutes { get; set; }

        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }

        // Filtering (optional)
        public int StatusFilter { get; set; } = -1;
    }


}
