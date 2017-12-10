using AssemblerTranslator.Analyzers;
using AssemblerTranslator.Expression;
using AssemblerTranslator.Expression.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerTranslator.DataTypes.Conditions
{
    public abstract class NestedBlock:BaseConstruction
    {
        protected BaseCondition _condition;
        protected BaseConstruction[] _internalConstructions;

        public override int CountOfRows => _internalConstructions.Select(c=>c.CountOfRows).Sum() + 2;
    }
}
