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
    class IfThenConstruction:NestedBlock
    {
        private static int counter = 0;
        public IfThenConstruction(BaseCondition condition, BaseConstruction[] body)
        {
            _condition = condition;
            _internalConstructions = body;
        }
        public override void AddToAssemblerCode()
        {
            int current = counter;
            counter++;
            string point = $"endif{current}";
            _condition.AddToAssemblerCode();
            CodeGenerator.AddNewInstruction($"{_condition.ReverseOperator} {point}");
            foreach (var item in _internalConstructions)
            {
                item.AddToAssemblerCode();
            }
            CodeGenerator.AddNewInstruction($"{point}:");
        }
    }
}
