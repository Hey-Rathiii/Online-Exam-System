using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamClassLibrary.Model
{
    public class InsertQueDTO
    {       

        public int QuestionID { get; set; }
        public int ExamID { get; set; }

        public string QuestionText { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }

        public string CorrectOption { get; set; }
        public int Marks { get; set; }
    }

}
