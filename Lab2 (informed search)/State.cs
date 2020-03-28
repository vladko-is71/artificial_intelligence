using System;
using System.Collections.Generic;

namespace AI_Lab2
{
    public class State : IComparable<State>
    {
        public string[] Locations;
        public State previous;
        public List<string> path;
        
        // The heuristics is "how many balls are on the wrong side to the empty space"
        public int Heuristics {
            get
            {
                int emptyCoordinate = -1;
                for (var i = 0; i < 8; i++)
                    if (Locations[i] == "(empty)")
                        emptyCoordinate = i;
                int wronglyPlaced = 0;
                for (var i = 0; i < emptyCoordinate; i++)
                    if (Locations[i].StartsWith("black"))
                        wronglyPlaced++;
                for (var i = emptyCoordinate + 1; i < 7; i++)
                    if (Locations[i].StartsWith("white"))
                        wronglyPlaced++;
                return wronglyPlaced;
            }
        }

        // Heuristics is goal proximity (backward cost), path.Count is the number of previously done steps (forward cost)
        // Their sum is the objective function which should be ->min on every step (according to A* algorithm)
        public int SummaryCost { get => path.Count + Heuristics; }
        
        public State()
        {
            Locations = new string[8];
            Locations[0] = "black #1";
            Locations[1] = "black #2";
            Locations[2] = "black #3";
            Locations[3] = "black #4";
            Locations[4] = "(empty)";
            Locations[5] = "white #1";
            Locations[6] = "white #2";
            Locations[7] = "white #3";

            previous = null;
            path = new List<string>();
        }
        
        public State(State previousState, int firstLocation, int secondLocation)
        {
            Locations = new string[8];
            for (var i = 0; i < 8; i++)
                Locations[i] = previousState.Locations[i];

            Locations[firstLocation] = previousState.Locations[secondLocation];
            Locations[secondLocation] = previousState.Locations[firstLocation];

            previous = previousState;

            string movedObject = (Locations[firstLocation] == "(empty)") ? Locations[secondLocation] : Locations[firstLocation];
            path = new List<string>(previousState.path);
            path.Add("The ball labelled as " + movedObject + " was moved.");
        }

        // Comparing two states by summary cost, so the queue with priorities could be made
        public int CompareTo(State other)
        {
            return (SummaryCost.CompareTo(other.SummaryCost));
        }
    }
}