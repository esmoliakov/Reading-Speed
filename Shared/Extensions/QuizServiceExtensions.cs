using System.Collections;
using System.Collections.Generic;

namespace Server.Extensions
{
    public static class QuizServiceExtensions
    {
        // Extension method to convert ArrayList to List<int>
        public static List<int> ToIntList(this ArrayList arrayList)
        {
            var intList = new List<int>();

            foreach (object score in arrayList)
            {
                if (score is int unboxedScore)
                {
                    intList.Add(unboxedScore);
                }
            }

            return intList;
        }
    }
}
