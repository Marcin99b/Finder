using System;
using System.Collections.Generic;
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
                var startIndex = 0;
                var length = 2;

                while (startIndex <= lines.Count)
                {
                    var currentSubstring = line.Skip(startIndex).Take(length).ToList();

                    var containsSubstring = this.FindSubstringsWhereContains(currentSubstring, lines);
                    if (containsSubstring.Count >= 3)
                    {
                        results.Add(currentSubstring);
                    }

                    if (startIndex + length < line.Count)
                    {
                        length++;
                    }
                    else
                    {
                        startIndex++;
                        length = 2;
                    }
                }
            }

            return results.OrderByDescending(x => x.Count).First();
        }

        private List<List<string>> FindSubstringsWhereContains(List<string> current,
            List<List<string>> lines)
        {
            var results = lines.Where(x =>
            {
                var isEqual = false;
                var index = 0;
                foreach (var element in x)
                {
                    if (element == current[index])
                    {
                        isEqual = true;
                        index++;
                        if (index == current.Count)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        isEqual = false;
                        index = 0;
                    }
                }

                return false;
            }).ToList();

            return results;
        }
    }
}
