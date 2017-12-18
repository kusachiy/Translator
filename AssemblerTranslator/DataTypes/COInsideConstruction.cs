using AssemblerTranslator.Analyzers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerTranslator.DataTypes
{
    public class COInsideConstruction
    {
        public BaseConstruction[] Body { get; set; }
        public string Constant { get; set; }
        private static int counter = 0;
        public COInsideConstruction(string constant,BaseConstruction[] body)
        {
            this.Constant = constant;
            this.Body = body;
        }

        public int CountOfRows => Body.Length;      

    }
}
