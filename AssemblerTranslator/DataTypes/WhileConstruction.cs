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
        public WhileConstruction(string condition,string[] body)
        {
            _condition = new IntCondition(condition);
            _codeStrings = new BaseAssignment[body.Length];

            for (int i = 0; i < body.Length; i++)
            {
                var parts = body[i].Split('=');
                _codeStrings[i] = new AssignmentInt(parts[0].Trim(), parts[1].Trim());
            }
        }
        public override void AddToAssemblerCode()
        {
            CodeGenerator.AddNewInstruction("while_begin:");
            _condition.AddToAssemblerCode();
            CodeGenerator.AddNewInstruction($"{_condition.ReverseOperator} endwhile");
            foreach (var item in _codeStrings)
            {
                item.AddToAssemblerCode();
            }
            CodeGenerator.AddNewInstruction("jmp while_begin");
            CodeGenerator.AddNewInstruction("endwhile:");
        }
    }
}
