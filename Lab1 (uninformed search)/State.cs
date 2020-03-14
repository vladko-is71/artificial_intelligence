using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Lab1
{
    public class State
    {
        public string[,] locations;
        public List<string> path;
        public State previousState;

        public int Depth { get => path.Count; }

        public State()
        {
            locations = new string[2, 3];
            locations[0, 0] = "table";
            locations[0, 1] = "chair #1";
            locations[0, 2] = "cupboard";
            locations[1, 0] = "chair #2";
            locations[1, 1] = "(empty)";
            locations[1, 2] = "armchair";

            path = new List<string>();

            previousState = null;
        }

        public State(State previousState, int firstX, int firstY, int secondX, int secondY)
        {
            locations = new string[2, 3];
            locations[0, 0] = previousState.locations[0, 0];
            locations[0, 1] = previousState.locations[0, 1];
            locations[0, 2] = previousState.locations[0, 2];
            locations[1, 0] = previousState.locations[1, 0];
            locations[1, 1] = previousState.locations[1, 1];
            locations[1, 2] = previousState.locations[1, 2];

            string firstPosition = locations[firstX, firstY];
            string secondPosition = locations[secondX, secondY];
            string movedElement = (secondPosition == "(empty)") ? firstPosition : secondPosition;
            string logText = "The " + movedElement + " was moved";

            locations[firstX, firstY] = secondPosition;
            locations[secondX, secondY] = firstPosition;

            path = new List<string>(previousState.path);
            path.Add(logText);

            this.previousState = previousState;
        }

        // Returns true if states have identical configuration of elements
        public bool StatesAreEqual(State compared)
        {
            if (compared == null)
                return false;

            for (var i = 0; i < 2; i++)
                for (var j = 0; j < 3; j++)
                    if (locations[i, j] != compared.locations[i, j])
                        return false;
            return true;
        }
    }
}
