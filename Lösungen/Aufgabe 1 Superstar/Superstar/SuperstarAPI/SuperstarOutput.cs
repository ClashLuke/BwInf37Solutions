using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperstarAPI
{
    public class SuperstarOutput
    {
        public Action<string, int> exclusion, increasedPropability, investigate, found, failed;
        public Action<string, string, bool, int> following;
        public Action newline;
    }
}
