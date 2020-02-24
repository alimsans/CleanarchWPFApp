using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WpfApp1.Controllers
{
    internal class CrossController
    {
        private int _operationsPending;

        public bool IsBusy { get; private set; }
        public int OperationsPending
        {
            get => _operationsPending;
            private set
            {
                if (value == 0)
                    OnFreed();
                else if (value == 1 && _operationsPending == 0)
                    OnControlBlocked();
                    
                _operationsPending = value;
            }
        }

        public event EventHandler ControlFreed;
        public event EventHandler ControlBlocked;


        public CrossController()
        {
            _operationsPending = 0;

            IsBusy = false;
        }

        public void RegisterOperation(ref EventHandler operationFinished)
        {
            IsBusy = true;
            OperationsPending++;

            operationFinished += OperationFinished;
        }

        private void OperationFinished(object sender, EventArgs e)
        {
            OperationsPending--;
        }

        private void OnFreed()
        {
            ControlFreed?.Invoke(this, EventArgs.Empty);
        }

        private void OnControlBlocked()
        {
            ControlBlocked?.Invoke(this, EventArgs.Empty);
        }
    }
}
