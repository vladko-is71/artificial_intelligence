using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            var initialState = new State();

            var finalState = Solution.BeamSearch(initialState);
            if (finalState == null)
            {
                Console.WriteLine("failure");
                return;
            }

            Console.WriteLine("The result was found, the path is following:");
            foreach (var step in finalState.path)
                Console.WriteLine(step);
        }
    }
}
