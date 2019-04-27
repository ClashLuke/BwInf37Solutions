using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SuperstarAPI
{
    public static class SuperSearch
    {
        public static User FindSuperstar(TeeniGramGroup group, out int finalBill, SuperstarOutput output = null)
        {
            int bill = 0;

            Dictionary<User, Dictionary<User, bool>> following = new Dictionary<User, Dictionary<User, bool>>();
            bool HasAsked(User a, User b) => following.ContainsKey(a) && following[a] != null && following[a].ContainsKey(b);
            bool Follows(User a, User b, out bool hasAsked)
            {
                if (!(hasAsked = HasAsked(a, b)))
                {
                    if (!following.ContainsKey(a)) following[a] = new Dictionary<User, bool>();

                    following[a].Add(b, group.Follows(a, b, ref bill));
                }
                return following[a][b];
            }

            Dictionary<User, int> propabilites = group.users.ToDictionary(x => x, x => 0);

            bool Check(User a, User b)
            {
                bool follows = Follows(a, b, out bool hasAsked);

                User userWithIncreasedPropability = follows ? b : a;
                User excludedUser = follows ? a : b;

                if (!hasAsked)
                {
                    if (propabilites[excludedUser] != -1)
                    {
                        output?.exclusion(excludedUser.name, propabilites[excludedUser]);
                        propabilites[excludedUser] = -1;
                    }
                    if (propabilites[userWithIncreasedPropability] != -1)
                    {
                        propabilites[userWithIncreasedPropability]++;
                        output?.increasedPropability(userWithIncreasedPropability.name, propabilites[userWithIncreasedPropability]);
                    }
                    output?.newline();
                }

                return follows;
            }

            bool Investigate(User toInvestigate, bool deep)
            {
                if (propabilites[toInvestigate] == -1) return false;

                List<User> indicies = group.users.Where(x => x != toInvestigate).ToList();
                while (indicies.Count > 0)
                {
                    User index = indicies.Aggregate((x, y) => propabilites[x] > propabilites[y] ? x : y);
                    indicies.Remove(index);

                    Check(toInvestigate, index);
                    if (propabilites[toInvestigate] == -1) return false;

                    if (!deep && propabilites[index] == -1) continue;

                    Check(index, toInvestigate);
                    if (propabilites[toInvestigate] == -1) return false;
                }

                return true;
            }                                                                                  
                                                                                                         
            while (true)                                                                                 
            {                                                                                            
                User highest = group.users.Aggregate((x, y) => propabilites[x] > propabilites[y] ? x : y);

                if (propabilites[highest] == -1)
                {
                    finalBill = bill;

                    output?.failed(highest.name, finalBill);
                    return null;
                }
                else
                {
                    output?.investigate(highest.name, propabilites[highest]);
                    if (Investigate(highest, false) && Investigate(highest, true))
                    {
                        finalBill = bill;
                        output?.found(highest.name, finalBill);
                        return highest;
                    }
                }
            }
        }
    }
}