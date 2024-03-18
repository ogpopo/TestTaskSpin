using AxGrid.FSM;

namespace Assets.Sources.SpinStates
{
    [State("SpinMachineSpin")]
    public class SpinMachineSpinState : FSMState
    {
        [Enter]
        private void EnterThis()
        {
            _log.Debug("Spin Start");
        }
    }
}