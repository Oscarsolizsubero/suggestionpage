using System;

namespace WishGrid.ViewModels.Shared
{
    public enum State
    {
        None
        , Successful
        , Warning
        , Error
    }

    public class VMMessage
    {

        private State _state;
        private object _data;

        public State State
        {
            get
            {
                return _state;
            }
        }

        public string Text
        {
            get
            {
                return _data.ToString();
            }
        }

        public string Data
        {
            get
            {
                return _data.ToString();
            }
            set
            {
                _data = value;
            }
        }

        public bool IsSuccessful()
        {
            return _state == State.Successful;
        }

        public VMMessage(State state, string text)
        {
            _state = state;
            _data = text;
        }

        public VMMessage(State state, Exception exception) : this(state, exception.ToString()) { }

        public VMMessage(State state) : this(state, (string)null) { }
    }
}
