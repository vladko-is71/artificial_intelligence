using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Lab3
{
    public class State : IComparable<State>
    {
        // The element is false, when the person is on the left bank (initial bank),
        // and true, if on the right bank (goal bank)
        public bool[] knights;
        public bool[] esquires;
        
        public bool boatOnGoalBank;

        public List<string> path;

        public int ObjectiveFunction
        {
            get
            {
                int objective = 0;
                for (var i = 0; i < 3; i++)
                {
                    objective += (knights[i]) ? 1 : 0;
                    objective += (esquires[i]) ? 1 : 0;
                }
                return objective;
            }
        }

        // initial state constructor
        public State()
        {
            knights = new bool[3];
            esquires = new bool[3];

            for (var i = 0; i < 3; i++)
            {
                knights[i] = false;
                esquires[i] = false; 
            }

            boatOnGoalBank = false;
            path = new List<string>();
        }

        // a constructor for case if one knight moves (either with esquire or not)
        public State(State previousState, int knightNumberChanged, bool isEsquireMoved)
        {
            knights = new bool[3];
            esquires = new bool[3];

            for (var i = 0; i < 3; i++)
            {
                if (i == knightNumberChanged)
                {
                    knights[i] = !previousState.knights[i];
                    esquires[i] = (isEsquireMoved) ? !previousState.esquires[i] : previousState.esquires[i];
                }
                else
                {
                    knights[i] = previousState.knights[i];
                    esquires[i] = previousState.esquires[i];
                }
            }

            boatOnGoalBank = !previousState.boatOnGoalBank;

            path = new List<string>(previousState.path);
            path.Add("The knight nr " + knightNumberChanged + " was moved " + (isEsquireMoved ? "with" : "without") + " an esquire");
        }

        // a constructor for case if two knights move
        public State (State previousState, int knightFirst, int knightSecond)
        {
            knights = new bool[3];
            esquires = new bool[3];

            for (var i = 0; i < 3; i++)
            {
                knights[i] = (i == knightFirst || i == knightSecond) ? !previousState.knights[i] : previousState.knights[i];
                esquires[i] = previousState.esquires[i];
            }

            boatOnGoalBank = !previousState.boatOnGoalBank;

            path = new List<string>(previousState.path);
            path.Add("The knights nr " + knightFirst + " & nr " + knightSecond + " were moved");
        }

        // Comparing two states by amount of people sent to other bank
        public int CompareTo(State other)
        {
            return -(ObjectiveFunction.CompareTo(other.ObjectiveFunction));
        }

    }
}
