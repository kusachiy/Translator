using AssemblerTranslator.DataTypes.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerTranslator.DataTypes
{
    class MyIntVariable : BaseVariable
    {
        public MyIntVariable(string name,int value)
        {
            Name = name;
            Value = value;
        }
        public MyIntVariable(string name)
        {
            Name = name;
        }
        public override Type GetType => typeof(int);
    }
}
