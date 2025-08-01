using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Timer : MonoBehaviour
{
    private TextMeshProUGUI _timerText;
    private TimeLoopManager _timeLoopManager;

    private void Awake()
    {
        _timerText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        _timeLoopManager = FindFirstObjectByType<TimeLoopManager>();
        
        DontDestroyOnLoad(FindFirstObjectByType<Canvas>().gameObject);
        
        _timeLoopManager.OnTimerTick += (_, args) => _timerText.text = $"{args.SecondsLeft / 60:D2}:{args.SecondsLeft % 60:D2}";
    }
}
