namespace SampleClass;

public class HelloClass
{
    public string Hello(bool maybe)
    {
        if (maybe) {
            var x = 1; 
            return "Hello, World";
        }
        else
            return "Goodbye, World!";
    }

    public void Untested(string input)
    {
        input = "goof";
    }
}