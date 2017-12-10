using AssemblerTranslator.Analyzers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerTranslator.DataTypes
{
    class PrintConstruction : BaseConstruction
    {
        private string _printedValue;

        public PrintConstruction(string printedValue)
        {
            _printedValue = printedValue;
        }

        public override int CountOfRows => 1;

        public override void AddToAssemblerCode()
        {
            CodeGenerator.AddNewInstruction("push ax");
            CodeGenerator.AddNewInstruction("mov ax, " + _printedValue);
            CodeGenerator.AddNewInstruction("CALL PRINT");
            CodeGenerator.AddNewInstruction("pop ax");
        }      
    }
}
