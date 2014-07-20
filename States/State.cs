
using System;
using BrewMatic3000.States.Setup;

namespace BrewMatic3000.States
{
    public delegate void DisplayContentEventHandler();

    public delegate void StateChangedEventHandler(State newState);

    public abstract class State : IDisposable
    {
        protected BrewData BrewData { get; private set; }

        private string[] _currentLcdContent;

        public event DisplayContentEventHandler DisplayContentChanged;

        public event StateChangedEventHandler StateChanged;

        protected State(BrewData brewData)
        {
            BrewData = brewData;
        }


        protected void RiseStateChangedEvent(Type newState)
        {
            StateChanged(InstanciateStateByType(newState));
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

        private State InstanciateStateByType(Type stateType)
        {
            if (stateType == typeof(State1Initial))
            {
                return new State1Initial(BrewData);
            }
            if (stateType == typeof(State2Warmup))
            {
                return new State2Warmup(BrewData);
            }
            if (stateType == typeof(StateSetupMashTemp))
            {
                return new StateSetupMashTemp(BrewData);
            }
            if (stateType == typeof(StateSetupMashTime))
            {
                return new StateSetupMashTime(BrewData);
            }
            if (stateType == typeof(StateSetupStrikeTemp))
            {
                return new StateSetupStrikeTemp(BrewData);
            }
            if (stateType == typeof(StateSetupMashTempChoose))
            {
                return new StateSetupMashTempChoose(BrewData);
            }
            if (stateType == typeof(StateSetupMashTimeChoose))
            {
                return new StateSetupMashTimeChoose(BrewData);
            }
            if (stateType == typeof(StateSetupStrikeTempChoose))
            {
                return new StateSetupStrikeTempChoose(BrewData);
            }
            if (stateType == typeof(State3Mash))
            {
                return new State3Mash(BrewData);
            }
            if (stateType == typeof(State4MashComplete))
            {
                return new State4MashComplete(BrewData);
            }
            if (stateType == typeof(StateDashboard))
            {
                return new StateDashboard(BrewData);
            }
            if (stateType == typeof(StateSetupHeat1Effect))
            {
                return new StateSetupHeat1Effect(BrewData);
            }
            if (stateType == typeof(StateSetupHeat1EffectChoose))
            {
                return new StateSetupHeat1EffectChoose(BrewData);
            }
            if (stateType == typeof(StateSetupHeat2Effect))
            {
                return new StateSetupHeat2Effect(BrewData);
            }
            if (stateType == typeof(StateSetupHeat2EffectChoose))
            {
                return new StateSetupHeat2EffectChoose(BrewData);
            }

            throw new NotImplementedException("State type \"" + stateType + "\" is not implemented");
        }

        public abstract void Start();


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





    }
}
