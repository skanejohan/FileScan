using ScanFiles;
using System.Linq;
using Xunit;

namespace Test
{
    public class LinesTests
    {
        [Fact]
        public void VerifySyntaxError()
        {
            var builder = new TestFunctionBuilder();
            Assert.False(builder.ParseInput("'D:\'.apa()", out var filesFunction, out var linesFunction));
            Assert.Contains("apa()", builder.Error);
        }

        [Fact]
        public void VerifyAllLinesInOneFile()
        {
            var builder = new TestFunctionBuilder();
            Assert.True(builder.ParseInput("'C:'.containing('file2').lines()", out var filesFunction, out var linesFunction));
            var result = Runner.Run(filesFunction, linesFunction).ToList();
            Assert.Equal(3, result.Count);
            Assert.Equal("file2_line1", result[0]);
            Assert.Equal("file2_line2", result[1]);
            Assert.Equal("file2_line3", result[2]);
        }

        [Fact]
        public void VerifyAllLinesInCertainFiles()
        {
            var builder = new TestFunctionBuilder();
            Assert.True(builder.ParseInput("'C:'.recursive().containing('file2').lines()", out var filesFunction, out var linesFunction));
            var result = Runner.Run(filesFunction, linesFunction).ToList();
            Assert.Equal(9, result.Count);
            Assert.Equal("file2_line1", result[0]);
            Assert.Equal("file2_line2", result[1]);
            Assert.Equal("file2_line3", result[2]);
            Assert.Equal("dir1_file2_line1", result[3]);
            Assert.Equal("dir1_file2_line2", result[4]);
            Assert.Equal("dir1_file2_line3", result[5]);
            Assert.Equal("dir2_file2_line1", result[6]);
            Assert.Equal("dir2_file2_line2", result[7]);
            Assert.Equal("dir2_file2_line3", result[8]);
        }

        [Fact]
        public void VerifyLinesCount()
        {
            var builder = new TestFunctionBuilder();
            Assert.True(builder.ParseInput("'C:'.containing('file2').lines().count()", out var filesFunction, out var linesFunction));
            var result = Runner.Run(filesFunction, linesFunction).ToList();
            Assert.Single(result);
            Assert.Equal("3", result[0]);
        }

        [Fact]
        public void VerifyLinesContaining()
        {
            var builder = new TestFunctionBuilder();
            Assert.True(builder.ParseInput("'C:'.recursive().containing('file2').lines().containing('line3')", out var filesFunction, out var linesFunction));
            var result = Runner.Run(filesFunction, linesFunction).ToList();
            Assert.Equal(3, result.Count);
            Assert.Equal("file2_line3", result[0]);
            Assert.Equal("dir1_file2_line3", result[1]);
            Assert.Equal("dir2_file2_line3", result[2]);
        }
    }
}
