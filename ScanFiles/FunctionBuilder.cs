using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ScanFiles
{
    public class FunctionBuilder
    {
        public string Error { get; private set; }

        public bool ParseInput(string code, out Func<IEnumerable<string>> filesFunction, out Func<string,IEnumerable<string>> linesFunction)
        {
            Error = "";
            if (ParseFilesFunction(ref code, out filesFunction) && ParseLinesFunction(ref code, out linesFunction))
            {
                return true;
            }
            filesFunction = null;
            linesFunction = null;
            return false;
        }

        protected virtual IEnumerable<string> EnumerateFiles(string baseDir, bool recursive)
        {
            string dir = Directory.Exists(baseDir) ? baseDir : Path.GetDirectoryName(baseDir);
            string searchPattern = Directory.Exists(baseDir) ? "*.*" : Path.GetFileName(baseDir);
            var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            if (Directory.Exists(dir))
            {
                return Directory.EnumerateFiles(dir, searchPattern, searchOption);
            }
            return Enumerable.Empty<string>();
        }

        protected virtual IEnumerable<string> EnumerateLines(string fileName)
        {
            return File.ReadLines(fileName);
        }

        private bool ParseFilesFunction(ref string code, out Func<IEnumerable<string>> function)
        {
            if (ParseFilesDefinition(ref code, out var fn))
            {
                while (Parser.ParseFunction1(ref code, "containing", out var match))
                {
                    fn = AttachContainsFunctionTo(fn, match);
                }
                if (Parser.ParseFunction0(ref code, "count"))
                {
                    if (code == "")
                    {
                        fn = AttachCountFunctionTo(fn);
                    }
                    else
                    {
                        Error = "count() must be the last function in the code";
                        function = null;
                        return false;
                    }
                }

                function = fn;
                return true;
            }

            function = null;
            return false;
        }

        private bool ParseLinesFunction(ref string code, out Func<string,IEnumerable<string>> function)
        {
            function = null;
            var parsedSuccessfully = true;

            if (Parser.ParseFunction0(ref code, "lines"))
            {
                Func<string, IEnumerable<string>> fn = EnumerateLines;

                do
                {
                    while (Parser.ParseFunction1(ref code, "containing", out var match))
                    {
                        fn = AttachContainsFunctionTo(fn, match);
                    }
                    parsedSuccessfully = false;
                }
                while (code != "" && parsedSuccessfully);

                if (Parser.ParseFunction0(ref code, "count"))
                {
                    fn = AttachCountFunctionTo(fn);
                }

                if (Parser.ParseFunction1(ref code, "present", out var pattern))
                {
                    fn = AttachPresentFunctionTo(fn, pattern);
                }

                function = fn;
            }

            if (code == "")
            {
                return true;
            }
            Error = $"unexpected code: {code}";
            return false;
        }

        private bool ParseFilesDefinition(ref string code, out Func<IEnumerable<string>> filesDefinitionFn)
        {
            if (Parser.ParseString(ref code, out var value))
            {
                var recursive = Parser.ParseFunction0(ref code, "recursive");
                filesDefinitionFn = () => EnumerateFiles(value, recursive);
                return true;
            }
            Error = "first part must be a quoted string, e.g. 'D:\\Github\\*.cs'";
            filesDefinitionFn = null;
            return false;
        }

        private Func<IEnumerable<string>> AttachContainsFunctionTo(Func<IEnumerable<string>> fn, string match)
        {
            return () => fn().Where(s => s.Contains(match));
        }

        private Func<string,IEnumerable<string>> AttachContainsFunctionTo(Func<string,IEnumerable<string>> fn, string match)
        {
            return input => fn(input).Where(s => s.Contains(match));
        }

        private Func<IEnumerable<string>> AttachCountFunctionTo(Func<IEnumerable<string>> fn)
        {
            return () => countFn(fn());

            IEnumerable<string> countFn(IEnumerable<string> items)
            {
                yield return $"{items.Count()}";
            }
        }

        private Func<string,IEnumerable<string>> AttachCountFunctionTo(Func<string,IEnumerable<string>> fn)
        {
            return s => countFn(fn(s));

            IEnumerable<string> countFn(IEnumerable<string> items)
            {
                yield return $"{items.Count()}";
            }
        }

        private Func<string, IEnumerable<string>> AttachPresentFunctionTo(Func<string, IEnumerable<string>> fn, string pattern)
        {
            return input => fn(input).Select(line => pattern.Replace("{line}", line));
        }
    }
}
