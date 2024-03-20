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
        }

        private void StartSpin()
        {
            Parent.Change("SpinMachineSpin");
        }
        //
        // [Exit]
        // private void Exit()
        // {
        //     Model.EventManager.RemoveAction("OnB_StartSpinClick", StartSpin);
        // }
    }
}