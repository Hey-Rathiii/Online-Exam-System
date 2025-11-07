using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamClassLibrary.Model
{
     public class StudentAnswerDTO
    {
        public int AnswerID { get; set; }
        public int StudentID { get; set; }
        public int ExamID { get; set; }
        public int QuestionID { get; set; }
        public char GivenAnswerOption { get; set; }
        public char CorrectAnswerOption { get; set; }
        public bool IsCorrect { get; set; }
        public int MarksAwarded { get; set; }
        public DateTime SubmittedAt { get; set; }

        // Optional helper property (not stored in DB)
        public string QuestionText { get; set; }
    }
}
