using System;
using UnityEngine;

namespace Sources.SpinMachine
{
    public class Accelerator
    {
        private Transform _scrollTransform;
        private float _spinSpeed;

        private float _timer = 0;
        private int _changesNumberCount;
        private float _lastUpdateTime = -1;

        private const float AccelerationValue = 5f;
        private const float StopSpeedValue = 5f;
        private const int NeedNumberChange = 10;

        public Action Stopped;
        public Action Accelerated;

        public Accelerator(Transform scrollTransform)
        {
            _scrollTransform = scrollTransform;
        }

        public void Update()
        {
            if (_scrollTransform != null || _spinSpeed == 0)
            {
                _scrollTransform.position += Vector3.down * _spinSpeed;
            }
        }

        public void ResetData()
        {
            _timer = 0;
            _changesNumberCount = 0;
            _lastUpdateTime = -1;
        }

        public void Accelerate(float delta)
        {
            SpeedChange(IncreaseSpinSpeed, Accelerated, delta);
        }

        public void StopAccelerate(float delta)
        {
            SpeedChange(ReduceSpinSpeed, Stopped, delta);
        }

        private void SpeedChange(Action speedChangeCallback, Action completionCallback, float delta)
        {
            if (_changesNumberCount == NeedNumberChange)
            {
                return;
            }

            var timer = (float) Math.Round(_timer, 2, MidpointRounding.AwayFromZero);

            if (timer % .01f == 0 && timer != _lastUpdateTime)
            {
                speedChangeCallback?.Invoke();
                _changesNumberCount++;
                _lastUpdateTime = timer;
            }

            _timer += delta;
            
            if (_changesNumberCount == NeedNumberChange || _spinSpeed <= 0)
            {
                completionCallback?.Invoke();
                _changesNumberCount = NeedNumberChange;
            }
        }

        private void IncreaseSpinSpeed()
        {
            _spinSpeed += AccelerationValue;
        }

        private void ReduceSpinSpeed()
        {
            _spinSpeed -= StopSpeedValue;
        }
    }
}