using AxGrid.FSM;
using Sources.SpinMachine;

namespace Assets.Sources.SpinStates
{
    [State("SpinMachineStop")]
    public class SpinMachineStopState : FSMState
    {
        private Accelerator _accelerator;

        public void Init(Accelerator accelerator)
        {
            _accelerator = accelerator;
        }
        
        [Enter]
        private void EnterThis()
        {
            _accelerator.ResetData();

            _accelerator.Stopped += OnSpinStop;
        }

        [Loop(0)]
        private void UpdateState(float deltaTime)
        {
            _accelerator.StopAccelerate(deltaTime);
        }

        private void OnSpinStop()
        {
            Parent.Change("SpinMachineWait");
        }
    }
}