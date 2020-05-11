using ScanFiles;
using System.Collections.Generic;

namespace Test
{
    class TestFunctionBuilder : FunctionBuilder
    {
        protected override IEnumerable<string> EnumerateFiles(string baseDir, bool recursive)
        {
            yield return "file1";
            yield return "file2";
            yield return "file3";
            if (recursive)
            {
                yield return "dir1\\file1";
                yield return "dir1\\file2";
                yield return "dir1\\file3";
                yield return "dir2\\file1";
                yield return "dir2\\file2";
                yield return "dir2\\file3";
            }
        }

        protected override IEnumerable<string> EnumerateLines(string fileName)
        {
            for (int i = 0; i < 3; i++)
            {
                yield return $"{fileName.Replace("\\", "_")}_line{i+1}";
            }
        }
    }
}
