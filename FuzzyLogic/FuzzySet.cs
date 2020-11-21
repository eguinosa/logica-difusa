using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyLogic
{
    class FuzzySet
    {
        public decimal SetLimitX { get; protected set; }

        public decimal MinValueX { get; protected set; }
        public decimal MaxValueX { get; protected set; }
        public decimal MaxValueY { get; protected set; }

        public IEnumerable<(decimal x, decimal y)> SetPoints { get { return points; } }

        protected List<Function> functions;
        protected List<(decimal x, decimal y)> points;


        public FuzzySet(decimal limitX, params (decimal x, decimal y)[] ps)
        {
            SetLimitX = limitX;

            if (ps.Length < 2) throw new Exception("2 or More points are required to create a Fuzzy Set");

            if (ps[0].x < 0) throw new Exception("The functions most be within the range of the Fuzzy Set");
            if (ps[ps.Length - 1].x > SetLimitX) throw new Exception("The functions most be within the range of the Fuzzy Set");
            if (ps[0].y != 0) throw new Exception("The function most start in the X axis (Y = 0)");
            if (ps[ps.Length - 1].y != 0) throw new Exception("The function most end in the X axis (Y = 0)");

            MinValueX = ps[0].x;
            MaxValueX = ps[ps.Length - 1].x;
            MaxValueY = ps[0].y;

            functions = new List<Function>();
            points = new List<(decimal x, decimal y)> { ps[0] };

            for (int i = 1; i < ps.Length; i++)
            {
                if (MaxValueY < ps[i].y) MaxValueY = ps[i].y;

                var a = ps[i - 1];
                var b = ps[i];

                if (a.x == b.x && a.y == b.y) throw new Exception("Points need to be different to create a Function");
                if (a.x > b.x) throw new Exception("X values most be sorted");

                points.Add(b);
                var f = new Function(a, b);
                functions.Add(f);
            }
        }

        public decimal F(decimal x)
        {
            if (x < MinValueX || x > MaxValueX) return 0;
            for (int i = 0; i < points.Count - 1; i++)
            {
                if (x >= points[i].x && x <= points[i + 1].x)
                {
                    var result = functions[i].F(x);
                    return result;
                }
            }
            return 0;
        }

        public decimal Centroid()
        {
            if (MaxValueY == 0) throw new Exception("The Fuzzy Set has no Area");

            decimal totalXFx = 0;
            decimal totalFx = 0;

            for (int i = 0; i < functions.Count; i++)
            {
                if (functions[i].UndefinedM) continue;

                var x1 = points[i].x;
                var x2 = points[i + 1].x;

                totalXFx += functions[i].IntegralXFx(x1, x2);
                totalFx += functions[i].IntegralFx(x1, x2);
            }

            var result = totalXFx / totalFx;
            return result;
        }

        public decimal Bisector()
        {
            if (MaxValueY == 0) throw new Exception("The Fuzzy Set has no Area");

            decimal totalArea = 0;
            var areas = new decimal[functions.Count];

            for (int i = 0; i < functions.Count; i++)
            {
                if (functions[i].UndefinedM) continue;
                var x1 = points[i].x;
                var x2 = points[i + 1].x;
                var area = functions[i].IntegralFx(x1, x2);
                totalArea += area;
                areas[i] = area;
            }

            decimal leftArea = 0;
            decimal rightArea = totalArea;

            for (int i = 0; i < areas.Length; i++)
            {
                rightArea -= areas[i];

                if (leftArea + areas[i] >= rightArea && leftArea <= areas[i] + rightArea)
                {
                    var (hasSolution, result) = functions[i].Bisector(points[i].x, points[i + 1].x, leftArea, rightArea);
                    if (hasSolution) return result;
                    else break;
                }

                leftArea += areas[i];
            }

            throw new Exception("No Solution found");
        }

        public decimal FirstOfMaximum()
        {
            if (MaxValueY == 0) throw new Exception("All Values of the function are 0");
            decimal x = 0;
            foreach (var item in points)
            {
                if (item.y == MaxValueY)
                {
                    x = item.x;
                    break;
                }
            }
            return x;
        }

        public decimal LastOfMaximum()
        {
            if (MaxValueY == 0) throw new Exception("All Values of the function are 0");
            decimal x = 0;
            foreach (var item in points)
            {
                if (item.y == MaxValueY)
                {
                    x = item.x;
                }
            }
            return x;
        }

        public decimal MeanOfMaxDiscrete()
        {
            if (MaxValueY == 0) throw new Exception("All Values of the function are 0");

            decimal maxvalue = 0;
            int countX = 0;
            int sumX = 0;

            int i = 0;
            int x = (int)decimal.Round(MinValueX);
            if (x < MinValueX) x++;

            while (x < MaxValueX)
            {
                if (x > points[i + 1].x || functions[i].UndefinedM)
                {
                    i++;
                    continue;
                }

                var fx = functions[i].F(x);
                if (fx == maxvalue)
                {
                    countX++;
                    sumX += x;
                }
                else if (fx > maxvalue)
                {
                    maxvalue = fx;
                    countX = 1;
                    sumX = x;
                }

                x++;
            }

            var result = (decimal)sumX / (decimal)countX;
            return result;
        }

        public FuzzySet Limit(decimal alpha)
        {
            if (alpha == 0) return new FuzzySet(SetLimitX, (0, 0), (SetLimitX, 0));
            if (alpha > MaxValueY) return new FuzzySet(SetLimitX, points.ToArray());

            var falpha = Function.YFunction(alpha);
            var newPoints = new List<(decimal x, decimal y)> { points[0] };
            int i = 1; //index for the points of the Fuzzy Set

            while (i < points.Count)
            {
                if (points[i].y <= alpha)
                {
                    newPoints.Add(points[i]);
                    i++;
                }
                else
                {
                    var newpointA = functions[i - 1].Intersection(falpha);
                    newPoints.Add(newpointA);
                    i++;
                    while (true)
                    {
                        if (points[i].y >= alpha)
                        {
                            i++;
                            continue;
                        }
                        else //points[i].y < alpha
                        {
                            var newPointB = functions[i - 1].Intersection(falpha);
                            newPoints.Add(newPointB);
                            newPoints.Add(points[i]);
                            i++;
                            break;
                        }
                    }
                }
            }

            var result = new FuzzySet(SetLimitX, newPoints.ToArray());
            return result;
        }

        public FuzzySet Multiply(decimal alpha)
        {
            if (alpha == 0) return new FuzzySet(SetLimitX, (0, 0), (SetLimitX, 0));
            if (alpha < 0) throw new Exception("Alpha must be a positive value");

            var newPoints = points.Select(item => (item.x, alpha * item.y));
            var result = new FuzzySet(SetLimitX, newPoints.ToArray());
            return result;
        }

        public static FuzzySet Union(FuzzySet a, FuzzySet b)
        {
            if (a.SetLimitX != b.SetLimitX) throw new Exception("Operation with different types of Fuzzy Sets");

            if (a.MaxValueY == 0) return new FuzzySet(b.SetLimitX, b.points.ToArray());
            if (b.MaxValueY == 0) return new FuzzySet(a.SetLimitX, a.points.ToArray());

            if (a.MaxValueX < b.MinValueX) return new FuzzySet(a.SetLimitX, a.points.Concat(b.points).ToArray());
            if (b.MaxValueX < a.MinValueX) return new FuzzySet(a.SetLimitX, b.points.Concat(a.points).ToArray());
            if (a.MaxValueX == b.MinValueX) return new FuzzySet(a.SetLimitX, a.points.Concat(b.points.Skip(1)).ToArray()); //sets start and end with y == 0
            if (b.MaxValueX == a.MinValueX) return new FuzzySet(a.SetLimitX, b.points.Concat(a.points.Skip(1)).ToArray());

            bool lastA = false, lastB = false;
            int i = 0, j = 0;
            var newPoints = new List<(decimal x, decimal y)>();

            if (a.MinValueX < b.MinValueX)
            {
                newPoints.Add(a.points[0]);
                lastA = true;
                i++;
            }
            else if (b.MinValueX < a.MinValueX)
            {
                newPoints.Add(b.points[0]);
                lastB = true;
                j++;
            }
            else // Equals MinValueX & by definition of this Fuzzy Set the y=0 for both
            {
                newPoints.Add(a.points[0]);
                lastA = true;
                lastB = true;
                i++;
                j++;
            }

            while (j == 0 && i < a.points.Count)
            {
                if (a.points[i].x < b.MinValueX)
                {
                    newPoints.Add(a.points[i]);
                    lastA = true;
                    i++;
                }
                else if (a.points[i].x == b.MinValueX)
                {
                    //if both y are 0 values <=> they are equals
                    if (a.points[i].y == b.points[0].y) lastB = true;
                    newPoints.Add(a.points[i]);
                    lastA = true;
                    i++;
                    j++;
                    break;
                }
                else // b.x > this.x & b.y = 0 for rule of this class and Fuzzy Set
                {
                    j++;
                    break;
                }
            }

            while (i == 0 && j < b.points.Count)
            {
                if (b.points[j].x < a.MinValueX)
                {
                    newPoints.Add(b.points[j]);
                    lastB = true;
                    j++;
                }
                else if (b.points[j].x == a.MinValueX)
                {
                    // if both Y are equal => both must be 0 by definition of this class
                    if (b.points[j].y == a.points[0].y) lastA = true;
                    newPoints.Add(b.points[j]);
                    lastB = true;
                    j++;
                    i++;
                    break;
                }
                else // this.x > b.x
                {
                    i++;
                    break;
                }
            }

            while (i < a.points.Count && j < b.points.Count)
            {
                if (a.points[i].x == b.points[j].x)
                {
                    if (a.points[i].y > b.points[j].y)
                    {
                        if (lastB && !lastA)
                        {
                            var p = a.functions[i - 1].Intersection(b.functions[j - 1]);
                            if (p != b.points[j - 1] && p != a.points[i])
                            {
                                newPoints.Add(p);
                            }
                        }
                        newPoints.Add(a.points[i]);
                        lastA = true;
                        lastB = false;
                        i++;
                        j++;
                    }
                    else if (b.points[j].y > a.points[i].y)
                    {
                        if (lastA && !lastB)
                        {
                            var p = b.functions[j - 1].Intersection(a.functions[i - 1]);
                            if (p != a.points[i - 1] && p != b.points[j])
                            {
                                newPoints.Add(p);
                            }
                        }
                        newPoints.Add(b.points[j]);
                        lastA = false;
                        lastB = true;
                        i++;
                        j++;
                    }
                    else // both are at the same point
                    {
                        newPoints.Add(a.points[i]);
                        lastA = true;
                        lastB = true;
                        i++;
                        j++;
                    }
                }
                else if (a.points[i].x < b.points[j].x)
                {
                    if (lastA)
                    {
                        if (lastB && a.points[i].y == b.functions[j - 1].F(a.points[i].x))
                        {
                            lastA = false;
                            i++;
                        }
                        else if (a.points[i].y >= b.functions[j - 1].F(a.points[i].x))
                        {
                            newPoints.Add(a.points[i]);
                            lastA = true;
                            lastB = false;
                            i++;
                        }
                        else
                        {
                            var p = a.functions[i - 1].Intersection(b.functions[j - 1]);
                            if (p != a.points[i - 1])
                            {
                                newPoints.Add(p);
                            }
                            lastA = false;
                            lastB = false;
                            i++;
                        }
                    }
                    else if (lastB)
                    {
                        if (a.points[i].y <= b.functions[j - 1].F(a.points[i].x))
                        {
                            lastA = false;
                            i++;
                        }
                        else
                        {
                            var p = a.functions[i - 1].Intersection(b.functions[j - 1]);
                            if (p != b.points[j - 1] && p != a.points[i])
                            {
                                newPoints.Add(p);
                            }
                            newPoints.Add(a.points[i]);
                            lastA = true;
                            lastB = false;
                            i++;
                        }
                    }
                    else // last point added was new
                    {
                        var f = new Function(newPoints[newPoints.Count - 1], b.points[j]);
                        if (a.points[i].y <= f.F(a.points[i].x))
                        {
                            lastA = false;
                            i++;
                        }
                        else
                        {
                            newPoints.Add(a.points[i]);
                            lastA = true;
                            lastB = false;
                            i++;
                        }
                    }
                }
                else // b.x < this.x
                {
                    if (lastB)
                    {
                        if (lastA && b.points[j].y == a.functions[i - 1].F(b.points[j].x))
                        {
                            lastB = false;
                            j++;
                        }
                        else if (b.points[j].y >= a.functions[i - 1].F(b.points[j].x))
                        {
                            newPoints.Add(b.points[j]);
                            lastA = false;
                            lastB = true;
                            j++;
                        }
                        else
                        {
                            var p = b.functions[j - 1].Intersection(a.functions[i - 1]);
                            if (p != b.points[j - 1])
                            {
                                newPoints.Add(p);
                            }
                            lastA = false;
                            lastB = false;
                            j++;
                        }
                    }
                    else if (lastA)
                    {
                        if (b.points[j].y <= a.functions[i - 1].F(b.points[j].x))
                        {
                            lastB = false;
                            j++;
                        }
                        else
                        {
                            var p = b.functions[j - 1].Intersection(a.functions[i - 1]);
                            if (p != a.points[i - 1] && p != b.points[j])
                            {
                                newPoints.Add(p);
                            }
                            newPoints.Add(b.points[j]);
                            lastA = false;
                            lastB = true;
                            j++;
                        }
                    }
                    else // last point added was new
                    {
                        var f = new Function(newPoints[newPoints.Count - 1], a.points[i]);
                        if (b.points[j].y <= f.F(b.points[j].x))
                        {
                            lastB = false;
                            j++;
                        }
                        else
                        {
                            newPoints.Add(b.points[j]);
                            lastA = false;
                            lastB = true;
                            j++;
                        }
                    }
                }
            }

            for (; i < a.points.Count; i++)
            {
                newPoints.Add(a.points[i]);
            }
            for (; j < b.points.Count; j++)
            {
                newPoints.Add(b.points[j]);
            }

            var result = new FuzzySet(a.SetLimitX, newPoints.ToArray());
            return result;
        }

        public static FuzzySet Intersection(FuzzySet a, FuzzySet b)
        {
            if (a.SetLimitX != b.SetLimitX) throw new Exception("Operation with different types of Fuzzy Sets");

            if (a.MaxValueY == 0 || b.MaxValueY == 0) return new FuzzySet(a.SetLimitX, (0, 0), (a.SetLimitX, 0));
            if (a.MaxValueX <= b.MinValueX || b.MaxValueX <= a.MinValueX) return new FuzzySet(a.SetLimitX, (0, 0), (a.SetLimitX, 0));

            bool lastA = false, lastB = false;
            int i = 0, j = 0;
            var newPoints = new List<(decimal x, decimal y)>();

            if (a.MinValueX > b.MinValueX)
            {
                newPoints.Add(a.points[0]);
                lastA = true;
                i++;
                j++;
                while (b.points[j].x < a.MinValueX && j < b.points.Count) j++;
            }
            else if (a.MinValueX < b.MinValueX)
            {
                newPoints.Add(b.points[0]);
                lastB = true;
                j++;
                i++;
                while (a.points[i].x < b.MinValueX && i < a.points.Count) i++;
            }
            else //they start at the same X value & by definition the Y value is 0 for both
            {
                newPoints.Add(a.points[0]);
                lastA = true;
                lastB = true;
                i++;
                j++;
            }

            while (i < a.points.Count && j < b.points.Count)
            {
                if (a.points[i].x < b.points[j].x)
                {
                    if (lastA)
                    {
                        if (a.points[i].y <= b.functions[j - 1].F(a.points[i].x))
                        {
                            newPoints.Add(a.points[i]);
                            lastA = true;
                            lastB = false;
                            i++;
                        }
                        else //a.f(x) > b.f => Create new point
                        {
                            if (lastB)
                            {
                                lastA = false;
                                i++;
                            }
                            else
                            {
                                var p = a.functions[i - 1].Intersection(b.functions[j - 1]);
                                newPoints.Add(p);
                                lastA = false;
                                lastB = false;
                                i++;
                            }
                        }
                    }
                    else if (lastB)
                    {
                        if (a.points[i].y >= b.functions[j - 1].F(a.points[i].x))
                        {
                            lastA = false;
                            i++;
                        }
                        else // Create New Point - x < b.f(x)
                        {
                            var p = a.functions[i - 1].Intersection(b.functions[j - 1]);
                            newPoints.Add(p);
                            newPoints.Add(a.points[i]);
                            lastA = true;
                            lastB = false;
                            i++;
                        }
                    }
                    else //new point was the last
                    {
                        var f = new Function(newPoints[newPoints.Count - 1], b.points[j]);
                        if (a.points[i].y >= f.F(a.points[i].x))
                        {
                            lastA = false;
                            i++;
                        }
                        else
                        {
                            newPoints.Add(a.points[i]);
                            lastA = true;
                            lastB = false;
                            i++;
                        }
                    }
                }
                else if (b.points[j].x < a.points[i].x)
                {
                    if (lastB)
                    {
                        if (b.points[j].y <= a.functions[i - 1].F(b.points[j].x))
                        {
                            newPoints.Add(b.points[j]);
                            lastA = false;
                            lastB = true;
                            j++;
                        }
                        else
                        {
                            if (lastA)
                            {
                                lastB = false;
                                j++;
                            }
                            else
                            {
                                var p = b.functions[j - 1].Intersection(a.functions[i - 1]);
                                newPoints.Add(p);
                                lastA = false;
                                lastB = false;
                                j++;
                            }
                        }
                    }
                    else if (lastA)
                    {
                        if (b.points[j].y >= a.functions[i - 1].F(b.points[j].x))
                        {
                            lastB = false;
                            j++;
                        }
                        else
                        {
                            var p = b.functions[j - 1].Intersection(a.functions[i - 1]);
                            newPoints.Add(p);
                            newPoints.Add(b.points[j]);
                            lastA = false;
                            lastB = true;
                            j++;
                        }
                    }
                    else
                    {
                        var f = new Function(newPoints[newPoints.Count - 1], a.points[i]);
                        if (b.points[j].y >= f.F(b.points[j].x))
                        {
                            lastB = false;
                            j++;
                        }
                        else
                        {
                            newPoints.Add(b.points[j]);
                            lastA = false;
                            lastB = true;
                            j++;
                        }
                    }
                }
                else // a.x == b.x
                {
                    if (a.points[i].y < b.points[j].y)
                    {
                        if (lastB)
                        {
                            var p = a.functions[i - 1].Intersection(b.functions[j - 1]);
                            newPoints.Add(p);
                        }
                        newPoints.Add(a.points[i]);
                        lastA = true;
                        lastB = false;
                        i++;
                        j++;
                    }
                    else if (b.points[j].y < a.points[i].y)
                    {
                        if (lastA)
                        {
                            var p = b.functions[j - 1].Intersection(a.functions[i - 1]);
                            newPoints.Add(p);
                        }
                        newPoints.Add(b.points[j]);
                        lastA = false;
                        lastB = true;
                        i++;
                        j++;
                    }
                    else
                    {
                        newPoints.Add(a.points[i]);
                        lastA = true;
                        lastB = true;
                        i++;
                        j++;
                    }
                }
            }

            var result = new FuzzySet(a.SetLimitX, newPoints.ToArray());
            return result;
        }

        protected static void DelRedundance(ref List<(decimal x, decimal y)> points)
        {
            int i = 1;
            while(i < points.Count)
            {
                if (points[i - 1] == points[i]) points.RemoveAt(i);
                else i++;
            }

            i = 2;
            while (i < points.Count)
            {
                var f = new Function(points[i - 2], points[i]);
                if (points[i - 1].y == f.F(points[i - 1].x)) points.RemoveAt(i - 1);
                else i++;
            }
        }

    }
}
