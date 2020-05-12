# FileScan

This project implements the ScanFiles command-line application, which can be used to examine text files on your system, describing which operations to make.

__Usage__ 

    ScanFiles [expression]

An expression always starts with the name of a directory, including a possible search filter. Examples are: 'C:\', 'D:\Github' and 'D:\Github\\\*.cs'. Note that, like all strings, the directory is enclosed with single quotes. These examples will list the files found in the specified directories.

To specify that the search should be recursive, use the __.recursive()__ specification.

Please note that if any part of your expression contains spaces, you must enclose the whole expression in double quotes. That is, you can call

    ScanFiles 'D:\Github'.recursive()
   
but you need double quotes when calling

    ScanFiles "'D:\Program Files\Github'.recursive()"
   
To include only files containing a certain string, use the __.containing()__ function.

To start listing the lines in a file, use the __.lines()__ function. From now on, all output will be the lines contained in each file rather than the file names.

To include only lines containing a certain string, use the __.containing()__ function.

To count the number of lines (or files), use the __.count()__ function. Note that this will cause one single line the be the result, and that this must be the last function.

To present the final output, you may use the __.present()__ function. This allows you to include the file name in the final output, also when presenting lines, as can be seen in the examples below.

__Examples__

To list the names of all \".cs\" files recursively, starting in D:\Github:

    ScanFiles 'D:\Github\*.cs'.recursive() 
    
To list the names of all \".cs\" files recursively, whose name contains "main", starting in D:\Github:

    ScanFiles 'D:\Github\*.cs'.recursive().containing('main')

To output the number of \".cs\" files, starting in D:\Github (recursively):

    ScanFiles 'D:\Github\*.cs'.recursive().count()

To list all lines that contain the word "theseus" in all the affected files, and present each line prepended with its file name (note the double quotes around the whole expression, needed because of the white space in the argument to present):

    ScanFiles "'D:\Github\*.cs'.recursive().lines().containing('theseus').present('{file}: {line}')"
