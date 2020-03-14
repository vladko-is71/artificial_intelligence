using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Lab1
{
    public class Solution : ISolution
    {

        public Solution()
        {
        }

        public State InitialState()
        {
            return new State();
        }

        // Returns true if the reached state is a goal state
        public bool IsGoalState(State currentState)
        {
            if (currentState.locations[0, 0] == "table"
                && (currentState.locations[0, 1] == "chair #1" || currentState.locations[0, 1] == "chair #2")
                && currentState.locations[0, 2] == "armchair"
                && (currentState.locations[1, 0] == "chair #1" || currentState.locations[1, 0] == "chair #2")
                && currentState.locations[1, 1] == "(empty)"
                && currentState.locations[1, 2] == "cupboard")
            {
                return true;
            }
            return false;
        }

        // Handles the swapping of two elements
        public State Result(State previousState, int firstX, int firstY, int secondX, int secondY)
        {
            if ((previousState.locations[firstX, firstY] == "(empty)" ^ previousState.locations[secondX, secondY] == "(empty)")
                && (firstX == firstY ^ secondX == secondY))
            {
                State newState = new State(previousState, firstX, firstY, secondX, secondY);
                return newState;
            }
            return null;
        }

        // Returns true if there's cycle in the path
        private bool IsCycle(State node)
        {
            State current = node.previousState;
            while (true)
            {
                if (current == null)
                    return false;
                else if (current.StatesAreEqual(node))
                    return true;

                current = current.previousState;
            }
        }

        // Returns all the possible moves from the parametre node
        public List<State> GetPossibleMoves(State previousState)
        {
            int emptyX = 0;
            int emptyY = 0;

            for (var i = 0; i < 2; i++)
                for (var j = 0; j < 3; j++)
                    if (previousState.locations[i, j] == "(empty)")
                    {
                        emptyX = i;
                        emptyY = j;
                    }

            List<State> possibleStates = new List<State>();
            
            possibleStates.Add((emptyX != 0) ? new State(previousState, emptyX, emptyY, emptyX - 1, emptyY) : null);
            possibleStates.Add((emptyX != 1) ? new State(previousState, emptyX, emptyY, emptyX + 1, emptyY) : null);
            possibleStates.Add((emptyY != 0) ? new State(previousState, emptyX, emptyY, emptyX, emptyY - 1) : null);
            possibleStates.Add((emptyY != 2) ? new State(previousState, emptyX, emptyY, emptyX, emptyY + 1) : null);

            possibleStates.RemoveAll(state => state == null);
            return possibleStates;

        }

        // Iterative deepening
        // Contains the general idea of algorithm
        // Returns the final state
        public State IterativeDeepening()
        {
            int depth = 0;
            List<State> statesToReview = new List<State>();
            statesToReview.Add(new State());
            while (true)
            {
                Console.WriteLine("Reviewing layer {0}", depth);
                var result = DepthLimitedSearch(depth, statesToReview);
                statesToReview.Clear();
                statesToReview = result.Item3;
                if (result.Item1 != "cutoff")
                {
                    if (result.Item2 == null)
                        Console.WriteLine("A node can't be found :(");
                    else
                        Console.WriteLine("The result was found!");

                    return result.Item2;
                }

                depth++;
            }
        }

        // Searching on one level
        // Returns: result state, resulting node (if exists), the list of states to expand on next level
        private Tuple<string, State, List<State>> DepthLimitedSearch(int depthL, List<State> statesToReview)
        {
            var frontier = new Queue<State>();
            foreach (var state in statesToReview)
                frontier.Enqueue(state);
            string resultState = "failure";
            State result = null;
            List<State> possibleStates = new List<State>();
            while (frontier.Count != 0)
            {
                State node = frontier.Dequeue();
                if (IsGoalState(node))
                {
                    resultState = "success";
                    result = node;
                    return new Tuple<string, State, List<State>>(resultState, result, possibleStates);
                }
                else if (node.Depth > depthL)
                {
                    resultState = "cutoff";
                    result = null;
                    
                }
                else if (!IsCycle(node) || depthL == 0)
                {
                    var expanded = GetPossibleMoves(node);
                    
                    foreach (var child in expanded)
                    {
                        frontier.Enqueue(child);
                        possibleStates.Add(child);
                    }
                }

            }
            return new Tuple<string, State, List<State>>(resultState, result, possibleStates);
        }
    }
}
