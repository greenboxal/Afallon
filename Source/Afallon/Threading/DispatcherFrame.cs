using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Threading
{
    public class DispatcherFrame : DispatcherObject
    {
        private readonly bool _exitWhenRequested;
        private bool _continue;

        public bool Continue
        {
            get
            {
                bool shouldContinue = _continue;

                if (shouldContinue && _exitWhenRequested)
                {
                    if (Dispatcher.IsShutdownStarted || Dispatcher.ShouldExitAllFrames)
                        shouldContinue = false;
                }

                return shouldContinue;
            }
            set
            {
                _continue = value;
                // TODO: Force continuation check?
            }
        }

        public DispatcherFrame()
            : this(true)
        {
            
        }

        public DispatcherFrame(bool exitWhenRequested)
        {
            _exitWhenRequested = exitWhenRequested;
            _continue = true;
        }
    }
}
