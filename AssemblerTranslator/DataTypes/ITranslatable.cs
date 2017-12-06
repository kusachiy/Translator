using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerTranslator.DataTypes
{
    public interface ITranslatable
    {
        void AddToAssemblerCode();
    }
}
