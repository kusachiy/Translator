using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerTranslator.Analyzers
{
    class Translator
    {
        private string _code;
        private CodeAnalyzer _codeAnalyzer;
        public Translator(string code)
        {
            _code = code;
            _codeAnalyzer = new CodeAnalyzer(_code);
        }
        public void Compile()
        {
            CodeGenerator.Reset();
            CodeGenerator.WriteDataSegment();
            _codeAnalyzer.StartAnalysis();
            CodeGenerator.WriteVariables(_codeAnalyzer.GetVariables);
            CodeGenerator.WriteSegmentsOfStackAndCode();

            _codeAnalyzer.AddAssignmentsCode();
            _codeAnalyzer.PrintResult();

            CodeGenerator.WriteEndOfMainProcedure();
            CodeGenerator.WritePrintValueProcedure();
            CodeGenerator.WriteEndOfCode();
        }      
       
        public string GetAssemblerCode => CodeGenerator.Generate();
    }
}
