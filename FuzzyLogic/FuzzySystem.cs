using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyLogic
{
    class FuzzySystem
    {
        protected FuzzyControl[] inference;

        public FuzzySystem(params (FuzzySet[] premises, FuzzySet[] conculsions)[] rules)
        {
            if (rules.Length < 1) throw new Exception("There has to be at least one rule");

            inference = new FuzzyControl[rules[0].conculsions.Length];

            for (int i = 0; i < rules[0].conculsions.Length; i++)
            {
                var newRules = new FuzzyRule[rules.Length];
                for (int j = 0; j < rules.Length; j++)
                {
                    newRules[j] = new FuzzyRule(rules[j].premises, rules[j].conculsions[i]);
                }
                inference[i] = new FuzzyControl(newRules);
            }
        }

        public decimal[] Resolve(Aggregation agg, Defuzzification def, params decimal[] vs)
        {
            var results = inference.Select(control => control.Resolve(agg, def, vs));
            return results.ToArray();
        }

        public decimal[] Resolve(Aggregation agg, Defuzzification def, params FuzzySet[] vs)
        {
            var results = inference.Select(control => control.Resolve(agg, def, vs));
            return results.ToArray();
        }
    
        public decimal[] Resolve(Aggregation agg, Defuzzification def, params (bool hasValue, decimal value, FuzzySet set)[] vs)
        {
            var results = inference.Select(control => control.Resolve(agg, def, vs));
            return results.ToArray();
        }
    }
}
