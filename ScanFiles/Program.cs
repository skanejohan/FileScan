using System;

namespace ScanFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                DisplayHint();
                return;
            }

            var functionBuilder = new FunctionBuilder();
            if (functionBuilder.ParseInput(args[0], out var filesFunction, out var linesFunction))
            {
                foreach (var s in Runner.Run(filesFunction, linesFunction))
                {
                    Console.WriteLine(s);
                }
            }
            else
            {
                Console.WriteLine(functionBuilder.Error);
            }
        }

        static void DisplayHint()
        {
            Console.WriteLine();
            Console.WriteLine("Usage: ScanFiles <expression>");
            Console.WriteLine();
            Console.WriteLine(" An expression always starts with the name of a directory, including a possible search filter. Examples");
            Console.WriteLine(" are: 'C:\', 'D:\\Github' and 'D:\\Github\\*.cs'. Note that, like all strings, the directory is enclosed");
            Console.WriteLine(" with single quotes. These examples will list the files found in the specified directories.");
            Console.WriteLine();
            Console.WriteLine(" To specify that the search should be recursive, use the \".recursive()\" specification");
            Console.WriteLine();
            Console.WriteLine(" Please note that if any part of your expression contains spaces, you must enclose the whole expression");
            Console.WriteLine(" in double quotes. That is, you can call");
            Console.WriteLine("   ScanFiles 'D:\\Github'.recursive()");
            Console.WriteLine(" but you need double quotes when calling");
            Console.WriteLine("   ScanFiles \"'D:\\Program Files\\Github'.recursive()\"");
            Console.WriteLine();
            Console.WriteLine(" To include only files containing a certain string, use the \".containing()\" function.");
            Console.WriteLine();
            Console.WriteLine(" To start listing the lines in a file, use the \".lines()\" function. From now on, all output will be");
            Console.WriteLine(" the lines contained in each file rather than the file names.");
            Console.WriteLine();
            Console.WriteLine(" To include only lines containing a certain string, use the \".containing()\" function.");
            Console.WriteLine();
            Console.WriteLine(" To count the number of lines (or files), use the \".count()\" function. Note that this will cause one");
            Console.WriteLine(" single line the be the result, and that this must be the last function.");
            Console.WriteLine();
            Console.WriteLine(" To present the final output, you may use the \".present()\" function. This allows you to include the file ");
            Console.WriteLine(" name in the final output, also when presenting lines, as can be seen in the examples below.");
            Console.WriteLine();
            Console.WriteLine(" Examples:");
            Console.WriteLine("  ScanFiles 'D:\\Github\\*.cs'.recursive() - will list the names of all \".cs\" files recursively, ");
            Console.WriteLine("    starting in D:\\Github");
            Console.WriteLine("  ScanFiles 'D:\\Github\\*.cs'.recursive().containing('main') - will list the names of all files ");
            Console.WriteLine("    containing the word \"main\".");
            Console.WriteLine("  ScanFiles 'D:\\Github\\*.cs'.recursive().count() - will output the number of files (recursively,");
            Console.WriteLine("    starting in D:\\Github)");
            Console.WriteLine("  ScanFiles 'D:\\Github\\*.cs'.recursive().lines().containing('theseus').present('{file}: {line}') -");
            Console.WriteLine("    will list all lines that contain the word \"theseus\" in all the affected files, and present each");
            Console.WriteLine("    line prepended with its file name");
        }

    }
}
