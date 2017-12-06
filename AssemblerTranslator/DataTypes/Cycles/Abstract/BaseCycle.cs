using AssemblerTranslator.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerTranslator.Cycles.Abstract
{
    public abstract class BaseCycle : ITranslatable
    {
        public void AddToAssemblerCode()
        {
            throw new NotImplementedException();
        }
    }
}
