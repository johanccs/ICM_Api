using AECI.ICM.Application.Interfaces;
using System.Diagnostics;
using System.Threading;

namespace AECI.ICM.Application.Services
{
    public class ExceptionMonitor : IExceptionMonitor
    {
        #region Fields

        bool StartToggle = true;

        Thread myThread;

        #endregion

        #region Constructor



        #endregion

        #region Public Methods

        public void Start()
        {
            while (StartToggle)
            {
                myThread = new Thread(new ThreadStart(StartThread));
                myThread.Start();
            }
        }

        #endregion

        #region Private Methods

        private void StartThread()
        {
            Debug.Print("Tick");

            Thread.Sleep(20000);
        }

        #endregion
    }
}
