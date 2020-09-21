using UnityEngine;

public class Bootstrapper 
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void Initialize()
    {
        var playerInputGameObject = new GameObject("[PLAYER INPUT]");
        playerInputGameObject.AddComponent<PlayerInput>();
        Object.DontDestroyOnLoad(playerInputGameObject);

        GameManager.Instance.InitializeGame();
    }
}