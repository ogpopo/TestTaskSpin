using Assets.Sources.SpinStates;
using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;

public class SpinMachine : MonoBehaviourExtBind
{
    [OnAwake]
    private void AwakeThis()
    {
        
    }

    [OnStart]
    private void StartThis()
    {
        Settings.Fsm = new();
        Settings.Fsm.Add(new SpinMachineWaitState());
        Settings.Fsm.Add(new SpinMachineSpinState());
        
        Settings.Fsm.Start("SpinMachineWait");
    }
}