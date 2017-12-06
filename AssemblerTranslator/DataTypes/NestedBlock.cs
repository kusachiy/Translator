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
    public abstract class NestedBlock:ITranslatable
    {
        public int CountOfRows { get; private set; }
        protected ConditionBase _condition;
        protected BaseAssignment[] _codeStrings;

        public abstract void AddToAssemblerCode();      
    }
}
