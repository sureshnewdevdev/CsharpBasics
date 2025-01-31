using System.Reflection;

/// <summary>
/// This is sample class
/// </summary>
public class MainClass
{
    static void Main(string[] args)
    {
        int a, b;

        Console.WriteLine("Please enter an integer value for A");
        a= int.Parse(Console.ReadLine());

        Console.WriteLine("Please enter an integer value for B");
        b= int.Parse(Console.ReadLine());

        int c = Calculate(a, b);

        PrintResult(c);
        Console.ReadLine();
    }

    private static void PrintResult3()
    {
        int a, b;
        a = 10;
        b = 55;

        if (a>b)
        {
            Console.WriteLine("A is big");
        }
        else
        {
            Console.WriteLine("B is big");
        }

        for (int i = 0; i < 10; i++) 
        {
            Console.WriteLine(i);
        }

        while (true)
        {

        }

    }

    /// <summary>
    /// This program to print the result
    /// </summary>
    /// <param name="c">this input is integer</param>
    private static void PrintResult(int c)
    {
        Console.WriteLine("the result of a, b is " + c);

        Console.ReadLine();
    }

    private static int Calculate(int a, int b)
    {
        return a + b;
    }

    

}