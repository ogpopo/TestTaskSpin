using System;
using System.Collections.Generic;
using Sources.Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "SlotRewardConfig")]
public class SlotRewardConfig : ScriptableObject
{
    [field:SerializeField] public List<RewardPerSlotData> RewardPerSlots { get; set; } = new();
    
    [Serializable]
    public class RewardPerSlotData
    {
        public SlotType SlotType;
        public int RewardValue;
    }
}