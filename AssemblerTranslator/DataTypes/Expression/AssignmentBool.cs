using AssemblerTranslator.Analyzers;
using AssemblerTranslator.DataTypes;
using AssemblerTranslator.Expression.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerTranslator.Expression
{
    class AssignmentBool : BaseAssignment
    {
        public AssignmentBool(string leftPart, string rightPart)
        {
            LeftPart = leftPart.Trim(' ');
            RightPart = rightPart.Trim(' ');
        }
        public override void AddToAssemblerCode()
        {
            RightPart = PolishNotationAnalyzer.GetBoolExpression(RightPart);
            CodeGenerator.InsertBoolExpression(LeftPart,RightPart);
        }
    }
}
