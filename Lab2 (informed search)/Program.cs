using System;

namespace AI_Lab2
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var startState = new State();
            var finalState = Solution.AstarAlgorithm(startState);
            Console.WriteLine("The solution was found! The path was following:");
            for (var i = 0; i < finalState.path.Count; i++)
                Console.WriteLine(finalState.path[i]);
        }
    }
}