using Sources.Enums;
using UnityEngine;

namespace Sources.SpinMachine
{
    public class Slot : MonoBehaviour
    {
        [SerializeField] private SlotType _slotType;
        [SerializeField] private Animator _animator;
        
        private static readonly int KnockOut = Animator.StringToHash("KnockOut");

        public SlotType SlotType => _slotType;
        
        public void Animate()
        {
            _animator.SetTrigger(KnockOut);
        }
    }
}