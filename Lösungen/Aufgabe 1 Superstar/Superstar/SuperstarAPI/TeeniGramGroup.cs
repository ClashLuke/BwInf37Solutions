using RandomHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SuperstarAPI
{
    public class TeeniGramGroup
    {
        public SuperstarOutput output;
        public List<User> users; // public for UI
        
        public User this[string name] => users.Find(x => x.name == name);
        public int Count => users.Count();

        public TeeniGramGroup(int count, SuperstarOutput output = null, bool lastNames = false, double propability = 0.8d)
        {
            this.output = output;

            Random random = new Random();
            ListRandom<string> nameRandom = new ListRandom<string>(names, random);

            users = Enumerable.Repeat(new User(), count).Select(x => new User { name = nameRandom.Next(), }).ToList();

            if (!lastNames) users.ForEach(x => x.name = x.name.Split(' ')[0]);

            users.ForEach(user =>
            {
                ListRandom<User> listRandom = new ListRandom<User>(users
                                                                   .Where(x => x.name != user.name)
                                                                   .ToList(), random);
                user.following = listRandom.Next(random.Next(0, listRandom.Count)).ToList();
            });

            if (random.NextDouble() < propability)
            {
                int pos = random.Next(0, users.Count);

                users.ForEach(delegate (User user)
                {
                    if (!user.following.Contains(users[pos])) user.following.Add(users[pos]);
                });

                users[pos].following.Clear();
            }
        }
        
        public TeeniGramGroup(string[] file, SuperstarOutput output = null)
        {
            this.output = output;

            users = file[0]
                .Split(' ')
                .Select(name => new User { name = name, following = new List<User>() })
                .ToList();

            for (int i = 1; i < file.Length; i++)
            {
                string[] args = file[i].Split(' ');

                this[args[0]].following.Add(this[args[1]]);
            }
        }
        
        public bool Follows(User a, User b, ref int bill)
        {
            bool following = a.following.Contains(b);

            bill++;

            output?.following(a.name, b.name, following, bill);
            return following;
        }

        public string[] Serialize()
        {
            List<string> lines = new List<string> {
                users
                .Select(x => x.name)
                .Aggregate((current, next) => $"{current} {next.Split(' ')[0]}")
            };

            users.ForEach(user => user.following.ForEach(
                followed => lines.Add($"{user.name.Split(' ')[0]} {followed.name.Split(' ')[0]}")));

            return lines.ToArray();
        }

        // Generated using https://www.randomlists.com/random-names
        private List<string> names = Properties.Resources.Names
            .Split('\n')
            .Select(x => x.Replace("\n", "").Replace("\r", ""))
            .Distinct()
            .ToList();
    }
}