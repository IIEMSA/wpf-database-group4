using System;
using System.Collections.Generic;
using System.Text;

namespace ExamsManagement
{
    internal class Grading
    {
        public static string GetGrade(int markValue)
        {
            if (markValue >= 80)
                return "A";
            else if (markValue >= 70)
                return "B";
            else if (markValue >= 60)
                return "C";
            else if (markValue >= 50)
                return "D";
            else
                return "F";
        }
    }
}
