using SomeExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TwistAPI
{
    public class LangCook
    {
        public static readonly LangCook BwInfDefaultGerman = new LangCook("LangCooks/Default/bwinfDefaultGerman.langcook", false, "../../../../App/LangCooks/Default/bwinfDefaultGerman.txt", "LangCooks/Default/bwinfDefaultGerman.txt");

        private Dictionary<string, List<string>> words;
        public List<string> this[string c] => words.GetValueOrDefault(c, new List<string>());

        public LangCook()
        {
            words = new Dictionary<string, List<string>>();
        }
        
        public LangCook(string langcookPath, bool rewrite = false, params string[] paths) : this()
        {
            if (!rewrite && File.Exists(langcookPath))
            {
                LoadLangcook(langcookPath);
            }
            else
            {
                foreach (string path in paths) IncludeFile(path);
                WriteLangcook(langcookPath, this);
            }
        }

        public void LoadLangcook(string langcookPath)
        {
            string[] file = File.ReadAllLines(langcookPath, Encoding.UTF8);

            try
            {
                for (int pointer = 0; pointer < file.Length;)
                {
                    string[] args = file[pointer].Split('#');
                    int count = int.Parse(args[0]);
                    string[] result = new string[count];
                    Array.Copy(file, ++pointer, result, 0, count);

                    if (words.ContainsKey(args[1]) && words[args[1]] != null) words[args[1]].AddRange(result);
                    else words[args[1]] = result.ToList();

                    pointer += count;
                }
            }
            catch { throw new Exception("Input .langcook file was in an incorrect format"); }
        }

        public static LangCook WriteLangcook(string langcookPath, LangCook lang = null)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(langcookPath));
            File.WriteAllLines(
                langcookPath,
                lang.words.SelectMany(
                pair => new[] { $"{pair.Value.Count}#{pair.Key}" }.Union(pair.Value))
                .ToList(),
                Encoding.UTF8);
            return lang;
        }

        public static Regex toWords;

        public void IncludeFile(string path)
        {
            path = Path.GetFullPath(path);
            if (!File.Exists(path) && !Directory.Exists(path)) return;
            if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
            {
                foreach (string subPath in Directory.GetFiles(path)) IncludeFile(path);  // Ordner rekursiv durchsuchen
            }
            else
            {
                if (toWords == null) toWords = new Regex(@"([a-zA-ZäöüÄÖÜß]{4,})", RegexOptions.Compiled | RegexOptions.IgnoreCase); // Keine Worte mit 3 oder weniger Buchstaben speichern
                foreach (Match match in toWords.Matches(" " + File.ReadAllText(path, Encoding.UTF8) + " "))
                {
                    string word = match.Groups[0].Value.ToLower(), key = GetKey(word);

                    if (!words.ContainsKey(key) || words[key] == null) words[key] = new List<string>();
                    words[key].AddIfNew(word);
                }
            }
        }

        public static string GetKey(string word) => $"{word[0]}{word[word.Length - 1]}{word.Length}";
    }
}