using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileScan
{
    internal static class Scanner
    {
        private static string CurrentFile;
        private static string CurrentCount;

        public static (bool, IEnumerable<IEnumerable<string>>,string) TryRun(string code)
        {
            Func<IEnumerable<string>> filesFunction;
            var functions = new Stack<Func<IEnumerable<string>, IEnumerable<string>>>();

            // Parse the input string

            if (Parser.ParseString(ref code, out var value))
            {
                filesFunction = CreateFilesFunction(value, Parser.ParseFunction0(ref code, "recursive"));
            }
            else
            {
                return (false,null, "Error: first part must be a quoted string, e.g. 'D:\\Github\\*.cs'");
            }

            while (code != "")
            {
                if (Parser.ParseFunction0(ref code, "lines"))
                {
                    functions.Push(CreateLinesFunction());
                }
                else if (Parser.ParseFunction0(ref code, "count"))
                {
                    functions.Push(CreateCountFunction());
                }
                else if (Parser.ParseFunction1(ref code, "present", out var param))
                {
                    functions.Push(CreatePresentFunction(param));
                }
                else if (Parser.ParseFunction1(ref code, "containing", out param))
                {
                    functions.Push(CreateContainingFunction(param));
                }
                else
                {
                    return (false,null, $"Error: failed to parse {code}");
                }
            }

            // Run the resulting code
            return (true, Run(filesFunction, functions), "");
        }

        private static IEnumerable<IEnumerable<string>> Run(Func<IEnumerable<string>> filesFunction, Stack<Func<IEnumerable<string>, IEnumerable<string>>> functions)
        {
            foreach (var f in filesFunction())
            {
                CurrentFile = f;
                yield return Run(functions);
            }
        }

        private static IEnumerable<string> Run(Stack<Func<IEnumerable<string>, IEnumerable<string>>> functions)
        {
            if (functions.Any())
            {
                var fun = functions.Pop();
                return fun(Run(functions));
            }
            return new List<string> { CurrentFile };
        }


        private static Func<IEnumerable<string>> CreateFilesFunction(string startDir, bool recursive)
        {
            string dir = Directory.Exists(startDir) ? startDir : Path.GetDirectoryName(startDir);
            string searchPattern = Directory.Exists(startDir) ? "*.*" : Path.GetFileName(startDir);
            var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            if (Directory.Exists(dir))
            {
                return () => Directory.EnumerateFiles(dir, searchPattern, searchOption);
            }
            return () => new List<string> { "Error: directory not found" };
        }

        private static Func<IEnumerable<string>, IEnumerable<string>> CreateLinesFunction()
        {
            return linesInFiles;

            static IEnumerable<string> linesInFiles(IEnumerable<string> fileNames)
            {
                foreach(var file in fileNames)
                {
                    var lines = new List<string>();
                    try
                    {
                        foreach (var line in File.ReadLines(file))
                        {
                            lines.Add(line);
                        }
                    }
                    catch
                    {
                        continue;
                    }

                    CurrentFile = file;
                    foreach (var line in lines)
                    {
                        yield return line;
                    }
                }
            }
        }

        private static Func<IEnumerable<string>,IEnumerable<string>> CreateContainingFunction(string match)
        {
            return input => input.Where(s => s.Contains(match));
        }

        private static Func<IEnumerable<string>, IEnumerable<string>> CreateCountFunction()
        {
            return count;

            IEnumerable<string> count(IEnumerable<string> items)
            {
                CurrentCount = $"{items.Count()}";
                yield return CurrentCount;
            }
        }

        private static Func<IEnumerable<string>, IEnumerable<string>> CreatePresentFunction(string expression)
        {
            return present;

            IEnumerable<string> present(IEnumerable<string> items)
            {
                foreach (var item in items)
                {
                    yield return expression
                        .Replace("{count}", CurrentCount)
                        .Replace("{file}", CurrentFile)
                        .Replace("{line}", item);
                }
            }
        }

    }
}
