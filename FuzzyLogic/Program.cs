using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyLogic
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Testing Things
            //var cero = new FuzzySet(2, (0, 0), (1, 0));
            //var all = new FuzzySet(2, (0, 0), (0, 2), (2, 2), (2, 0));

            //var a = new FuzzySet(4, (0, 0), (1, 2), (2, 3), (4,0));
            //var b = new FuzzySet(4, (0, 0), (1, 2), (2, 1), (3, 1), (3, 0));

            //var c = new FuzzySet(9, (0, 0), (0, 4), (1, 4), (3, 1), (5, 2), (7, 2), (9, 0));
            //var d = new FuzzySet(9, (2, 0), (3, 3), (5, 3), (6, 1), (9, 1), (9, 0));

            //var inter = FuzzySet.Intersection(c, d);
            //var union = FuzzySet.Union(a, b);


            //Console.WriteLine("Fuzzy Set:");
            //foreach (var item in union.SetPoints)
            //{
            //    Console.WriteLine(item);
            //}

            ////var centroid = inter.Centroid();
            ////var bisector = inter.Bisector();

            //var firstMax = union.FirstOfMaximum();
            //var lastMax = union.LastOfMaximum();
            //var meanMax = union.MeanOfMaxDiscrete();

            //Console.WriteLine("The First Max: {0}", firstMax);
            //Console.WriteLine("The Last Max: {0}", lastMax);
            //Console.WriteLine("The Mean of Max: {0}", meanMax);
            //Console.WriteLine(decimal.Round(meanMax));
            #endregion

            //Amount of Clothes
            var amountSmall = new FuzzySet(100, (0, 0), (0, 1), (20, 1), (35, 0));
            var amountMedium = new FuzzySet(100, (15, 0), (35, 1), (55, 1), (75, 0));
            var amountHigh = new FuzzySet(100, (50, 0), (75, 1), (100, 1), (100, 0));

            //Dirtiness of Clothes
            var littleDirty = new FuzzySet(100, (0, 0), (0, 1), (20, 1), (30, 0));
            var dirty = new FuzzySet(100, (15, 0), (30, 1), (55, 1), (75, 0));
            var veryDirty = new FuzzySet(100, (50, 0), (75, 1), (100, 1), (100, 0));

            //Washing Program
            var washGentle = new FuzzySet(100, (0, 0), (0, 1), (20, 1), (40, 0));
            var washNormal = new FuzzySet(100, (20, 0), (40, 1), (60, 1), (80, 0));
            var washStrong = new FuzzySet(100, (60, 0), (80, 1), (100, 1), (100, 0));

            //Washing Timer
            var veryShort = new FuzzySet(15, (0, 0), (0, 1), (2, 1), (3.5m, 0));
            var shortTimer = new FuzzySet(15, (1.5m, 0), (3, 1), (5, 1), (6.5m, 0));
            var mediumTimer = new FuzzySet(15, (4.5m, 0), (6, 1), (9, 1), (10.5m, 0));
            var longTimer = new FuzzySet(15, (8.5m, 0), (10, 1), (12, 1), (13.5m, 0));
            var veryLong = new FuzzySet(15, (11.5m, 0), (13, 1), (15, 1), (15, 0));

            //Rules
            var rule1 = (new FuzzySet[] { amountHigh, veryDirty }, new FuzzySet[] { washStrong, veryLong });
            var rule2 = (new FuzzySet[] { amountHigh, dirty }, new FuzzySet[] { washStrong, veryLong });
            var rule3 = (new FuzzySet[] { amountHigh, littleDirty }, new FuzzySet[] { washNormal, longTimer });

            var rule4 = (new FuzzySet[] { amountMedium, veryDirty }, new FuzzySet[] { washStrong, longTimer });
            var rule5 = (new FuzzySet[] { amountMedium, dirty }, new FuzzySet[] { washNormal, mediumTimer });
            var rule6 = (new FuzzySet[] { amountMedium, littleDirty }, new FuzzySet[] { washGentle, mediumTimer });

            var rule7 = (new FuzzySet[] { amountSmall, veryDirty }, new FuzzySet[] { washNormal, shortTimer });
            var rule8 = (new FuzzySet[] { amountSmall, dirty }, new FuzzySet[] { washGentle, shortTimer });
            var rule9 = (new FuzzySet[] { amountSmall, littleDirty }, new FuzzySet[] { washGentle, veryShort });

            //Creating the Fuzzy System
            var system = new FuzzySystem(rule1, rule2, rule3, rule4, rule5, rule6, rule7, rule8, rule9);

            var result = system.Resolve(Aggregation.Mamdani, Defuzzification.Centroid, amountMedium, dirty);

            Console.WriteLine("The intensity of the Washing Machine: {0}", result[0]);
            Console.WriteLine("The Time of the Washing Mahine: {0}", result[1]);


            //Console.WriteLine(result);
            Console.ReadLine();

        }
    }
}
