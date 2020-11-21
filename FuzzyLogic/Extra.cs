using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyLogic
{
    public enum Aggregation
    {
        Mamdani,
        Larsen
    }

    public enum Defuzzification
    {
        Centroid,
        Bisector,
        FOM,
        LOM,
        MOM
    }

    public class Transform
    {
        public static string MinFormat(decimal mins)
        {
            var min = (int) mins;
            var sec = (int) Math.Round(60 * (mins - min));
            var stringMin = min.ToString();
            var stringSec = sec.ToString();
            if (stringMin.Length == 1) stringMin = "0" + stringMin;
            if (stringSec.Length == 1) stringSec = "0" + stringSec;

            return stringMin + ":" + stringSec;
        }
    }
}
