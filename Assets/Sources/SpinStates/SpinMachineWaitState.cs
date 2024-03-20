using AxGrid.FSM;

namespace Assets.Sources.SpinStates
{
    [State("SpinMachineWait")]
    public class SpinMachineWaitState : FSMState
    {
        private SpinMachine _spinMachine;
        
        [Enter]
        private void EnterThis()
        {
            Model.EventManager.AddAction("OnB_StartSpinClick", StartSpin);
        }

        private void StartSpin()
        {
            Parent.Change("SpinMachineSpin");
        }
        
        public void Init(SpinMachine spinMachine)
        {
            _spinMachine = spinMachine;
        }
        
        [Exit]
        private void Exit()
        {
            //Model.EventManager.RemoveAction("OnB_StartSpinClick", StartSpin);
            _spinMachine.SetActiveSpinElements(false);
        }
    }
}