using AssemblerTranslator.Analyzers;
using AssemblerTranslator.DataTypes.Conditions;
using AssemblerTranslator.Expression;
using AssemblerTranslator.Expression.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerTranslator.DataTypes
{
    class WhileConstruction : NestedBlock
    {
        private static int counter = 0;
        public WhileConstruction(BaseCondition condition,BaseConstruction[] body)
        {
            _condition = condition;
            _internalConstructions = body;
        }
        public override void AddToAssemblerCode()
        {
            int current = counter;
            counter++;
            CodeGenerator.AddNewInstruction($"while_begin_{current}:");
            _condition.AddToAssemblerCode();
            CodeGenerator.AddNewInstruction($"{_condition.ReverseOperator} endwhile_{current}");
            foreach (var item in _internalConstructions)
            {
                item.AddToAssemblerCode();
            }
            CodeGenerator.AddNewInstruction($"jmp while_begin_{current}");
            CodeGenerator.AddNewInstruction($"endwhile_{current}:");
        }
    }
}
