using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP523_Zasetsky
{
    internal class Core
    {
        public static БдДляПр7Context Context = new БдДляПр7Context();
        public static БдДляПр7Context CreateContext()
        {
            return new БдДляПр7Context();
        }
    }
}
