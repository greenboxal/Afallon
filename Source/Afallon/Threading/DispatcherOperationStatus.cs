using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Threading
{
    public enum DispatcherOperationStatus
    {
        Pending,
        Aborted,
        Completed,
        Executing,
    }
}
