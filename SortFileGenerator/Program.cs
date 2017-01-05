using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortFileGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var size = ReadFileSize();
            var path = ReadFilePath();

            var start = DateTime.Now;
            GenerateFile(size, path);

            Console.WriteLine(string.Format("Completed in {0}", (DateTime.Now - start).TotalSeconds));
            Console.ReadKey();
        }

        private static long ReadFileSize()
        {
            Console.WriteLine("Enter file size in GBytes:");
            var sizeStr = Console.ReadLine();
            int size = 0;
            if (!int.TryParse(sizeStr, out size))
            {
                Console.WriteLine("Must be a number!");
                return ReadFileSize();
            }
            return (long) size * 1073741824;
        }

        private static string ReadFilePath()
        {
            Console.WriteLine("Enter absolute file path:");
            var path = Console.ReadLine();
            try
            {
                var writer = File.OpenWrite(path);
                writer.Dispose();
            }
            catch
            {
                Console.WriteLine("Invalid path or no permissions!");
                return ReadFilePath();
            }
            return path;
        }

        private static void GenerateFile(long size, string path)
        {
            var random = new Random();
            long currentSize = 0;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ";
            
            List<string> buffer = new List<string>();
            using (var writer = new StreamWriter(path))
            {
                while (currentSize < size)
                {
                    var randLength = random.Next(500);
                    var randNumber = random.Next(500000);
                    var line = string.Concat(randNumber, '.', GetRandom(chars, random));

                    buffer.Add(line);

                    // duplication of same number
                    if (randLength < 20)
                    {
                        buffer.Add(string.Concat(randNumber, '.', GetRandom(chars, random)));
                    }

                    if (buffer.Count > 100000)
                    {
                        var writePart = string.Join(Environment.NewLine, buffer);
                        writer.Write(writePart);
                        buffer.Clear();
                        currentSize += writePart.Length;
                    }
                }
            }
        }

        private static string GetRandom(string charSet, Random random)
        {
            return new string(Enumerable.Repeat(charSet, random.Next(500))
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
