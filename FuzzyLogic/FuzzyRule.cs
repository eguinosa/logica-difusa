using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyLogic
{
    class FuzzyRule
    {
        protected FuzzySet[] premises;
        protected FuzzySet conclusion;

        public FuzzyRule(FuzzySet[] premises, FuzzySet conclusion)
        {
            if (premises.Length < 1) throw new Exception("There has to be at least one Premise");

            this.premises = premises;
            this.conclusion = conclusion;
        }

        public decimal Alpha(decimal[] vs)
        {
            if (vs.Length != premises.Length) throw new Exception("The values must be equal to the amount of Fuzzy Sets of the Premises");

            var minAlpha = premises[0].F(vs[0]);

            for (int i = 1; i < premises.Length; i++)
            {
                var newAlpha = premises[i].F(vs[i]);
                if (newAlpha < minAlpha) minAlpha = newAlpha;
            }

            return minAlpha;
        }

        public decimal Alpha(FuzzySet[] vs)
        {
            if (vs.Length != premises.Length) throw new Exception("The values must be equal to the amount of Fuzzy Sets of the Premises");

            var minAlpha = FuzzySet.Intersection(premises[0], vs[0]).MaxValueY;

            for (int i = 1; i < premises.Length; i++)
            {
                var newAlpha = FuzzySet.Intersection(premises[i], vs[i]).MaxValueY;
                if (newAlpha < minAlpha) minAlpha = newAlpha;
            }

            return minAlpha;
        }

        public FuzzySet Mamdani(decimal[] vs)
        {
            var alpha = Alpha(vs);
            var result = conclusion.Limit(alpha);
            return result;
        }

        public FuzzySet Mamdani(FuzzySet[] vs)
        {
            var alpha = Alpha(vs);
            var result = conclusion.Limit(alpha);
            return result;
        }

        public FuzzySet Larsen(decimal[] vs)
        {
            var alpha = Alpha(vs);
            var result = conclusion.Multiply(alpha);
            return result;
        }

        public FuzzySet Larsen(FuzzySet[] vs)
        {
            var alpha = Alpha(vs);
            var result = conclusion.Multiply(alpha);
            return result;
        }

        public bool Compatible(FuzzyRule rule)
        {
            if (premises.Length != rule.premises.Length) return false;
            if (conclusion.SetLimitX != rule.conclusion.SetLimitX) return false;
            for (int i = 0; i < premises.Length; i++)
            {
                if (premises[i].SetLimitX != rule.premises[i].SetLimitX) return false;
            }

            return true;
        }
    }
}
