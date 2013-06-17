using System;

namespace System.Windows.Input
{
    public class TraversalRequest
    {
        private readonly FocusNavigationDirection _focusNavigationDirection;

        public FocusNavigationDirection FocusNavigationDirection
        {
            get { return _focusNavigationDirection; }
        }

        public bool Wrapped { get; set; }

        public TraversalRequest(FocusNavigationDirection focusNavigationDirection)
        {
            _focusNavigationDirection = focusNavigationDirection;
        }
    }
}
