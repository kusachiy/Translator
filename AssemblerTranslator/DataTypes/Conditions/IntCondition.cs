using AssemblerTranslator.Analyzers;
using AssemblerTranslator.Expression.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerTranslator.Expression
{
    class IntCondition : ConditionBase
    {
        public override string Operator { get; set; } ="";

        public IntCondition(string input)
        {
            foreach (var item in _signs)
            {
                if (input.Contains(item))
                {
                    base._sign = item;
                    var parts = input.Split(new string[] { _sign }, StringSplitOptions.RemoveEmptyEntries);
                    base._leftPart = parts[0].Trim();
                    base._rightPart = parts[1].Trim();
                    break;
                }
            }
            if(_sign=="")
                throw new Exception("Не найдена операция сравнения");
            switch (_sign)
            {
                case "==":
                    Operator = "je";
                    break;
                case "!=":
                    Operator = "jne";
                    break;
                case "<":
                    Operator = "jl";
                    break;
                case "<=":
                    Operator = "jle";
                    break;
                case ">":
                    Operator = "jg";
                    break;
                case ">=":
                    Operator = "jge";
                    break;
            }
        }

        public override void AddToAssemblerCode()
        {
            CodeGenerator.CompariseIntExpressions(_leftPart, _rightPart);
        }
    }
}
