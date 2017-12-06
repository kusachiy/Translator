using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerTranslator.DataTypes.Abstract
{
    public abstract class BaseVariable
    {
        public string Name { get; set; }
        public ValueType Value { get; set; }
        public new virtual Type GetType => Value?.GetType();
    }
}
