using System;

namespace BrewMatic3000.States
{
    public class Screen
    {
        public enum ScreenState
        {
            Default,
            WarningPrevious,
            WarningNext,
            InitialMessage
        }

        private ScreenState _screenState = ScreenState.Default;

        public int Identifier { get; private set; }

        private string[] _defaultContent;

        private string _longWarningPrevious;

        private string _longWarningNext;

        private string[] _initialMessage;

        private DateTime _initialMessageTimeout;

        private const int InitialMessageMilliSeconds = 1000;

        public string[] GetScreenContent
        {
            get
            {
                switch (_screenState)
                {
                    case ScreenState.Default:
                        return _defaultContent;
                    case ScreenState.WarningPrevious:
                        return new[] { _defaultContent[0], _longWarningPrevious + "?", "", "..hold to continue" };
                    case ScreenState.WarningNext:
                        return new[] { _defaultContent[0], _longWarningNext + "?", "", "..hold to continue" };
                    case ScreenState.InitialMessage:
                        {
                            if (DateTime.Now > _initialMessageTimeout)
                            {
                                _screenState = ScreenState.Default;
                                _initialMessageTimeout = DateTime.MinValue;
                                return _defaultContent;
                            }
                            return _initialMessage;
                        }
                }
                return new[] { "", "Error", "", "" };
            }
        }

        public void SetScreenState(ScreenState state)
        {
            if (state == ScreenState.WarningNext && _longWarningNext == null)
                return;
            if (state == ScreenState.WarningPrevious && _longWarningPrevious == null)
                return;
            _screenState = state;

        }

        public void UpdateScreenContent(Screen screen)
        {
            Identifier = screen.Identifier;
            _defaultContent = screen._defaultContent;
            _longWarningPrevious = screen._longWarningPrevious;
            _longWarningNext = screen._longWarningNext;
        }

        public void SetInitialMessage(string[] initialMessage)
        {
            _initialMessage = initialMessage;
            _initialMessageTimeout = DateTime.Now.AddMilliseconds(InitialMessageMilliSeconds);
            _screenState = ScreenState.InitialMessage;
        }


        public Screen(int identifier, string[] lines, string longWarningNext = null, string longWarningPrevious = null)
        {
            Identifier = identifier;
            _defaultContent = lines;
            _longWarningPrevious = longWarningPrevious;
            _longWarningNext = longWarningNext;
        }
    }
}
