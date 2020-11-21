using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyLogic
{
    class FuzzyControl
    {
        protected FuzzyRule[] rules;

        public FuzzyControl(FuzzyRule[] rules)
        {
            if (rules.Length < 1) throw new Exception("There has to be at least one rule");

            for (int i = 0; i < rules.Length - 1; i++)
            {
                if (!rules[i].Compatible(rules[i + 1])) throw new Exception("The rules most be compatible with each other");
            }

            this.rules = rules;
        }

        public FuzzySet Mamdani(decimal[] vs)
        {
            var mamdani = rules.Select(rule => rule.Mamdani(vs));
            var result = mamdani.Aggregate((a, b) => FuzzySet.Union(a, b));
            return result;
        }

        public FuzzySet Mamdani(FuzzySet[] vs)
        {
            var mamdani = rules.Select(rule => rule.Mamdani(vs));
            var result = mamdani.Aggregate((a, b) => FuzzySet.Union(a, b));
            return result;
        }

        public FuzzySet Mamdani((bool hasValue, decimal value, FuzzySet set)[] vs)
        {
            var mamdani = rules.Select(rule => rule.Mamdani(vs));
            var result = mamdani.Aggregate((a, b) => FuzzySet.Union(a, b));
            return result;
        }

        public FuzzySet Larsen(decimal[] vs)
        {
            var larsen = rules.Select(rule => rule.Larsen(vs));
            var result = larsen.Aggregate((a, b) => FuzzySet.Union(a, b));
            return result;
        }

        public FuzzySet Larsen(FuzzySet[] vs)
        {
            var larsen = rules.Select(rule => rule.Larsen(vs));
            var result = larsen.Aggregate((a, b) => FuzzySet.Union(a, b));
            return result;
        }
        
        public FuzzySet Larsen((bool hasValue, decimal value, FuzzySet set)[] vs)
        {
            var larsen = rules.Select(rule => rule.Larsen(vs));
            var result = larsen.Aggregate((a, b) => FuzzySet.Union(a, b));
            return result;
        }

        public decimal Resolve(Aggregation agg, Defuzzification def, params decimal[] vs)
        {
            FuzzySet set;
            decimal result;

            switch (agg)
            {
                case Aggregation.Mamdani:
                    set = Mamdani(vs);
                    break;
                case Aggregation.Larsen:
                    set = Larsen(vs);
                    break;
                default:
                    set = Mamdani(vs);
                    break;
            }

            switch (def)
            {
                case Defuzzification.Centroid:
                    result = set.Centroid();
                    break;
                case Defuzzification.Bisector:
                    result = set.Bisector();
                    break;
                case Defuzzification.FOM:
                    result = set.FirstOfMaximum();
                    break;
                case Defuzzification.LOM:
                    result = set.LastOfMaximum();
                    break;
                case Defuzzification.MOM:
                    result = set.MeanOfMaxDiscrete();
                    break;
                default:
                    result = set.Centroid();
                    break;
            }

            return result;
        }

        public decimal Resolve(Aggregation agg, Defuzzification def, params FuzzySet[] vs)
        {
            FuzzySet set;
            decimal result;

            switch (agg)
            {
                case Aggregation.Mamdani:
                    set = Mamdani(vs);
                    break;
                case Aggregation.Larsen:
                    set = Larsen(vs);
                    break;
                default:
                    set = Mamdani(vs);
                    break;
            }

            switch (def)
            {
                case Defuzzification.Centroid:
                    result = set.Centroid();
                    break;
                case Defuzzification.Bisector:
                    result = set.Bisector();
                    break;
                case Defuzzification.FOM:
                    result = set.FirstOfMaximum();
                    break;
                case Defuzzification.LOM:
                    result = set.LastOfMaximum();
                    break;
                case Defuzzification.MOM:
                    result = set.MeanOfMaxDiscrete();
                    break;
                default:
                    result = set.Centroid();
                    break;
            }

            return result;
        }
    
        public decimal Resolve(Aggregation agg, Defuzzification def, params (bool hasValue, decimal value, FuzzySet set)[] vs)
        {
            FuzzySet set;
            decimal result;

            switch (agg)
            {
                case Aggregation.Mamdani:
                    set = Mamdani(vs);
                    break;
                case Aggregation.Larsen:
                    set = Larsen(vs);
                    break;
                default:
                    set = Mamdani(vs);
                    break;
            }

            switch (def)
            {
                case Defuzzification.Centroid:
                    result = set.Centroid();
                    break;
                case Defuzzification.Bisector:
                    result = set.Bisector();
                    break;
                case Defuzzification.FOM:
                    result = set.FirstOfMaximum();
                    break;
                case Defuzzification.LOM:
                    result = set.LastOfMaximum();
                    break;
                case Defuzzification.MOM:
                    result = set.MeanOfMaxDiscrete();
                    break;
                default:
                    result = set.Centroid();
                    break;
            }

            return result;
        }
    }
}
