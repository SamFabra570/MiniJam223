using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    public GameObject endGameMenu;
    
    [SerializeField] private TextMeshProUGUI tileCountUI;
    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        endGameMenu.SetActive(false);
    }

    public void ToggleEndGameMenu()
    {
        if (endGameMenu.activeSelf)
            endGameMenu.SetActive(false);
        else
            endGameMenu.SetActive(true);
    }

    public void SetTileCountUI(int tileCount)
    {
        tileCountUI.text = tileCount.ToString();
    }
}
