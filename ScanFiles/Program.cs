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
            Console.WriteLine(" An expression always starts with the name of a directory, including a possible");
            Console.WriteLine(" search filter. Examples are: 'C:\', 'D:\\Github' and 'D:\\Github\\*.cs'. Note");
            Console.WriteLine(" that, like all strings, the directory is enclosed with single quotes. These");
            Console.WriteLine(" examples will list the files found in the specified directories.");
            Console.WriteLine();
            Console.WriteLine(" To specify that the search should be recursive, use the \".recursive()\"");
            Console.WriteLine(" specification. An example is: 'D:\\Github\\*.cs'.recursive().");
            Console.WriteLine();
            Console.WriteLine(" Please note that if any part of your expression contains spaces, you must");
            Console.WriteLine(" enclose the whole expression in double quotes. That is, you can call");
            Console.WriteLine("   ScanFiles 'D:\\Github'.recursive()");
            Console.WriteLine(" but you need double quotes when calling");
            Console.WriteLine("   ScanFiles \"'D:\\Program Files\\Github'.recursive()\"");

            Console.WriteLine(" To include only files containing a certain string, use the \".containing()\"");
            Console.WriteLine(" function, e.g. 'D:\\Github\\*.cs'.recursive().containing('main') to list all");
            Console.WriteLine(" files containing the word \"main\".");

            //'D:\Github\*.cs'.recursive().lines().containing('theseus').present("{file}: {line}")
            // lines
            // count
            // present
            // containing


            // Examples: count the number of ".cs" files in the D:\Github directory
            // ScanFiles 'D:\Github\*.cs'.recursive().count()

            // Examples: count the number of ".cs" files in the D:\Github directory (recursive) whose name (including path) contains "Theseus"
            // ScanFiles 'D:\Github\*.cs'.recursive().containing('Theseus').count()

            // Examples: count the lines of all ".cs" files in the D:\Github directory whose name contains "Theseus", and present the information properly:
            // ScanFiles 'D:\Github\*.cs'.recursive().containing('Theseus').lines().count()

            // ScanFiles "'D:\Projects\*.cs'.recursive().containing('main').lines().count().present('{file} has {count} lines')"

        }

    }
}
