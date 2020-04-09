using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Lab3
{
    public static class Solution
    {
        // Returns true if current state is a goal state
        public static bool IsGoalState(State currentState)
        {
            for (var i = 0; i < 3; i++)
            {
                if (!currentState.knights[i])
                    return false;
                if (!currentState.esquires[i])
                    return false;
            }
            return true;
        }

        // Checks if either of states in the array is a goal state
        // (Usually, the input is the array of best possible solutions on some iteration)
        public static State CheckingSolutions(List<State> probableSolutions)
        {
            foreach (var solution in probableSolutions)
                if (IsGoalState(solution))
                    return solution;

            return null;
        }

        // Getting possible moves from one state
        public static List<State> GetPossibleMoves(State currentState)
        {
            var possibleMoves = new List<State>();
            State move;

            for (var i = 0; i < 3; i++)
            {
                if (currentState.knights[i] == currentState.boatOnGoalBank)
                {
                    move = new State(currentState, i, false);
                    possibleMoves.Add(move);

                    if (currentState.esquires[i] == currentState.boatOnGoalBank)
                    {
                        move = new State(currentState, i, true);
                        possibleMoves.Add(move);
                    }

                    for (var j = i + 1; j < 3; j++)
                        if (currentState.knights[j] == currentState.boatOnGoalBank)
                        {
                            move = new State(currentState, i, j);
                            possibleMoves.Add(move);
                        }
                }
            }

            return possibleMoves;
        }

        // Getting possible moves from the array of states
        public static List<State> GetPossibleMoves(List<State> currentStates)
        {
            var possibleMoves = new List<State>();
            foreach (var state in currentStates)
            {
                List<State> partOfPossibleMoves = GetPossibleMoves(state);
                foreach (var move in partOfPossibleMoves)
                    possibleMoves.Add(move);
            }
            return possibleMoves;
        }

        // Out of the array of possible moves, this function returns K best
        public static List<State> GetPriorityMoves(List<State> possibleMoves, int k)
        {
            if (possibleMoves.Count <= k)
                return possibleMoves;

            possibleMoves.Sort();
            var goodMoves = new List<State>();
            for (var i = 0; i < k; i++)
                goodMoves.Add(possibleMoves[i]);
            return goodMoves;
        }

        // Beam search: the parametre is the initial state,
        // the function returns the final state
        public static State BeamSearch(State startPoint)
        {
            if (IsGoalState(startPoint))
                return startPoint;

            var currentlyBestSolutions = new List<State>() { startPoint };

            while (true)
            {
                var possibleMoves = GetPossibleMoves(currentlyBestSolutions);
                currentlyBestSolutions = GetPriorityMoves(possibleMoves, 4);

                var currentResult = CheckingSolutions(currentlyBestSolutions);
                if (currentResult != null)
                    return currentResult;
            }
        }
    }
}
