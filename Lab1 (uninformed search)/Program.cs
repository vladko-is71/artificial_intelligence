using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initial phase with initial state being created
            var solution = new Solution();
            // var initialState = new State();

            var finalState = solution.IterativeDeepening();
            Console.WriteLine("The path was following:");
            foreach (var step in finalState.path)
                Console.WriteLine(step);

        }
    }
}
