using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperstarAPI
{
    // Container Klasse
    // In Doku nur erwähnen
    public class User
    {
        public string name;
        public List<User> following;

        public override string ToString()
        {
            return $"{name} follows {following.Count} users";
        }
    }
}
