using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils
{
    class VectorNot2DException : Exception
    {
        public VectorNot2DException() : base("Vector did not have 2 or more Dimensions")
        {
            
        }
    }
}
