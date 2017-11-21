using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerTranslator.Analyzers
{
    class CodeAnalyzer
    {

        string[] _codeStrings;
        string[] _types = { "int", "double" };
        Dictionary<string,double?> _variables;
        int _caret = 0;
        double _printedValue=0;

        public double GetDecimalValue => _printedValue;
        public string GetLog => VariablesLog();


        public CodeAnalyzer(string input)
        {
            _codeStrings = input.Split(new char[]{'\r','\n'},StringSplitOptions.RemoveEmptyEntries);
            _variables = new Dictionary<string, double?>();
        }

        public void StartAnalysis()
        {
            VariablesAnalysis();
            if (_codeStrings[1] != "begin")
                return;
            if (_codeStrings.Last() != "end")
                return;
            for (int i = 2; i < _codeStrings.Length - 2; i++)
            {
                AssignmentAnalysis(i);
            }
            PrintAnalysis(_codeStrings.Length - 2);

        }

        /// <summary>
        /// пока что всё в одну строку
        /// </summary>
        private void VariablesAnalysis()
        {
            var firstString = _codeStrings[0];
            var lexems = firstString.Split();
            if (!_types.Contains(lexems[0]))//первое слово - тип
                return;
            var buf = firstString.Substring(lexems[0].Length + 1).Trim().Split(',');
            foreach (var item in buf)
            {
                _variables.Add(item,null);
            }
        }

        private void AssignmentAnalysis(int index)
        {
            var str = _codeStrings[index];
            var parts = str.Split('=');
            if (!_variables.Keys.Contains(parts[0]))
                return;
            var expression = PolishNotationAnalyzer.GetExpression(parts[1]);
            _variables[parts[0]] = PolishNotationAnalyzer.Calculator(expression, _variables);
        }

        private void PrintAnalysis(int index)
        {
            var str = _codeStrings[index].Split() ;
            _printedValue = (double)_variables[str.Last()];
        }
        private string VariablesLog()
        {
            string result = "";
            foreach (var item in _variables)
            {
                result += string.Format("Переменная {0}, Значение {1} \r\n", item.Key, item.Value);
            }
            return result;
        }
        
    }
}
