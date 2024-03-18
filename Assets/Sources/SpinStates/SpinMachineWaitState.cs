using System.Diagnostics;
using AxGrid.FSM;

namespace Assets.Sources.SpinStates
{
    [State("SpinMachineWait")]
    public class SpinMachineWaitState : FSMState
    {
        [Enter]
        private void EnterThis()
        {
            Model.EventManager.AddAction("OnB_StartSpinClick", StartSpin);
            Model.EventManager.AddAction("OnB_StopSpinClick", StopSpin);
        }

        private void StartSpin()
        {_log.Debug("111");
            Parent.Change("SpinMachineSpin");
        }

        private void StopSpin()
        {
        }
    }
}