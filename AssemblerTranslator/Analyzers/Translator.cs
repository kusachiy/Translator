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
            StartAnalysis();
            CodeGenerator.WriteVariables(_codeAnalyzer.GetVariables);
            CodeGenerator.WriteSegmentsOfStackAndCode();
            WriteAssignments();
            _codeAnalyzer.PrintResult();
            CodeGenerator.WriteEndOfMainProcedure();
            CodeGenerator.WritePrintValueProcedure();
            CodeGenerator.WriteEndOfCode();
        }
        private void StartAnalysis()
        {
            _codeAnalyzer.StartAnalysis();
        }
        private void WriteAssignments()
        {
            _codeAnalyzer.AddAssignmentsCode();
        }
        public string GetVariablesValues()
        {
            return _codeAnalyzer.GetLog;
        }
        public string GetAssemblerCode => CodeGenerator.Generate();
    }
}
