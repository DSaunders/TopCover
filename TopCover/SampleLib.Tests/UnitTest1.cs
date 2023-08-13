using SampleClass;

namespace SampleLib.Tests;

public class UnitTest1
{
    [Fact]
    public void HelloTest()
    {
        Assert.Equal("Hello, World", new HelloClass().Hello(true));
    }
    
    [Fact]
    public void HelloTest2()
    {
        new HelloClass().Untested("foo");
    }
}