using AxGrid.FSM;
using Sources.SpinMachine;

namespace Assets.Sources.SpinStates
{
    [State("SpinMachineSpin")]
    public class SpinMachineSpinState : FSMState
    {
        private Accelerator _accelerator;
        private SpinMachine _spinMachine;

        private float _timer = 0;
        private bool _stopButtonIsActive;

        public void Init(Accelerator accelerator, SpinMachine spinMachine)
        {
            _accelerator = accelerator;
            _spinMachine = spinMachine;
        }

        [Enter]
        private void EnterThis()
        {
            Model.EventManager.AddAction("OnB_StopSpinClick", StopSpin);
            _timer = 0;
            _stopButtonIsActive = false;
            _accelerator.ResetData();
        }

        [Loop(0)]
        private void UpdateState(float deltaTime)
        {
            _accelerator.Accelerate(deltaTime);
            _timer += deltaTime;

            if (_timer >= 3 && _stopButtonIsActive == false)
            {
                _spinMachine.SetActiveStopElements(true);
                _stopButtonIsActive = true;
            }
        }

        private void StopSpin()
        {
            Parent.Change("SpinMachineStop");
        }

        [Exit]
        private void ExitThis()
        {
            //Model.EventManager.RemoveAction("OnB_StopSpinClick", StopSpin);
            _spinMachine.SetActiveStopElements(false);
        }
    }
}