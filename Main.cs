using System;

namespace FileScan
{
    class _Main
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                DisplayHint();
                return;
            }

            var (result, strings, error) = Scanner.TryRun(args[0]);
            if (result)
            {
                foreach (var files in strings)
                {
                    foreach (var s in files)
                    {
                        Console.WriteLine(s);
                    }
                }
            }
            else
            {
                Console.WriteLine(error);
            }
        }

        static void DisplayHint()
        {
            Console.WriteLine();
            Console.WriteLine("Usage: FileScan <expression>");
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
            Console.WriteLine("   FileScan 'D:\\Github'.recursive()");
            Console.WriteLine(" but you need double quotes when calling");
            Console.WriteLine("   FileScan \"'D:\\Program Files\\Github'.recursive()\"");

            Console.WriteLine(" To include only files containing a certain string, use the \".containing()\"");
            Console.WriteLine(" function, e.g. 'D:\\Github\\*.cs'.recursive().containing('main') to list all");
            Console.WriteLine(" files containing the word \"main\".");

            //'D:\Github\*.cs'.recursive().lines().containing('theseus').present("{file}: {line}")
            // lines
            // count
            // present
            // containing


            // Examples: count the number of ".cs" files in the D:\Github directory
            // FileScan 'D:\Github\*.cs'.recursive().count()

            // Examples: count the number of ".cs" files in the D:\Github directory (recursive) whose name (including path) contains "Theseus"
            // FileScan 'D:\Github\*.cs'.recursive().containing('Theseus').count()

            // Examples: count the lines of all ".cs" files in the D:\Github directory whose name contains "Theseus", and present the information properly:
            // FileScan 'D:\Github\*.cs'.recursive().containing('Theseus').lines().count()
            
            // FileScan "'D:\Projects\*.cs'.recursive().containing('main').lines().count().present('{file} has {count} lines')"

        }
    }
}
