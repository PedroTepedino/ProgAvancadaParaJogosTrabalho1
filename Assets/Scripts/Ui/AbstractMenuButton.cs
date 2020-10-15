using System;
using UnityEngine.UI;

public abstract class AbstractMenuButton<T> : Button where T : AbstractMenuButton<T>
{
    protected static T _instance;
        
    public static bool Pressed => _instance != null && _instance.IsPressed();
    
    protected override void OnEnable()
    {
        _instance = this as T;
        base.OnEnable();
    }
}