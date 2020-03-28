using System;
using System.Collections.Generic;

namespace AI_Lab2
{
    public static class Solution
    {

        // Returns true if the reached state is a goal state
        public static bool IsGoalState(State currentState)
        {
            for (var i = 0; i < 3; i++)
                if (!currentState.Locations[i].StartsWith("white"))
                    return false;
            if (currentState.Locations[3] != "(empty)")
                return false;
            for (var i = 4; i < 8; i++)
                if (!currentState.Locations[i].StartsWith("black"))
                    return false;
            return true;
        }

        // Returns the empty coordinate location
        private static int EmptyCoordinate (State checkedState)
        {
            for (var i = 0; i < 8; i++)
                if (checkedState.Locations[i] == "(empty)")
                    return i;
            return -1;
        }
        
        // Returns all possible expansions from the parametre node
        public static List<State> GetPossibleMoves(State previousState)
        {
            int emptyCoordinate = EmptyCoordinate(previousState);
            var possibleMoves = new List<State>();

            if (emptyCoordinate >= 1 && emptyCoordinate <= 7 && previousState.Locations[emptyCoordinate - 1].StartsWith("black"))
                possibleMoves.Add(new State(previousState, emptyCoordinate, emptyCoordinate - 1));
            
            if (emptyCoordinate >= 2 && emptyCoordinate <= 7 && previousState.Locations[emptyCoordinate - 2].StartsWith("black"))
                possibleMoves.Add(new State(previousState, emptyCoordinate, emptyCoordinate - 2));

            if (emptyCoordinate >= 0 && emptyCoordinate <= 6 && previousState.Locations[emptyCoordinate + 1].StartsWith("white"))
                possibleMoves.Add(new State(previousState, emptyCoordinate, emptyCoordinate + 1));

            if (emptyCoordinate >= 0 && emptyCoordinate <= 5 && previousState.Locations[emptyCoordinate + 2].StartsWith("white"))
                possibleMoves.Add(new State(previousState, emptyCoordinate, emptyCoordinate + 2));

            return possibleMoves;
        }

        // An implementation of A* algorithm
        public static State AstarAlgorithm(State startPoint)
        {
            if (IsGoalState(startPoint))
                return startPoint;

            // An implementation of queue with priorities is a list
            // that will sorted every time new elements are added into it
            // (according to the summary cost)
            List<State> queueWithPriorities = new List<State>();
            var currentPoint = startPoint;
            List<State> possibleMoves;

            do
            {
                possibleMoves = GetPossibleMoves(currentPoint);
                foreach (var move in possibleMoves)
                    queueWithPriorities.Add(move);
                queueWithPriorities.Sort();
                // dequeueing the best (so far) solution
                currentPoint = queueWithPriorities[0];
                queueWithPriorities.RemoveAt(0);
                if (IsGoalState(currentPoint))
                    return currentPoint;
                possibleMoves.Clear();
            } while (queueWithPriorities.Count > 0);

            return null;
        }

    }
}