using System.Collections.Generic;
using System.Linq;
using Assets.Sources.SpinStates;
using AxGrid;
using AxGrid.Base;
using AxGrid.Path;
using DG.Tweening;
using Sources.SpinMachine;
using Sources.Wallet;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpinMachine : MonoBehaviourExtBind
{
    [SerializeField] private Button _spinButton;
    [SerializeField] private Button _stopButton;
    [SerializeField] private TextMeshProUGUI _spinText;
    [SerializeField] private TextMeshProUGUI _stopText;

    [SerializeField] private ScrollRect _scrollTransform;
    [SerializeField] private List<Slot> _slotTemplates;
    [SerializeField] private List<Slot> _slots = new ();

    [SerializeField] private Wallet _wallet;

    [SerializeField] private SlotRewardConfig _config;

    [SerializeField] private RectTransform _indicator;

    private float _nextUpdatePointY;
    private float _slotYSize;

    private Accelerator _accelerator;

    [OnAwake]
    private void AwakeThis()
    {
        _slotYSize = -_slotTemplates.FirstOrDefault().GetComponent<RectTransform>().sizeDelta.y;
        _nextUpdatePointY = _slotYSize;

        _accelerator = new (_scrollTransform.content);

        Settings.Fsm = new ();

        var waitState = new SpinMachineWaitState();
        waitState.Init(this);
        Settings.Fsm.Add(waitState);

        var spinState = new SpinMachineSpinState();
        spinState.Init(_accelerator, this);
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

    [OnStart(RunLevel.Low)]
    private void HighPriorityStart()
    {
        SetActiveSpinElements(true);
        SetActiveStopElements(false);
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

    public void SetActiveSpinElements(bool value)
    {
        _spinText.color = value == true ? Color.white : Color.grey;
        _spinButton.interactable = value;
    }

    public void SetActiveStopElements(bool value)
    {
        _stopText.color = value == true ? Color.white : Color.grey;
        _stopButton.interactable = value;
    }

    private void FixedUpdate()
    {
        _accelerator.Update();
    }

    private void CreateNewSlot()
    {
        var newSlot = Instantiate(_slotTemplates[Random.Range(0, _slotTemplates.Count)], _scrollTransform.content);
        _slots.Add(newSlot);
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

                SetActiveSpinElements(true);
            }));
    }
}