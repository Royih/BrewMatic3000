using System;
using System.Threading;

namespace BrewMatic3000.States
{
    public delegate void DisplayContentEventHandler();

    public delegate void StateChangedEventHandler(State newState);

    public abstract class State : IDisposable
    {
        protected Thread Worker;

        protected bool Abort;

        private int _selectedScreen = 0;

        protected BrewData BrewData { get; private set; }

        protected Screen CurrentScreen;

        protected Screen GetScreenError(int screenNumber)
        {
            return new Screen(screenNumber, new[] { "", "Screen [" + screenNumber + "]: Error." });
        }

        protected int GetCurrentScreenNumber
        {
            get
            {
                if (CurrentScreen == null)
                    return -1;
                return CurrentScreen.Identifier;
            }
        }

        public event DisplayContentEventHandler DisplayContentChanged;

        public event StateChangedEventHandler StateChanged;

        public string[] _initialMessage = null;

        protected State(BrewData brewData, string[] initialMessage = null, int initialScreen = 0)
        {
            BrewData = brewData;
            _initialMessage = initialMessage;
            _selectedScreen = initialScreen;
            Worker = new Thread(
            DoWork
            ) { Priority = ThreadPriority.Normal };
            Worker.Start();

        }

        protected void RiseStateChangedEvent(State nextState)
        {
            StateChanged(nextState);
        }

        public string[] CurrentLcdContent
        {
            get
            {
                if (CurrentScreen == null)
                    return new[] { "", "", "", "" };
                return CurrentScreen.GetScreenContent;
            }
        }

        protected virtual void StartExtra()
        {

        }

        private void DoWork()
        {
            CurrentScreen = GetScreen(_selectedScreen);
            if (_initialMessage != null)
                CurrentScreen.SetInitialMessage(_initialMessage);
            StartExtra();
            while (!Abort)
            {
                DoWorkExtra();
                CurrentScreen.UpdateScreenContent(GetScreen(_selectedScreen));
                if (DisplayContentChanged != null)
                {
                    DisplayContentChanged();
                }
                Thread.Sleep(500);
            }
        }

        protected virtual void DoWorkExtra()
        {

        }



        public void Dispose()
        {
            Abort = true;
        }

        #region Handle Screens

        public abstract Screen GetScreen(int screenNumber);

        public abstract int GetNumberOfScreens();

        public void SetScreen(int screenNumber)
        {
            _selectedScreen = screenNumber;
        }

        #endregion

        #region Handle KeyPresses

        public virtual void KeyPressPreviousShort()
        {
            if (--_selectedScreen < 0)
            {
                _selectedScreen = GetNumberOfScreens();
            }
        }

        public virtual void KeyPressNextShort()
        {
            if (++_selectedScreen > GetNumberOfScreens())
            {
                _selectedScreen = 0;
            }
        }

        public virtual void KeyPressPreviousLongWarning()
        {
            CurrentScreen.SetScreenState(Screen.ScreenState.WarningPrevious);
        }

        public virtual void KeyPressNextLongWarning()
        {
            CurrentScreen.SetScreenState(Screen.ScreenState.WarningNext);
        }

        public virtual void KeyPressPreviousLongWarningCancelled()
        {
            CurrentScreen.SetScreenState(Screen.ScreenState.Default);
        }

        public virtual void KeyPressNextLongWarningCancelled()
        {
            CurrentScreen.SetScreenState(Screen.ScreenState.Default);
        }

        public void KeyPressPreviousPrivate()
        {
            KeyPressPreviousLong();
            CurrentScreen.SetScreenState(Screen.ScreenState.Default);
            RiseStateChangedEvent(new StateDashboard(BrewData));
        }

        public virtual void KeyPressPreviousLong()
        {

        }

        public void KeyPressNextLongPrivate()
        {
            KeyPressNextLong();
            CurrentScreen.SetScreenState(Screen.ScreenState.Default);
        }

        public virtual void KeyPressNextLong()
        {

        }

        #endregion



    }
}
