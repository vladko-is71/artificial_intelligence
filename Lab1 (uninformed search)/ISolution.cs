using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Lab1
{
    public interface ISolution
    {
        State InitialState();
        bool IsGoalState(State state);
        State Result(State previousState, int firstX, int firstY, int secondX, int secondY);

    }
}
