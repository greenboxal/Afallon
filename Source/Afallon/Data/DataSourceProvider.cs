using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace System.Windows.Data
{
    public abstract class DataSourceProvider : INotifyPropertyChanged, ISupportInitialize
    {
        private int _deferLevel;
        private bool _initialLoadCalled;

        public object Data { get; private set; }
        public Exception Error { get; private set; }
        public bool IsInitialLoadEnabled { get; set; }

        protected Dispatcher Dispatcher { get; set; }

        protected bool IsRefreshDeferred
        {
            get
            {
                if (_deferLevel > 0)
                    return true;

                if (!IsInitialLoadEnabled)
                    return !_initialLoadCalled;

                return false;
            }
        }

        public event EventHandler DataChanged;

        protected event PropertyChangedEventHandler PropertyChanged;

        protected virtual void BeginInit()
        {
            _deferLevel++;
        }

        protected virtual void EndInit()
        {
            EndDefer();
        }

        protected virtual void BeginQuery()
        {
            
        }

        public IDisposable DeferRefresh()
        {
            _deferLevel++;
            return new DeferOperation(this);
        }

        public void InitialLoad()
        {
            if (!IsInitialLoadEnabled || _initialLoadCalled)
                return;

            _initialLoadCalled = true;
            BeginQuery();
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        protected void OnQueryFinished(object newData)
        {
            OnQueryFinished(newData, null, null, null);
        }

        protected virtual void OnQueryFinished(object newData, Exception error, DispatcherOperationCallback completionWork,
                                       object callbackArguments)
        {
            if (Dispatcher.CheckAccess())
            {
                UpdateResult(error, newData, completionWork, callbackArguments);
            }
            else
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                       new Action(() => UpdateResult(
                                           error,
                                           newData,
                                           completionWork,
                                           callbackArguments
                                                            )));
            }
        }

        public void Refresh()
        {
            _initialLoadCalled = true;
            BeginQuery();
        }

        private void UpdateResult(Exception error, object newData, DispatcherOperationCallback completionWork, object callbackArguments)
        {
            bool errorChanged = Error != error;

            Error = error;

            if (error != null)
            {
                newData = null;
                _initialLoadCalled = false;
            }

            Data = newData;

            if (completionWork != null)
                completionWork(callbackArguments);

            if (DataChanged != null)
                DataChanged(this, EventArgs.Empty);

            if (errorChanged)
                OnPropertyChanged(new PropertyChangedEventArgs("Error"));
        }

        private void EndDefer()
        {
            _deferLevel--;

            if (_deferLevel == 0)
                Refresh();
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { PropertyChanged += value; }
            remove { PropertyChanged -= value; }
        }

        void ISupportInitialize.BeginInit()
        {
            BeginInit();
        }

        void ISupportInitialize.EndInit()
        {
            EndInit();
        }

        private class DeferOperation : IDisposable
        {
            private DataSourceProvider _owner;

            public DeferOperation(DataSourceProvider owner)
            {
                _owner = owner;
            }

            public void Dispose()
            {
                if (_owner == null)
                    return;

                _owner.EndDefer();
                _owner = null;
            }
        }
    }
}
