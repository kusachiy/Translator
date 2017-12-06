using AssemblerTranslator.DataTypes.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerTranslator.DataTypes
{
    class MyBoolVariable : BaseVariable
    {
        public MyBoolVariable(string name,bool value)
        {
            Name = name;
            Value = value;
        }
        public MyBoolVariable(string name)
        {
            Name = name;
        }
        public override Type GetType => typeof(bool);
    }
}
