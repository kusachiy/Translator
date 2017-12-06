using AssemblerTranslator.Expression.Abstract;
using System;
using System.Threading.Tasks;
using AssemblerTranslator.Analyzers;

namespace AssemblerTranslator.Expression
{
    class AssignmentInt : BaseAssignment
    {
        public AssignmentInt(string leftPart,string rightPart)
        {
            LeftPart = leftPart.Trim();
            RightPart = rightPart.Trim();
        }

        public override void AddToAssemblerCode()
        {
            RightPart = PolishNotationAnalyzer.GetExpression(RightPart);
            CodeGenerator.InsertIntExpression(LeftPart, RightPart);
        }


    }
}
