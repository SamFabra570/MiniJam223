using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isPaused;
    
    [SerializeField] private int maxLifeCount;
    [SerializeField] private int remainingLives;
    
    [SerializeField] private TilePlacementController tilePlacementController;
    [SerializeField] private Tilemap playerBuildTilemap;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        
        Instance = this;
    }

    private void Start()
    {
        UIManager.Instance.SetTileCountUI(tilePlacementController.maxTileCount);
    }

    private void TogglePauseGame()
    {
        if (isPaused)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
        
        isPaused = !isPaused;
    }

    public void EndGame()
    {
        TogglePauseGame();
        
        UIManager.Instance.ToggleEndGameMenu();
    }

    public void RestartLevel()
    {
        UIManager.Instance.ToggleEndGameMenu();
        
        CharacterBehaviour.Instance.ResetPosition();
        UIManager.Instance.SetTileCountUI(tilePlacementController.maxTileCount);
        playerBuildTilemap.ClearAllTiles();
        
        TogglePauseGame();
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
