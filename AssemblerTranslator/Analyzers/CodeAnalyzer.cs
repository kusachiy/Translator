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
        string[] _types = { "integer", "bool" };
        string[] _keyWords = { "if", "for","while", "case","print" };
        char[] _separators = { '(', ')', '{', '}',' ','=' };
        List<BaseVariable> _variables;
        List<BaseAssignment> _assignments;
        private List<BaseVariable> _printedValues= new List<BaseVariable>();
        private int caret;

        private List<BaseConstruction> _constructions;

        public List<BaseVariable> GetVariables => _variables;


        public CodeAnalyzer(string input)
        {
            if (input == null)
                return;
            _codeStrings = input.Split(new char[]{'\r','\n'},StringSplitOptions.RemoveEmptyEntries);
            _variables = new List<BaseVariable>();
            _constructions = new List<BaseConstruction>();
        }

        public void StartAnalysis()
        {
            caret = 0;
            caret += VariablesAnalysis();
            if (_codeStrings[caret] != "begin")
                throw new Exception($"Ожидается 'begin' Строка №{caret + 1}");
            caret++;
            while (caret < _codeStrings.Length - 1)
            {
                caret += UnknownConstructionAnalysis(caret);
            }
            if (_codeStrings.Last() != "end")
                throw new Exception($"Ожидается 'end' Строка №{_codeStrings.Length}");
            //PrintAnalysis(_codeStrings.Length - 2);
        }

        public void AddAssignmentsCode()
        {
            foreach (var block in _constructions)
            {
                try { block.AddToAssemblerCode(); }
                catch { throw new Exception($"Синтаксическая ошибка. Блок {block}"); }
            }           
        }

        private int VariablesAnalysis()
        {
            int i = 0;
            string type = "";
            while (_types.Contains(type=_codeStrings[i].Split()[0].ToLower()))//первое слово - тип
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
            if (i == 0)
                throw new Exception("Ожидается объявление переменных/ Ошибка в объявлении");
            return i;
        }      

        private int UnknownConstructionAnalysis(int index)
        {
            var construction = GetConstruction(index);
            _constructions.Add(construction);
            return construction.CountOfRows;
        }

        public BaseConstruction GetConstruction(int index)
        {
            var parts = _codeStrings[index].Trim().Split();
            var firstWord = parts[0];
            if (!_keyWords.Contains(firstWord.ToLower())) //если начинается с ключевого слова   
                return GetAssignment(index);
            else
            {
                if (firstWord.ToLower() == "print")
                {
                    var v = _variables.FirstOrDefault(s => s.Name == parts[1]);
                    if (v == null)
                        throw new Exception($"Неизвестная переменная. Строка №{index + 1}");
                    PrintConstruction printConstruction = new PrintConstruction(v.Name);                   
                    return printConstruction;
                }

                int height = 0;
                string conditionString = "";

                if (firstWord.ToLower() == "if")
                {                   
                    for (int i = 1; i < parts.Length - 1; i++)
                    {
                        conditionString += parts[i];
                    }
                    if (parts.Last().ToLower() != "then")
                        throw new Exception("Ожидается THEN");
                    for (int i = index + 1; i < _codeStrings.Length; i++)
                    {
                        height = i;
                        if (_codeStrings[i].ToLower().Trim() == "endif")
                            break;                       
                        if (i == _codeStrings.Length - 1)
                            throw new Exception("EndIf не найдено");
                    }
                    var condition = new IntCondition(conditionString);
                    var body = GetInternalConstructions(index + 1, height);
                    IfThenConstruction construction = new IfThenConstruction(condition, body);                 
                    return construction;
                }
                if (firstWord.ToLower() == "while")
                {
                    for (int i = 1; i < parts.Length; i++)
                    {
                        conditionString += parts[i];
                    }                   
                    for (int i = index + 1; i < _codeStrings.Length; i++)
                    {
                        height = i;
                        if (_codeStrings[i].ToLower().Trim() == "endwhile")
                            break;
                        if (i == _codeStrings.Length - 1)
                            throw new Exception("ENDWHILE не найдено");
                    }
                    var condition = new IntCondition(conditionString);
                    var body = GetInternalConstructions(index + 1,height);
                    WhileConstruction construction = new WhileConstruction(condition, body.ToArray());
                    return construction;
                }
                if (firstWord.ToLower() == "case")
                {
                    conditionString = parts[1];
                    if (parts.Last().ToLower() != "of")
                        throw new Exception("Ожидается OF");
                    for (int i = index + 1; i < _codeStrings.Length; i++)
                    {
                        height = i;
                        if (_codeStrings[i].ToLower().Trim() == "endcase")
                            break;
                        if (i == _codeStrings.Length - 1)
                            throw new Exception("ENDCASE не найдено");
                    }
                    var cVar = _variables.FirstOrDefault(v => v.Name == conditionString.Trim());
                    if (cVar == null)
                        throw new Exception($"Неизвестная переменная. Строка №{caret + 1}");
                    var body = GetInsideConstructions(index + 1, height-1);
                    CaseOfConstruction construction = new CaseOfConstruction(conditionString,body);
                    return construction;
                }

                throw new Exception($"Неизвестная конструкция. Строка {index + 1}");
            }
        }

        private BaseAssignment GetAssignment(int index)
        {
            var str = _codeStrings[index];
            var parts = str.Split('=');
            var cVar = _variables.FirstOrDefault(v => v.Name == parts[0].Trim());
            if (cVar==null)
                throw new Exception($"Неизвестная переменная. Строка №{index + 1}");
            string expression="";
            if (cVar.GetType == typeof(int))
            {
                AssignmentInt assignmentInt = new AssignmentInt(parts[0], parts[1]);
                return assignmentInt;
            }          
            AssignmentBool assignment = new AssignmentBool(parts[0], parts[1]);
            return assignment;
                    
        }
         private bool IsIdent(string text)
        {
            if (text == "")
                throw new Exception("Ошибка в опеределении переменных");
            if (!char.IsLetter(text[0]))
                return false;
            for (int i = 1; i < text.Length; i++)
            {
                if(!char.IsLetterOrDigit(text[i]))
                    return false;
            }
            return true;
        }
         private BaseConstruction[] GetInternalConstructions(int start,int end)
        {
            List<BaseConstruction> constructions = new List<BaseConstruction>();
            for (int i = start; i < end;)
            {
                var cnstr = GetConstruction(i);
                i += cnstr.CountOfRows;
                constructions.Add(cnstr);
            }
            return constructions.ToArray();
        }
        private COInsideConstruction[] GetInsideConstructions(int start, int end)
        {
            List<COInsideConstruction> constructions = new List<COInsideConstruction>();
            for (int i = start; i < end; i++)
            {
                List<BaseConstruction> list = new List<BaseConstruction>();
                string str = _codeStrings[i].Trim();
                var array = str.Split(':');
                if (array.Length < 2)
                    throw new Exception($"Ошибка в Case {caret + 1}");
                var cst = array[0];
                if (!IsNumber(cst))
                    throw new Exception($"Неизвестная переменная. Строка №{caret + 1}");
                
                while (i < end && !IsCaseCondition(_codeStrings[i+1]))
                {
                    i++;
                    var cnstr = GetConstruction(i);
                    list.Add(cnstr);
                }
                constructions.Add(new COInsideConstruction(cst, list.ToArray()));
            }
            return constructions.ToArray();
        }
        private bool IsCaseCondition(string str)
        {
            return str.Split(':').Length > 1;
        }
        private bool IsNumber(string number)
        {
            for (int i = 0; i < number.Length; i++)
            {
                if (!char.IsDigit(number[i]))
                    return false;
            }
            return true;
        }
    }
}
