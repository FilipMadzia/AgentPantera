using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMessagePanel : MonoBehaviour
{
    [SerializeField] private GameObject deathMessagePanel;
    
    private PlayerDeath _playerDeath;

    private void Start()
    {
        _playerDeath = FindFirstObjectByType<PlayerDeath>();

        SceneManager.sceneLoaded += (_, _) =>
        {
            deathMessagePanel.SetActive(false);
            
            _playerDeath = FindFirstObjectByType<PlayerDeath>();
            _playerDeath.OnPlayerDeath += (_, _) => deathMessagePanel.SetActive(true);
        };
    }
}
