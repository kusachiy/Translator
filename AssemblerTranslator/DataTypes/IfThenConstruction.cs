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
        public IfThenConstruction(string condition,string[] body)
        {
            _condition = new IntCondition(condition);
            _codeStrings = new BaseAssignment[body.Length];
                        
            for (int i = 0; i < body.Length; i++)
            {
                var parts = body[i].Split('=');
                _codeStrings[i] = new AssignmentInt(parts[0].Trim(),parts[1].Trim());
            }
        }

        public override void AddToAssemblerCode()
        {
            _condition.AddToAssemblerCode();
            CodeGenerator.AddNewInstruction($"{_condition.Operator} endif");
            foreach (var item in _codeStrings)
            {
                item.AddToAssemblerCode();
            }
            CodeGenerator.AddNewInstruction("endif:");
        }
    }
}
