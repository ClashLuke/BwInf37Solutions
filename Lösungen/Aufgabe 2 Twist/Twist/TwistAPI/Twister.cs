using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwistAPI
{
    public static class Twister
    {
        #region Twisting
        public static string Twist(string text, Action<double> progressHandler, Random rand = null)
        {
            StringBuilder sb = new StringBuilder(), current = new StringBuilder();
            rand = rand ?? new Random(); // Neue Instanz generieren wenn keine oder null übergeben wurde
            double step = 1d / text.Length;
            double progress = 0;
            foreach (char c in text)
            {
                if (char.IsLetter(c)) current.Append(c);
                else
                {
                    TwistWord(current, sb, rand);
                    current.Clear();
                    sb.Append(c);
                }
                progressHandler(progress += step);
            }
            TwistWord(current, sb, rand); // Das letzte Wort würde ignoriert werden wenn das letzte Zeichen im Text ein Buchstabe ist
            progressHandler(1);

            return sb.ToString();
        }

        private static void TwistWord(StringBuilder text, StringBuilder sb, Random rand)
        {
            if (text.Length <= 3) // Worte mit 3 Buchstaben oder weniger können nicht getwistet werden
            {
                sb.Append(text);
            }
            else
            {
                sb.Append(text[0]); // Anfangsbuchstabe bleibt gleich
                for (int i = 1; text.Length > 2; i++)
                {
                    int pos = rand.Next(1, text.Length - 1);
                    sb.Append(text[pos]);
                    text = text.Remove(pos, 1);
                }
                sb.Append(text[1]); // Endbuchstabe bleibt gleich
            }
        }
        #endregion

        #region De-Twisting
        public static string DeTwist(string text, LangCook language, Action<double> progressHandler)
        {
            StringBuilder sb = new StringBuilder(), current = new StringBuilder();
            double step = 1d / text.Length;
            double progress = 0;
            foreach (char _char in text)
            {
                if (char.IsLetter(_char)) current.Append(_char);
                else
                {
                    DeTwistWordAndWriteSolutions(current, sb, language);
                    current.Clear();
                    sb.Append(_char);
                }
                progressHandler(progress += step);
            }
            DeTwistWordAndWriteSolutions(current, sb, language);
            progressHandler(1);

            return sb.ToString();
        }

        private static void DeTwistWordAndWriteSolutions(StringBuilder word, StringBuilder sb, LangCook language)
        {
            if (word.Length == 0) return;

            var solutions = DeTwistWord(word.ToString(), language);

            if (solutions.Count == 1) // Eindeutige Lösung gefunden
            {
                sb.Append(solutions[0]);
            }
            else if (solutions.Count > 1) // Mehrere Lösungen gefunden
            {
                sb.Append($"[AMBIGUOUSITY {solutions.Aggregate((x, y) => $"{x}, {y}")}]");
            }
            else // Keine Lösungen gefunden
            {
                sb.Append($"[ERROR {word.ToString()}]");
            } 
        }

        private static List<string> DeTwistWord(string text, LangCook language)
        {
            if (text.Length <= 3) // Worte mit maximal 3 Buchstaben können nicht getwistet werden, und somit auch nicht enttwistet
            {
                return new List<string> { text };
            }
            else
            {
                string textLower = text.ToLower();
                var remainder = language[LangCook.GetKey(textLower)]
                                .Select(x => new
                                {
                                    original = x,
                                    body = new StringBuilder(x.Substring(1, x.Length - 2))
                                })
                                .ToList();

                for (int i = 1; i < textLower.Length - 1; i++)
                {
                    for (int j = 0; j < remainder.Count; j++)
                    {
                        int index = remainder[j].body.ToString().IndexOf(textLower[i]);

                        if (index == -1)
                        {
                            remainder.RemoveAt(j--);
                        }
                        else
                        {
                            remainder[j].body.Remove(index, 1);
                        }
                    }
                }

                return remainder.Select(x => x.original.CopyCase(text)).ToList();
            }
        }

        private static string CopyCase(this string target, string source)
        {
            if (source.Length != target.Length) throw new ArgumentException("Source and Target have different sizes");
            return new string(Enumerable.Range(0, source.Length)
                .Select(x => char.IsUpper(source[x]) ? char.ToUpper(target[x]) : (char.IsLower(source[x]) ? char.ToLower(target[x]) : target[x])).ToArray());
        }
        #endregion
    }
}