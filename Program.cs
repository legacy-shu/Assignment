using System;
using System.Linq;
namespace Assignment
{
    class Program
    {
        static void Main(string[] args)
        {
            Handler handler = new Handler();
            Assignment assignment = new Assignment(handler);

            if (args.Length >= 1)
            {
                assignment.Start(args.Select(s => s.ToLower()).ToList());
            }

            while (true)
            {
                Console.WriteLine("Pease enter any command:");
                string input = Console.ReadLine();
                assignment.Start(input.Split().Select(s => s.ToLower()).ToList());
            }

        }

    }
}

