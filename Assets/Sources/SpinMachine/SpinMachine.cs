using System.Collections.Generic;
using System.Linq;
using Assets.Sources.SpinStates;
using AxGrid;
using AxGrid.Base;
using AxGrid.Path;
using DG.Tweening;
using Sources.SpinMachine;
using Sources.Wallet;
using UnityEngine;
using UnityEngine.UI;

public class SpinMachine : MonoBehaviourExtBind
{
    [SerializeField] private ScrollRect _scrollTransform;
    [SerializeField] private List<Slot> _slotTemplates;
    [SerializeField] private List<Slot> _slots = new ();

    [SerializeField] private Wallet _wallet;
    
    [SerializeField] private SlotRewardConfig _config;

    [SerializeField] private RectTransform _indicator;

    private float _nextUpdatePointY;
    private float _slotYSize;

    private Accelerator _accelerator;

    public int i = 1;

    [OnAwake]
    private void AwakeThis()
    {
        _slotYSize = -_slotTemplates.FirstOrDefault().GetComponent<RectTransform>().sizeDelta.y;
        _nextUpdatePointY = _slotYSize;

        _accelerator = new (_scrollTransform.content);

        Settings.Fsm = new ();
        Settings.Fsm.Add(new SpinMachineWaitState());

        var spinState = new SpinMachineSpinState();
        spinState.Init(_accelerator);
        Settings.Fsm.Add(spinState);

        var stopState = new SpinMachineStopState();
        stopState.Init(_accelerator);
        Settings.Fsm.Add(stopState);

        _accelerator.Stopped += OnDropped;
    }

    [OnStart]
    private void StartThis()
    {
        Settings.Fsm.Start("SpinMachineWait");
    }

    [OnUpdate]
    private void UpdateThis()
    {
        if (_scrollTransform.content.localPosition.y <= _nextUpdatePointY &&
            Settings.Fsm.CurrentStateName != "SpinMachineWait")
        {
            _nextUpdatePointY += _slotYSize;
            CreateNewSlot();
        }

        Settings.Fsm.Update(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        _accelerator.Update();
    }

    private void CreateNewSlot()
    {
        var newSlot = Instantiate(_slotTemplates[Random.Range(0, _slotTemplates.Count)], _scrollTransform.content);
        _slots.Add(newSlot);

        newSlot.gameObject.name += " " + i;
        i++;
    }

    private void OnDropped()
    {
        float minimumDistance = -1;
        Slot nearestSlot = _slots.First();

        for (int i = _slots.Count - 1; i > _slots.Count - 9; i--)
        {
            var distance =
                Mathf.Abs(Vector2.Distance(_slots[i].GetComponent<RectTransform>().position, _indicator.position));
            if (minimumDistance == -1)
            {
                minimumDistance = distance;
                continue;
            }
            
            if (distance < minimumDistance)
            {
                nearestSlot = _slots[i];
                minimumDistance = distance;
            }
        }

        var rect = nearestSlot.GetComponent<RectTransform>();

        float number = _slots.IndexOf(nearestSlot);

        var pos = (-(rect.sizeDelta.y * number) + rect.sizeDelta.y * 1.7f);
        Path = new CPath().Wait(.4f).Action(() =>
            _scrollTransform.content.DOMoveY(pos, .4f).OnComplete(() =>
            {
                nearestSlot.Animate();
                int reward = _config.RewardPerSlots.FirstOrDefault(x => x.SlotType == nearestSlot.SlotType).RewardValue;
                //Settings.Model.Set("WalletValue", Settings.Model.Get<int>("WalletValue") + reward);

                PlayerPrefs.SetInt("WalletValue", PlayerPrefs.GetInt("WalletValue") + reward);
                _wallet.OnWalletValueChanged(PlayerPrefs.GetInt("WalletValue"));
            }));
    }
}