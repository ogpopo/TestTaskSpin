using System;
using System.Linq;
using AxGrid;
using AxGrid.Base;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Sources.Wallet
{
    public class Wallet : MonoBehaviourExtBind
    {
        [SerializeField] private TextMeshProUGUI _walletValueText;

        private Tween moneyTween;

        [OnAwake]
        private void AwakeThis()
        {
            // Settings.Model.EventManager.AddAction("WalletValue", OnWalletValueChanged);
            // if (Settings.Model.TryGetValue("WalletValue", out var value) == false)
            // {
            //     Settings.Model.Set("WalletValue", 0);
            // }

            OnWalletValueChanged(PlayerPrefs.GetInt("WalletValue"));
        }
        
        public void OnWalletValueChanged(int newValue)
        {
            DOTween.Kill(moneyTween);
            
            int changableValue = Int32.Parse(string.Join("", _walletValueText.text.Where(c => char.IsDigit(c))));

            moneyTween = DOTween.To(() => changableValue, x => changableValue = x, newValue, .3f).OnUpdate(() =>
            {
                _walletValueText.text = changableValue + "$";
            });
        }
    }
}