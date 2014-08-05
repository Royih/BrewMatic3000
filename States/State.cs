
using System;
using System.Threading;
using BrewMatic3000.States.Setup;

namespace BrewMatic3000.States
{
    public delegate void DisplayContentEventHandler();

    public delegate void StateChangedEventHandler(State newState);

    public abstract class State : IDisposable
    {
        protected int SecondsLeftOfNewStateIndication = 4;

        protected BrewData BrewData { get; private set; }

        private string[] _currentLcdContent;

        public event DisplayContentEventHandler DisplayContentChanged;

        public event StateChangedEventHandler StateChanged;

        protected State(BrewData brewData)
        {
            BrewData = brewData;
        }

        protected void RiseStateChangedEvent(State nextState)
        {
            StateChanged(nextState);
        }

        public string[] CurrentLcdContent
        {
            get
            {
                if (_currentLcdContent == null)
                    _currentLcdContent = new[] { "", "" };
                return _currentLcdContent;
            }
        }

        protected void WriteToLcd(NavigateAction action)
        {
            WriteToLcd(action.Line1, action.Line2);
        }
        protected void WriteToLcd(string line1, string line2 = "")
        {
            _currentLcdContent = new[] { line1, line2 };
            DisplayContentChanged();
        }

        private int _selectedActionPointer = -1;

        public NavigateAction ToggleActions(NavigateAction[] actions)
        {
            if (++_selectedActionPointer < actions.Length)
            {
                return actions[_selectedActionPointer];
            }
            _selectedActionPointer = -1;
            return null;
        }

        public NavigateAction GetSelectedAction(NavigateAction[] actions)
        {
            return _selectedActionPointer > -1 ? actions[_selectedActionPointer] : null;
        }

        public abstract void Start();

        public abstract string[] GetNewStateIndication(int secondsLeft);

        public virtual void Dispose()
        {

        }

        public virtual void OnKeyPressShort()
        {

        }

        public virtual void OnKeyPressLong()
        {

        }

        public virtual void OnKeyPressLongWarning()
        {

        }

        public virtual void OnKeyPressLongCancelled()
        {

        }

        public void ShowStateName()
        {
            while (SecondsLeftOfNewStateIndication-- > 1)
            {
                var indication = GetNewStateIndication(SecondsLeftOfNewStateIndication);
                if (indication == null)
                {
                    return;
                }
                WriteToLcd(indication[0], indication[1]);
                Thread.Sleep(1000);
            }
        }




    }
}
