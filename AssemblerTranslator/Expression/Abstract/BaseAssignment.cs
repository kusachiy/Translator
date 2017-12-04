﻿using AssemblerTranslator.DataTypes.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerTranslator.Expression.Abstract
{
    public abstract class BaseAssignment
    {
        public Type VariableType { get; set; }
        public string LeftPart { get; set; }
        public string RightPart { get; set; }
        public abstract void AddToAssemblerCode();
    }
}
