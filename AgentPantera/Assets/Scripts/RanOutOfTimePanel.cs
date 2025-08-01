using UnityEngine;
using UnityEngine.SceneManagement;

public class RanOutOfTimePanel : MonoBehaviour
{
    [SerializeField] private GameObject ranOutOfTimeMessagePanel;
    
    private TimeLoopManager _timeLoopManager;

    private void Start()
    {
        _timeLoopManager = FindFirstObjectByType<TimeLoopManager>();
        _timeLoopManager.OnRanOutOfTime += (_, _) => ranOutOfTimeMessagePanel.SetActive(true);

        SceneManager.sceneLoaded += (_, _) => ranOutOfTimeMessagePanel.SetActive(false);
    }
}
