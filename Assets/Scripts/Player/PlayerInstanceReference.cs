using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInstanceReference : MonoBehaviour
{
    public static PlayerInstanceReference Instance { get; private set; }

    public RunnerPowerUpManager PowerUpManager { get; private set; }
    private void OnEnable()
    {
        Instance = this;
        PowerUpManager = this.GetComponent<RunnerPowerUpManager>();
    }

    private void OnDisable()
    {
        Instance = null;
    }
}
