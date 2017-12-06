using AssemblerTranslator.DataTypes;
using AssemblerTranslator.DataTypes.Abstract;
using AssemblerTranslator.DataTypes.Conditions;
using AssemblerTranslator.Expression;
using AssemblerTranslator.Expression.Abstract;
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
        string[] _types = { "int", "bool" };
        string[] _keyWords = { "if", "for", "while" };
        char[] _separators = { '(', ')', '{', '}',' ','=' };
        List<BaseVariable> _variables;
        List<BaseAssignment> _assignments;
        private string _printedValue;
        private int caret = 0;

        public string GetValue => _printedValue;
        public string GetLog => VariablesLog();
        public List<BaseVariable> GetVariables => _variables;


        public CodeAnalyzer(string input)
        {
            _codeStrings = input.Split(new char[]{'\r','\n'},StringSplitOptions.RemoveEmptyEntries);
            _variables = new List<BaseVariable>();
            
        }

        public void StartAnalysis()
        {
            int caret = 0;
            caret += VariablesAnalysis();
            if (_codeStrings[caret] != "begin")
                return;
            caret++;
            if (_codeStrings.Last() != "end")
                return;               
            //PrintAnalysis(_codeStrings.Length - 2);
        }
        public void AddAssignmentsCode()
        {
            while (caret < _codeStrings.Length - 2)
            {
                caret += StrongAssingmentAnalysis(caret);
            }
        }

        private int VariablesAnalysis()
        {
            int i = 0;
            string type = "";
            while (_types.Contains(type=_codeStrings[i].Split()[0]))//первое слово - тип
            {
                var fString = _codeStrings[i];
                var lexems = fString.Split();
                var buf = fString.Substring(lexems[0].Length + 1).Trim().Split(',');
                foreach (var item in buf)
                {
                    if (_types[0] == type)
                        _variables.Add(new MyIntVariable(item));
                    else
                        _variables.Add(new MyBoolVariable(item));
                }
                i++;
            }
            return i;
        }
        private int StrongAssingmentAnalysis(int index)
        {
            var str = _codeStrings[index];
            var parts = str.Split(_separators);
            if (!_keyWords.Contains(parts[0]))
            {
                AssignmentAnalysis(index);
                return 1;
            }
            else
            {
                List<string> body = new List<string>();
                string condition = "";
                for (int i = 1; i < parts.Length-1; i++)
                {
                    condition += parts[i];
                }
                if (parts[0].ToLower() == "if")
                {
                    if (parts.Last().ToLower() != "then")
                        throw new Exception("Ожидается THEN");
                    for (int i = index+1; i < _codeStrings.Length; i++)
                    {                        
                        if (_codeStrings[i].ToLower().Trim() == "endif")
                            break;
                        body.Add(_codeStrings[i]);
                        if (i == _codeStrings.Length - 1)
                            throw new Exception("EndIf не найдено");
                    }
                    IfThenConstruction construction = new IfThenConstruction(condition, body.ToArray());
                    construction.AddToAssemblerCode();
                    return body.Count+2;
                }
            }
            return 0;
        }
        private void AssignmentAnalysis(int index)
        {
            var str = _codeStrings[index];
            var parts = str.Split('=');
            var cVar = _variables.FirstOrDefault(v => v.Name == parts[0].Trim());
            if (cVar==null)
                return;
            string expression="";
            if (cVar.GetType == typeof(int))
            {
                AssignmentInt assignmentInt = new AssignmentInt(parts[0], parts[1]);
                assignmentInt.AddToAssemblerCode();
            }
            else
            {
                AssignmentBool assignment = new AssignmentBool(parts[0], parts[1]);
                assignment.AddToAssemblerCode();
            }           
        }
        public void PrintResult()
        {
            var str = _codeStrings[_codeStrings.Length - 2].Split() ;
            CodeGenerator.PrintValue(str[1]);
        }
        private string VariablesLog()
        {
            string result = "";
            foreach (var item in _variables)
            {
                result += string.Format("Переменная {0}, Значение {1} \r\n", item.Name, item.Value);
            }
            return result;
        }        
    }
}
