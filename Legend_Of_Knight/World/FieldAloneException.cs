using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.World
{
    public class FieldAloneException : Exception
    {
        public FieldAloneException() : base("Ein Feld existierte mit einem oder weniger Nachbarn.")
        {

        }
    }
}
