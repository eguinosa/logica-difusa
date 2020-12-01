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
            var veryShort = new FuzzySet(15, (0, 0), (0, 1), (1, 1), (4, 0));
            var shortTimer = new FuzzySet(15, (0, 0), (4, 1), (7.5m, 0));
            var mediumTimer = new FuzzySet(15, (3.5m, 0), (7.5m, 1), (11.5m, 0));
            var longTimer = new FuzzySet(15, (7.5m, 0), (11, 1), (15, 0));
            var veryLong = new FuzzySet(15, (11, 0), (14, 1), (15, 1), (15, 0));

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

            Console.WriteLine("Programa para simular el funcionamiento de una Lavadora, que dado la cantidad de ropa\n" +
                              "y el grado de suciedad que esta tenga, determine con que intensidad tiene que trabajar\n" +
                              "el motor de la lavadora y por cuanto tiempo.");
            Console.WriteLine("-> La Cantidad de Ropa esta determinada por el % de la capacidad de la lavadora que ocupe (0-100%):");
            Console.WriteLine("[Mucha, Media, Poca]"); ;
            Console.WriteLine("-> El Grado de suciedad (0-100%):");
            Console.WriteLine("[Muy Sucia, Sucia, Poco Sucia]");
            Console.WriteLine("-> La Intensidad de Trabajo del Motor (0-100%):");
            Console.WriteLine("[Fuerte, Normal, Suave]");
            Console.WriteLine("El Tiempo de Trabajo de la Lavadora (0-15min):");
            Console.WriteLine("[Muy Largo, Largo, Medio, Corto, Muy Corto]");
            Console.WriteLine("*************************************************\n");

            FuzzySet fuzzyRopa, fuzzySucia;
            decimal percentRopa, percentSucia;
            bool ropaFuzzy, ropaPercent, suciaFuzzy, suciaPercent;

            while (true)
            {
                Console.WriteLine("<< Introduzca la Cantidad de Ropa (o el % de Capacidad que ocupa) >>");
                Console.Write("[Mucha, Media, Poca] o [0-100] : ");
                string data = Console.ReadLine().Replace(" ", "").ToLower();
                ropaPercent = decimal.TryParse(data, out percentRopa);
                switch (data)
                {
                    case "mucha":
                        fuzzyRopa = amountHigh;
                        ropaFuzzy = true;
                        break;
                    case "media":
                        fuzzyRopa = amountMedium;
                        ropaFuzzy = true;
                        break;
                    case "poca":
                        fuzzyRopa = amountSmall;
                        ropaFuzzy = true;
                        break;
                    default:
                        fuzzyRopa = null;
                        ropaFuzzy = false;
                        break;
                }
                if ((!ropaPercent && !ropaFuzzy) || (ropaPercent && (percentRopa > 100 || percentRopa < 0)))
                {
                    Console.WriteLine("Incorrect data Introduced\n");
                    continue;
                }

                Console.WriteLine("<< Introduzca el Grado de Suciedad >>");
                Console.Write("[Muy Sucia, Sucia, Poco Sucia] o [0-100] : ");
                data = Console.ReadLine().Replace(" ", "").ToLower();
                suciaPercent = decimal.TryParse(data, out percentSucia);
                switch (data)
                {
                    case "muysucia":
                        fuzzySucia = veryDirty;
                        suciaFuzzy = true;
                        break;
                    case "sucia":
                        fuzzySucia = dirty;
                        suciaFuzzy = true;
                        break;
                    case "pocosucia":
                        fuzzySucia = littleDirty;
                        suciaFuzzy = true;
                        break;
                    default:
                        fuzzySucia = null;
                        suciaFuzzy = false;
                        break;
                }
                if ((!suciaFuzzy && !suciaPercent) || (suciaPercent && (percentSucia < 0 || percentSucia > 100)))
                {
                    Console.WriteLine("Incorrect data Introduced\n");
                    continue;
                }

                //if ((ropaPercent && !suciedPercent) || (ropaFuzzy && !ropaFuzzy))
                //{
                //    Console.WriteLine("Both datas need to be of the same type to work properly");
                //    continue;
                //}

                Console.WriteLine("<< Introduzca el Metodo de Agregacion >>");
                Console.Write("[Mamdani, Larsen] : ");
                data = Console.ReadLine().Replace(" ", "").ToLower();
                Aggregation agg;
                switch (data)
                {
                    case "mamdani":
                        agg = Aggregation.Mamdani;
                        break;
                    case "larsen":
                        agg = Aggregation.Larsen;
                        break;
                    default:
                        Console.WriteLine("Incorrect data Introduced\n");
                        continue;
                }

                Console.WriteLine("<< Introzca el Metodo de Desdifusificacion >>");
                Console.Write("[Centroid, Bisector, FOM, LOM, MOM] : ");
                data = Console.ReadLine().Replace(" ", "").ToLower();
                Defuzzification defuzz;
                switch (data)
                {
                    case "centroid":
                        defuzz = Defuzzification.Centroid;
                        break;
                    case "bisector":
                        defuzz = Defuzzification.Bisector;
                        break;
                    case "fom":
                        defuzz = Defuzzification.FOM;
                        break;
                    case "lom":
                        defuzz = Defuzzification.LOM;
                        break;
                    case "mom":
                        defuzz = Defuzzification.MOM;
                        break;
                    default:
                        Console.WriteLine("Incorrect data Introduced\n");
                        continue;
                }

                var result = system.Resolve(agg, defuzz, (ropaPercent, percentRopa, fuzzyRopa), (suciaPercent, percentSucia, fuzzySucia));
                //var result = system.Resolve(Aggregation.Mamdani, Defuzzification.Centroid, (false, -1, amountMedium), (true, 25, null));

                var intensidad = (int)decimal.Round(result[0]);
                var tiempo = Transform.MinFormat(result[1]);

                Console.WriteLine("\n<<< Resultados >>> ");
                Console.WriteLine("La intensidad de trabajo de la Lavadora es: {0}%", intensidad);
                Console.WriteLine("El tiempo total de Lavado es: {0}min", tiempo);
                data = Console.ReadLine();
                if (data == "q" || data == "quit") break;
            }

        }
    }
}
