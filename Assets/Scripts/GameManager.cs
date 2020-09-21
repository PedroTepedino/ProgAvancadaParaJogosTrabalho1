using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null) CreateGameManager();
            
            return _instance;
        }  
    }

    private static void CreateGameManager()
    {
        var gameManager = new GameObject("[GAME MANAGER]");
        _instance = gameManager.AddComponent<GameManager>();
    }

    private void Awake()
    {
        if (_instance != null) Destroy(this.gameObject);
        else _instance = this;
        
        DontDestroyOnLoad(this.gameObject);
    }

    public void InitializeGame()
    {
    }
}