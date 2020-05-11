using ScanFiles;
using System.Linq;
using Xunit;

namespace Test
{
    public class FilesTests
    {
        [Fact]
        public void VerifyFirstPartMustBeQuotedString()
        {
            var builder = new TestFunctionBuilder();
            Assert.False(builder.ParseInput("", out var filesFunction, out var linesFunction));
            Assert.Contains("first part must", builder.Error);

            Assert.False(builder.ParseInput("noquotes", out filesFunction, out linesFunction));
            Assert.Contains("first part must", builder.Error);

            Assert.True(builder.ParseInput("'quotes'", out filesFunction, out linesFunction));
            Assert.Equal("", builder.Error);
        }

        [Fact]
        public void VerifyFileNames()
        {
            var builder = new TestFunctionBuilder();
            Assert.True(builder.ParseInput("'D:\'", out var filesFunction, out var linesFunction));
            var result = Runner.Run(filesFunction, linesFunction).ToList();
            Assert.Equal(3, result.Count);
            Assert.Equal("file1", result[0]);
            Assert.Equal("file2", result[1]);
            Assert.Equal("file3", result[2]);
        }

        [Fact]
        public void VerifyFileNamesRecursively()
        {
            var builder = new TestFunctionBuilder();
            Assert.True(builder.ParseInput("'D:\'.recursive()", out var filesFunction, out var linesFunction));
            var result = Runner.Run(filesFunction, linesFunction).ToList();
            Assert.Equal(9, result.Count);
            Assert.Equal("file1", result[0]);
            Assert.Equal("file2", result[1]);
            Assert.Equal("file3", result[2]);
            Assert.Equal("dir1\\file1", result[3]);
            Assert.Equal("dir1\\file2", result[4]);
            Assert.Equal("dir1\\file3", result[5]);
            Assert.Equal("dir2\\file1", result[6]);
            Assert.Equal("dir2\\file2", result[7]);
            Assert.Equal("dir2\\file3", result[8]);
        }

        [Fact]
        public void VerifyFileNamesCount()
        {
            var builder = new TestFunctionBuilder();
            Assert.True(builder.ParseInput("'D:\'.count()", out var filesFunction, out var linesFunction));
            var result = Runner.Run(filesFunction, linesFunction).ToList();
            Assert.Single(result);
            Assert.Equal("3", result[0]);
            Assert.True(builder.ParseInput("'D:\'.recursive().count()", out filesFunction, out linesFunction));
            result = Runner.Run(filesFunction, linesFunction).ToList();
            Assert.Single(result);
            Assert.Equal("9", result[0]);
        }

        [Fact]
        public void VerifyFileNamesCountEndsInput()
        {
            var builder = new TestFunctionBuilder();
            Assert.False(builder.ParseInput("'D:\'.count().something", out var filesFunction, out var linesFunction));
            Assert.Contains("count()", builder.Error);
        }

        [Fact]
        public void VerifyFileNamesContaining()
        {
            var builder = new TestFunctionBuilder();
            Assert.True(builder.ParseInput("'D:\'.containing('a')", out var filesFunction, out var linesFunction));
            Assert.Empty(filesFunction());

            Assert.True(builder.ParseInput("'D:\'.containing('')", out filesFunction, out linesFunction));
            var result = Runner.Run(filesFunction, linesFunction).ToList();
            Assert.Equal(3, result.Count);
            Assert.Equal("file1", result[0]);
            Assert.Equal("file2", result[1]);
            Assert.Equal("file3", result[2]);

            Assert.True(builder.ParseInput("'D:\'.containing('file2')", out filesFunction, out linesFunction));
            result = Runner.Run(filesFunction, linesFunction).ToList();
            Assert.Single(result);
            Assert.Equal("file2", result[0]);
        }

        [Fact]
        public void VerifyFileNamesContainingCount()
        {
            var builder = new TestFunctionBuilder();
            Assert.True(builder.ParseInput("'D:\'.containing('file3').count()", out var filesFunction, out var linesFunction));
            var result = Runner.Run(filesFunction, linesFunction).ToList();
            Assert.Single(result);
            Assert.Equal("1", result[0]);
        }

        [Fact]
        public void VerifyFileNamesContainingRecursive()
        {
            var builder = new TestFunctionBuilder();
            Assert.True(builder.ParseInput("'D:\'.recursive().containing('a')", out var filesFunction, out var linesFunction));
            Assert.Empty(filesFunction());

            Assert.True(builder.ParseInput("'D:\'.recursive().containing('file2')", out filesFunction, out linesFunction));
            var result = Runner.Run(filesFunction, linesFunction).ToList();
            Assert.Equal(3, result.Count());
            Assert.Equal("file2", result[0]);
            Assert.Equal("dir1\\file2", result[1]);
            Assert.Equal("dir2\\file2", result[2]);
        }

        [Fact]
        public void VerifyFileNamesContainingRecursiveCount()
        {
            var builder = new TestFunctionBuilder();
            Assert.True(builder.ParseInput("'D:\'.recursive().containing('a').count()", out var filesFunction, out var linesFunction));
            var result = Runner.Run(filesFunction, linesFunction).ToList();
            Assert.Single(result);
            Assert.Equal("0", result[0]);

            Assert.True(builder.ParseInput("'D:\'.recursive().containing('file2').count()", out filesFunction, out linesFunction));
            result = Runner.Run(filesFunction, linesFunction).ToList();
            Assert.Single(result);
            Assert.Equal("3", result[0]);
        }
    }
}
