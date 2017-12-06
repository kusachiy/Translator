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
        string[] _keyWords = { "if", "for", "while","print" };
        char[] _separators = { '(', ')', '{', '}',' ','=' };
        List<BaseVariable> _variables;
        List<BaseAssignment> _assignments;
        private List<BaseVariable> _printedValues= new List<BaseVariable>();
        private int caret;

        //public string GetValue => _printedValue;

        public List<BaseVariable> GetVariables => _variables;


        public CodeAnalyzer(string input)
        {
            _codeStrings = input.Split(new char[]{'\r','\n'},StringSplitOptions.RemoveEmptyEntries);
            _variables = new List<BaseVariable>();
            
        }

        public void StartAnalysis()
        {
            caret = 0;
            caret += VariablesAnalysis();
            if (_codeStrings[caret] != "begin")
                throw new Exception($"Ожидается 'begin' Строка №{caret + 1}");
            caret++;
            if (_codeStrings.Last() != "end")
                throw new Exception($"Ожидается 'end' Строка №{caret + 1}");
            //PrintAnalysis(_codeStrings.Length - 2);
        }
        public void AddAssignmentsCode()
        {
            while (caret < _codeStrings.Length - 1)
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
                    if(!IsIdent(item))
                        throw new Exception($"Не идентификатор. Строка №{caret +1}");
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
            var parts = str.Split();
            if (!_keyWords.Contains(parts[0].ToLower()))
            {
                AssignmentAnalysis(index);
                return 1;
            }
            else
            {
                if (parts[0].ToLower() == "print")
                {
                    var v = _variables.FirstOrDefault(s => s.Name == parts[1]);
                    if (v == null)
                        throw new Exception($"Неизвестная переменная. Строка №{caret+1}");
                    _printedValues.Add(v);
                    return 1;
                }
                List<string> body = new List<string>();                  
                
                if (parts[0].ToLower() == "if")
                {
                    string condition = "";
                    for (int i = 1; i < parts.Length - 1; i++)
                    {
                        condition += parts[i];
                    }
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
                    try
                    {
                        IfThenConstruction construction = new IfThenConstruction(condition, body.ToArray());
                        construction.AddToAssemblerCode();
                    }
                    catch (Exception e)
                    { throw new Exception($"Ошибка в ветвлении.{e.Message}. Строка {caret + 1}"); }
                    return body.Count+2;
                }
                else if (parts[0].ToLower() == "while")
                {
                    string condition = "";
                    for (int i = 1; i < parts.Length; i++)
                    {
                        condition += parts[i];
                    }
                    for (int i = index + 1; i < _codeStrings.Length; i++)
                    {
                        if (_codeStrings[i].ToLower().Trim() == "loop")
                            break;
                        body.Add(_codeStrings[i]);
                        if (i == _codeStrings.Length - 1)
                            throw new Exception("Loop не найдено");
                    }
                    
                    try
                    {
                        WhileConstruction construction = new WhileConstruction(condition, body.ToArray());
                        construction.AddToAssemblerCode();
                    }
                    catch (Exception e)
                    { throw new Exception($"Ошибка в цикле.{e.Message}. Строка {caret + 1}"); }
                    return body.Count + 2;
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
                throw new Exception($"Неизвестная переменная. Строка №{caret + 1}");
            string expression="";
            if (cVar.GetType == typeof(int))
            {
                AssignmentInt assignmentInt = new AssignmentInt(parts[0], parts[1]);
                try { assignmentInt.AddToAssemblerCode(); }
                catch{ throw new Exception($"Синтаксическая ошибка. Строка №{caret + 1}"); }
            }
            else
            {
                AssignmentBool assignment = new AssignmentBool(parts[0], parts[1]);
                try
                { assignment.AddToAssemblerCode(); }
                catch { throw new Exception($"Синтаксическая ошибка. Строка №{caret + 1}"); }
            }           
        }
        public void PrintResult()
        {
            foreach (var varr in _printedValues)
            {
                CodeGenerator.PrintValue(varr.Name);
            }
        }

        private bool IsIdent(string text)
        {
            if (!char.IsLetter(text[0]))
                return false;
            for (int i = 1; i < text.Length; i++)
            {
                if(!char.IsLetterOrDigit(text[i]))
                    return false;
            }
            return true;
        }
        
    }
}
