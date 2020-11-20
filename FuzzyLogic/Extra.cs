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
}
