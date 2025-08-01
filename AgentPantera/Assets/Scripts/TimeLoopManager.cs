using System;
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

    public EventHandler<TimerTickEventArgs> OnTimerTick;
    public EventHandler OnLoopTrigger;
    
    private PlayerDeath _playerDeath;
    private float _timer;
    private int _ticksLeft;

    private void Awake()
    {
        _playerDeath = FindFirstObjectByType<PlayerDeath>();
    }
    
    private void Start()
    {
        _playerDeath.OnPlayerDeath += LoopTime;
        _timer = timeBeforeLoop;
        _ticksLeft = timeBeforeLoop;
        
        OnTimerTick.Invoke(this, new TimerTickEventArgs { SecondsLeft = timeBeforeLoop });
        
        DontDestroyOnLoad(gameObject);
        
        SceneManager.sceneLoaded += (_, _) =>
        {
            _playerDeath = FindFirstObjectByType<PlayerDeath>();
            _playerDeath.OnPlayerDeath += LoopTime;
        };
    }

    private void Update()
    {
        _timer -= Time.deltaTime;

        if (Math.Ceiling(_timer) < _ticksLeft)
        {
            _ticksLeft--;
            OnTimerTick.Invoke(this, new TimerTickEventArgs { SecondsLeft = _ticksLeft });
        }
        
        if (_timer <= 0)
            LoopTime(this, EventArgs.Empty);
    }

    private void LoopTime(object sender, EventArgs e)
    {
        OnLoopTrigger.Invoke(this, EventArgs.Empty);
        
        Destroy(this);
        
        SceneManager.LoadScene(sceneToLoopBackTo);
    }
}
