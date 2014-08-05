
using System;

namespace BrewMatic3000.States
{
    public class NavigateAction
    {
        public string Line1 { get; private set; }

        public string Line2 { get; private set; }

        public string Warning { get; private set; }

        public State NextState { get; private set; }

        public NavigateAction(string line1, string line2, string warning, State nextState)
        {
            Line1 = line1;
            Line2 = line2;
            Warning = warning;
            NextState = nextState;
        }
    }
}
