using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerTranslator.DataTypes
{
    public abstract class BaseConstruction : ITranslatable
    {
        public abstract void AddToAssemblerCode();
        public abstract int CountOfRows { get; }
    }
}
