using UnityEngine;

public class DestroyAllDontDestroyOnLoad : MonoBehaviour
{
    void Start()
    {
        Destroy(FindFirstObjectByType<TimeLoopManager>().gameObject);
        Destroy(FindFirstObjectByType<MainCanvas>().gameObject);
    }
}
