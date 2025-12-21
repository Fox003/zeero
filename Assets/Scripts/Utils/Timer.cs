using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Timer
{
    private bool _isRunning;
    private readonly Action _onFinish;
    private readonly Action _onTick;
    private TimeSpan _refreshInterval;
    private readonly double _duration;
    
    private double _elapsed;

    public Timer(double durationSeconds, TimeSpan refreshInterval, Action onFinish = null, Action onTick = null)
    {
        _duration = durationSeconds;
        _refreshInterval = refreshInterval;
        _onFinish = onFinish;
        _onTick = onTick;
    }

    public void Start()
    {
        if (_isRunning) return;

        _elapsed = 0;
        _isRunning = true;
        Run().Forget();
    }

    public async UniTask Run()
    {
        while (_isRunning)
        {
            await UniTask.Delay(_refreshInterval);

            _elapsed += _refreshInterval.TotalSeconds;
            _onTick?.Invoke();

            if (_elapsed >= _duration)
            {
                _isRunning = false;
                _onFinish?.Invoke();
                return;
            }
        }
    }

    public void Stop()
    {
        _isRunning = false;
    }
}
