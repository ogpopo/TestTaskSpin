﻿using AxGrid.FSM;
using Sources.SpinMachine;

namespace Assets.Sources.SpinStates
{
    [State("SpinMachineSpin")]
    public class SpinMachineSpinState : FSMState
    {
        private Accelerator _accelerator;

        public void Init(Accelerator accelerator)
        {
            _accelerator = accelerator;
        }

        [Enter]
        private void EnterThis()
        {
            Model.EventManager.AddAction("OnB_StopSpinClick", StopSpin);

            _accelerator.ResetData();
        }

        [Loop(0)]
        private void UpdateState(float deltaTime)
        {
            _accelerator.Accelerate(deltaTime);
        }

        private void StopSpin()
        {
            Parent.Change("SpinMachineStop");
        }

        // [Exit]
        // private void ExitThis()
        // {
        //     Model.EventManager.RemoveAction("OnB_StopSpinClick", StopSpin); 
        // }
    }
}