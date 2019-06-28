using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Finder
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.txt");
            var lines = File.ReadAllLines(path).Select(x => x.Split(" ").ToList()).ToList();

            var substringsService = new SubstringsService();
            var result = substringsService.FindBiggestSubstring(lines);

            Console.WriteLine(string.Join(" ", result));
            Console.ReadKey();
        }
    }

    class SubstringsService
    {
        public IEnumerable<string> FindBiggestSubstring(List<List<string>> lines)
        {
            var results = new List<List<string>>();

            foreach (var line in lines)
            {
                var index = 0;
                var length = 2;

                while (index <= lines.Count)
                {
                    var currentSubstring = line.Skip(index).Take(length).ToList();

                    if (!results.Contains(currentSubstring) && lines.FindSubstringsWhereContains(currentSubstring).Count >= 3)
                    {
                        results.Add(currentSubstring);
                    }

                    if (index + length < line.Count)
                    {
                        length++;
                    }
                    else
                    {
                        index++;
                        length = 2;
                    }
                }
            }

            return results.OrderByDescending(x => x.Count).First();
        }

    }

    static class SubstringExtensions
    {
        public static List<List<string>> FindSubstringsWhereContains(this List<List<string>> lines, List<string> current)
        {
            var results = lines.Where(x =>
            {
                var index = 0;
                foreach (var element in x)
                {
                    if (element == current[index])
                    {
                        index++;
                        if (index == current.Count)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        index = 0;
                    }
                }

                return false;
            }).ToList();

            return results;
        }
    }
}
