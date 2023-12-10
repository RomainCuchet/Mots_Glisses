using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Mots_Glisses
{
    internal class Structures
    {
        public struct cmd
        {
            public Action function;
            public string prompt;
        }
    }
}
