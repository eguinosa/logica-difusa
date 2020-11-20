using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyLogic
{
    class Function
    {
        protected decimal m;
        protected decimal n;
        protected (decimal x, decimal y) point1;
        protected (decimal x, decimal y) point2;

        protected bool noM; //in case m is an undefined value;
        protected decimal noMX;

        public bool UndefinedM { get { return noM; } }

        public Function((decimal x, decimal y) a, (decimal x, decimal y) b)
        {
            point1 = a;
            point2 = b;

            if (a.x == b.x)
            {
                m = 0;
                n = b.y;
                noM = true;
                noMX = a.x;
            }
            else
            {
                m = (a.y - b.y) / (a.x - b.x);
                n = a.y - m * a.x;
            }
        }

        public decimal F(decimal x)
        {
            if (noM && x != noMX) throw new Exception("This is a function parallel to the Y axis");
            decimal result = m * x + n;
            return result;
        }

        public decimal Centroid()
        {
            return Centroid(point1.x, point2.x);
        }

        public decimal Centroid(decimal x1, decimal x2)
        {
            var result = IntegralXFx(x1, x2) / IntegralFx(x1, x2);
            return result;
        }

        public decimal IntegralFx(decimal a, decimal b)
        {
            if (noM) throw new Exception("The M of this function is Undefined");
            var result = m * (b * b - a * a) / 2 + n * (b - a);
            return result;
        }

        public decimal IntegralXFx(decimal a, decimal b)
        {
            if (noM) throw new Exception("The M of this function is Undefined");
            var result = m * (b * b * b - a * a * a) / 3 + n * (b * b - a * a) / 2;
            return result;
        }

        public (bool hasSolution, decimal result) Bisector()
        {
            return Bisector(point1.x, point2.x, 0, 0);
        }

        public (bool hasSolution, decimal result) Bisector(decimal x1, decimal x2)
        {
            return Bisector(x1, x2, 0, 0);
        }

        public (bool hasSolution, decimal result) Bisector(decimal x1, decimal x2, decimal leftArea, decimal rightArea)
        {
            if (noM) return (false, 0);
            if (m == 0)
            {
                if (n == 0)
                {
                    if (leftArea == rightArea) return (true, (x1 + x2) / 2);
                    return (false, 0);
                }
                var result = (n * (x1 + x2) + rightArea - leftArea) / (2 * n);
                return (true, result);
            }
            var a = m;
            var b = 2 * n;
            var c = -(m * (x1 * x1 + x2 * x2) / 2 + n * (x1 + x2) + rightArea - leftArea);

            var d = b * b - 4 * a * c;
            if (d < 0) return (false, 0);
            var root = (decimal)Math.Sqrt((double)d);

            var result1 = (-b + root) / (2 * a);
            var result2 = (-b - root) / (2 * a);

            if (x1 <= result1 && result1 <= x2) return (true, result1);
            if (x1 <= result2 && result2 <= x2) return (true, result2);
            else return (false, 0);
        }

        public (decimal x, decimal y) Intersection(Function f)
        {
            if (noM)
            {
                if (f.noM && noMX != f.noMX) throw new Exception("Both functions are parallel to the Y axis with no point in common");
                var y = f.F(noMX);
                return (noMX, y);
            }
            else if (f.noM)
            {
                var y = this.F(f.noMX);
                return (f.noMX, y);
            }
            else if (m == f.m) throw new Exception("Both functions are parallel to each other");
            else
            {
                var x = (f.n - n) / (m - f.m);
                var y = this.F(x);
                return (x, y);
            }
        }

        public static Function XFunction(decimal x)
        {
            var result = new Function((x, 1), (x, 0));
            return result;
        }

        public static Function YFunction(decimal y)
        {
            var result = new Function((0, y), (1, y));
            return result;
        }
    }
}
