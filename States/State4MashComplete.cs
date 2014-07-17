namespace BrewMatic3000.States
{
    public class State4MashComplete : State
    {

        private NavigateAction[] Actions
        {
            get
            {
                return new[]
                {
                    new NavigateAction("Reset brew", "","..hold to reset", typeof(State1Initial))
                };
            }
        }

        public State4MashComplete(BrewData brewData)
            : base(brewData)
        {
        }

        public override void OnKeyPressLongWarning()
        {
            var action = GetSelectedAction(Actions);
            if (action != null)
            {
                WriteToLcd(action.Warning);
            }
            else
            {
                WriteToLcd("..hold to reset");
            }
        }

        private void WriteStandardMessage()
        {
            WriteToLcd("Mash Complete..");
        }

        public override void OnKeyPressLongCancelled()
        {
            WriteStandardMessage();
        }

        public override void OnKeyPressLong()
        {
            var action = GetSelectedAction(Actions);
            if (action != null)
            {
                RiseStateChangedEvent(action.StateType);
            }
            else
            {
                RiseStateChangedEvent(typeof(State1Initial));
            }
        }

        public override void OnKeyPressShort()
        {
            var action = ToggleActions(Actions);
            if (action != null)
            {
                WriteToLcd(action);
            }
            else
            {
                WriteStandardMessage();
            }
        }

        public override void Start()
        {
            WriteStandardMessage();
        }

    }
}
