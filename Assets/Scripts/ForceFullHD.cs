using UnityEngine;

public class ForceFullHD : MonoBehaviour
{
    private void Awake()
    {
        Screen.SetResolution(1920, 1080,true, 60);
    }
}
