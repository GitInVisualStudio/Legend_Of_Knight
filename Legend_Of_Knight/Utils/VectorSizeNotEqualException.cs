using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils
{
    class VectorSizeNotEqualException : Exception
    {
        public VectorSizeNotEqualException() : base("Vectors did not have equally many dimensions")
        {
        }
    }
}
