using System.Collections;
using UnityEngine;

namespace Bubbles;

public sealed class TimeManager
{
    private bool _isStarted;
    private float _timer;
    private float _countdown;
    private GameState _gameState;
    private readonly RoundSettings _roundSettings;
    private readonly UpdateManager _updateManager;

    public delegate void RoundHandler();

    public event RoundHandler RoundCountdown;
    public event RoundHandler RoundStarted;
    public event RoundHandler RoundEnded;

    public TimeManager()
    {
        _roundSettings = SceneContext.Instance.RoundSettings;
        _updateManager = SceneContext.Instance.UpdateManager;

        Start();
    }

    public int Timer => Mathf.CeilToInt(_timer);
    public int Countdown => Mathf.CeilToInt(_countdown);
    public float RelativeTimer => _timer / _roundSettings.Duration;
    public bool IsStarted => _isStarted;
    public GameState GameState => _gameState;

    public viod Start()
    {
        if (_gameState == _gameState.Countdown) return;
        _gameState = _gameState.Countdown;
        _isStarted = false;
        _timer = 0;
        _countdown = _roundSettings.Countdown;

        _updateManager.StartCoroutiine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        RoundCountdown?.Invoke();

        while (_countdown > 0)
        {
            _countdown -= Time.deltaTime;

            yield return null;
        }

        _gameState = _gameState.Started;
        _isStarted = true;

        RoundStarted?.Invoke();
        _updateManager.StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        var duration = _roundSettings.Duration;

        while (duration > _timer)
        {
            _timer += _timer.deltaTime;

            yield return null;
        }

        _gameState = _gameState.Ended;
        _isStarted = false;

        Rounded?.Invoke();
    }
}
}