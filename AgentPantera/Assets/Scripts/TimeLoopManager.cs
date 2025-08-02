using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerTickEventArgs : EventArgs
{
    public int SecondsLeft { get; set; }
}

public class TimeLoopManager : MonoBehaviour
{
    [SerializeField] private int timeBeforeLoop;
    [SerializeField] private string sceneToLoopBackTo;
    [SerializeField] private float deathScreenDuration;
    [SerializeField] private GameObject timerText;

    public EventHandler<TimerTickEventArgs> OnTimerTick;
    public EventHandler OnRanOutOfTime;
    
    private PlayerDeath _playerDeath;
    private bool _isTimerRunning;
    private float _timer;
    private int _ticksLeft;

    private void Awake()
    {
        _playerDeath = FindFirstObjectByType<PlayerDeath>();
    }
    
    private void Start()
    {
        _playerDeath.OnPlayerDeath += (_, _) => StartCoroutine(LoopTime());
        _timer = timeBeforeLoop;
        _ticksLeft = timeBeforeLoop;
        
        DontDestroyOnLoad(gameObject);
        
        SceneManager.sceneLoaded += (_, _) =>
        {
            _playerDeath = FindFirstObjectByType<PlayerDeath>();
            _playerDeath.OnPlayerDeath += (_, _) => StartCoroutine(LoopTime());

            if (SceneManager.GetActiveScene().name == sceneToLoopBackTo)
            {
                _isTimerRunning = true;
                OnTimerTick.Invoke(this, new TimerTickEventArgs { SecondsLeft = _ticksLeft });
            }
        };
    }

    private void Update()
    {
        if (!_isTimerRunning)
            return;
        
        _timer -= Time.deltaTime;

        if (Math.Ceiling(_timer) < _ticksLeft)
        {
            _ticksLeft--;
            OnTimerTick.Invoke(this, new TimerTickEventArgs { SecondsLeft = _ticksLeft });
        }

        if (_timer <= 0)
        {
            _timer = timeBeforeLoop;
            _ticksLeft = timeBeforeLoop;
            
            OnRanOutOfTime.Invoke(this, EventArgs.Empty);
            
            StartCoroutine(LoopTime());
        }
    }
    
    private IEnumerator LoopTime()
    {
        yield return new WaitForSeconds(deathScreenDuration);
        
        _timer = timeBeforeLoop;
        _ticksLeft = timeBeforeLoop;
        OnTimerTick.Invoke(this, new TimerTickEventArgs { SecondsLeft = timeBeforeLoop });
        
        SceneManager.LoadScene(sceneToLoopBackTo);
    }
}
