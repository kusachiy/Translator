using AssemblerTranslator.DataTypes;
using AssemblerTranslator.Expression.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerTranslator.Expression
{
    public abstract class BaseCondition:ITranslatable
    {
        protected static string[] _signs = {"<=",">=","==","!=", "<", ">" };
        protected string _leftPart,_rightPart,_sign = "";
        public bool IsTrue { get; set; }
        public abstract string Operator { get; set; }
        public abstract string ReverseOperator { get; set; }
        public abstract void AddToAssemblerCode();

    }
}
