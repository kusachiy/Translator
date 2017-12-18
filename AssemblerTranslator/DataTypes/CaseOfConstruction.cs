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
    class CaseOfConstruction : NestedBlock
    {
        private string _variable;
        private COInsideConstruction[] _body;

        private static int counter = 0;
        private static int conditionCounter = 0;

        public CaseOfConstruction(string variable,COInsideConstruction[] body)
        {
            _variable = variable;
            _body = body;
        }
        public override void AddToAssemblerCode()
        {
            int current = counter;
            int currentConditionCounter = conditionCounter;
            counter++;
            //CodeGenerator.AddNewInstruction($"while_begin_{current}:");
            //_condition.AddToAssemblerCode();
            //CodeGenerator.AddNewInstruction($"{_condition.ReverseOperator} endwhile_{current}");
            CodeGenerator.AddNewInstruction($"mov ax,{_variable}");
            foreach (var item in _body)
            {
                CodeGenerator.AddNewInstruction($"cmp ax,{item.Constant}");
                CodeGenerator.AddNewInstruction($"je handle_condition{currentConditionCounter}");
                currentConditionCounter++;
            }
            CodeGenerator.AddNewInstruction($"jmp endswitch{current}");
            currentConditionCounter = conditionCounter;
            foreach (var item in _body)
            {
                CodeGenerator.AddNewInstruction($"handle_condition{currentConditionCounter}:");
                foreach (var condition in item.Body)
                {
                    condition.AddToAssemblerCode();
                    CodeGenerator.AddNewInstruction($"jmp endswitch{current}");
                }
                currentConditionCounter++;
                conditionCounter++;
            }
            CodeGenerator.AddNewInstruction($"endswitch{current}:");
        }
        public override int CountOfRows => _body.Sum(b=>b.CountOfRows+1)+2;
    }
}
