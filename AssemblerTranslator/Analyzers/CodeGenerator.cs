using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssemblerTranslator.DataTypes.Abstract;

namespace AssemblerTranslator.Analyzers
{
    public static class CodeGenerator
    {
        private static List<string> _code;


        static CodeGenerator()
        {
            Reset();
        }

        public static void AddNewInstruction(string instruction)
        {
            _code.Add(instruction);
        }

        public static void Reset()
        {
            _code = new List<string>();
        }

        public static void WriteDataSegment()
        {
            AddNewInstruction("data segment para public \"data\"");
        }
        public static void WriteSegmentsOfStackAndCode()
        {
            AddNewInstruction("PRINT_BUF DB ' ' DUP(10)");
            AddNewInstruction("BUFEND    DB '$'");
            AddNewInstruction("data ends");
            AddNewInstruction("stk segment stack");
            AddNewInstruction("db 256 dup (\"?\")");
            AddNewInstruction("stk ends");
            AddNewInstruction("code segment para public \"code\"");
            AddNewInstruction("main proc");
            AddNewInstruction("assume cs:code,ds:data,ss:stk");
            AddNewInstruction("mov ax,data");
            AddNewInstruction("mov ds,ax");
        }
        public static void WriteEndOfMainProcedure()
        {
            AddNewInstruction("mov ax,4c00h");
            AddNewInstruction("int 21h");
            AddNewInstruction("main endp");
        }
        public static void WriteEndOfCode()
        {
            AddNewInstruction("code ends");
            AddNewInstruction("end main");
        }
        public static void WriteVariables(List<BaseVariable> variables)
        {
          
            foreach (var vr in variables)
            {
                AddNewInstruction(vr.Name + "  dw    1");
            }
        }
        public static void InsertIntExpression(string leftVar, string reversePolishNotationString)
        {
            for (int i = 0; i < reversePolishNotationString.Length; i++) //Для каждого символа в строке
            {                
                if (Char.IsLetter(reversePolishNotationString[i]))
                {
                    string a = string.Empty;

                    while (!PolishNotationAnalyzer.IsDelimeter(reversePolishNotationString[i]) && !PolishNotationAnalyzer.IsIntOperator(reversePolishNotationString[i])) //Пока не разделитель
                    {
                        a += reversePolishNotationString[i]; //Добавляем
                        i++;
                        if (i == reversePolishNotationString.Length) break;
                    }                    
                    AddValueToStack(a);
                    i--;
                }
                if (Char.IsDigit(reversePolishNotationString[i]))
                {
                    string a = string.Empty;

                    while (!PolishNotationAnalyzer.IsDelimeter(reversePolishNotationString[i]) && !PolishNotationAnalyzer.IsIntOperator(reversePolishNotationString[i])) //Пока не разделитель
                    {
                        a += reversePolishNotationString[i]; //Добавляем
                        i++;
                        if (i == reversePolishNotationString.Length) break;
                    }                   
                    AddValueToStack(a);
                    i--;
                }
                else if (PolishNotationAnalyzer.IsIntOperator(reversePolishNotationString[i])) //Если символ - оператор
                {                    
                    AddNewInstruction("pop bx");
                    AddNewInstruction("pop ax");
                    switch (reversePolishNotationString[i])
                    {
                        case '+':
                            AddNewInstruction("add ax, bx");
                            AddNewInstruction("push ax");
                            break;
                        case '-':
                            AddNewInstruction("sub ax, bx");
                            AddNewInstruction("push ax");
                            break;
                        case '*':
                            AddNewInstruction("mul bx");
                            AddNewInstruction("push ax");
                            break;
                        case '/':
                            AddNewInstruction("cwd");
                            AddNewInstruction("div bl");
                            AddNewInstruction("push ax");
                            break;
                    }

                }
            }
            AddNewInstruction("pop ax");
            AddNewInstruction("mov " + leftVar+ ", ax");

        }

        public static void CompariseIntExpressions(string leftExp, string rightExp)
        {
            InsertIntExpression("cx", PolishNotationAnalyzer.GetExpression(leftExp));
            InsertIntExpression("dx", PolishNotationAnalyzer.GetExpression(rightExp));
            AddNewInstruction("cmp cx, dx");
        }

        public static void InsertBoolExpression(string leftVar, string reversePolishNotationString)
        {
            for (int i = 0; i < reversePolishNotationString.Length; i++) //Для каждого символа в строке
            {               
                if (Char.IsLetter(reversePolishNotationString[i]))
                {
                    string a = string.Empty;

                    while (!PolishNotationAnalyzer.IsDelimeter(reversePolishNotationString[i]) && !PolishNotationAnalyzer.IsBoolOperator(reversePolishNotationString[i])) //Пока не разделитель
                    {
                        a += reversePolishNotationString[i]; //Добавляем
                        i++;
                        if (i == reversePolishNotationString.Length) break;
                    }
                    if (a.ToLower() == "true")
                        AddValueToStack("1");
                    else if (a.ToLower() == "false")
                        AddValueToStack("0");
                    else
                        AddValueToStack(a);
                    i--;
                }
                if (Char.IsDigit(reversePolishNotationString[i]))
                {
                    string a = string.Empty;

                    while (!PolishNotationAnalyzer.IsDelimeter(reversePolishNotationString[i]) && !PolishNotationAnalyzer.IsBoolOperator(reversePolishNotationString[i])) //Пока не разделитель
                    {
                        a += reversePolishNotationString[i]; //Добавляем
                        i++;
                        if (i == reversePolishNotationString.Length) break;
                    }
                    AddValueToStack(a);
                    i--;
                }
                else if (PolishNotationAnalyzer.IsBoolOperator(reversePolishNotationString[i])) //Если символ - оператор
                {
                    if (reversePolishNotationString[i] == '!')
                    {
                        AddNewInstruction("pop ax");
                        AddNewInstruction("xor ax,00000001b");
                        AddNewInstruction("push ax");
                    }
                    else
                    {
                        AddNewInstruction("pop bx");
                        AddNewInstruction("pop ax");
                        switch (reversePolishNotationString[i])
                        {
                            case '&':
                                AddNewInstruction("and ax, bx");
                                AddNewInstruction("push ax");
                                break;
                            case '|':
                                AddNewInstruction("or ax, bx");
                                AddNewInstruction("push ax");
                                break;
                            case '^':
                                AddNewInstruction("xor bx");
                                AddNewInstruction("push ax");
                                break;
                        }
                    }
                }
            }
            AddNewInstruction("pop ax");
            AddNewInstruction("mov " + leftVar + ", ax");
        }


        public static void WritePrintValueProcedure()
        {           
                AddNewInstruction("PRINT PROC NEAR");
                AddNewInstruction("MOV   CX, 10");
                AddNewInstruction("MOV   DI, BUFEND - PRINT_BUF");
                AddNewInstruction("PRINT_LOOP:");
                AddNewInstruction("MOV   DX, 0");
                AddNewInstruction("DIV   CX");
                AddNewInstruction("ADD   DL, '0'");
                AddNewInstruction("MOV   [PRINT_BUF + DI - 1], DL");
                AddNewInstruction("DEC   DI");
                AddNewInstruction("CMP   AL, 0");
                AddNewInstruction("JNE   PRINT_LOOP");
                AddNewInstruction("LEA   DX, PRINT_BUF");
                AddNewInstruction("ADD   DX, DI");
                AddNewInstruction("MOV   AH, 09H");
                AddNewInstruction("INT   21H");
                AddNewInstruction("RET");
                AddNewInstruction("PRINT ENDP");       

        }

        public static void PrintValue(string _printedValue)
        {
            AddNewInstruction("push ax");
            AddNewInstruction("mov ax, " + _printedValue);
            AddNewInstruction("CALL PRINT");
            AddNewInstruction("pop ax");
        }


   


        public static string Generate()
        {
            string code = "";
            foreach (var str in _code)
            {
                code += str+'\n';
            }
            return code;
        }

        private static void AddValueToStack(string value)
        {
            AddNewInstruction("mov ax," + value);
            AddNewInstruction("push ax");
        }

    }
}
