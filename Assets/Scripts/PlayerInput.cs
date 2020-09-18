﻿using System;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;

public class PlayerInput : MonoBehaviour
{
    public event Action<bool> OnWalking;

    public static PlayerInput Instance { get; private set; }

    public float Vertical => Input.GetAxis("Vertical");

    public float HorizontalCam => Input.GetAxis("HorizontalCam");

    public float ForwardCam => Input.GetAxis("MouseScrollWheel");

    public int Acceleration
    {
        get
        {
            var input = Input.GetAxis("Vertical");
            if (input > 0.1f) return 1;
            if (input < -0.1f) return -1;
            return 0;
        }
    }

    public float Horizontal => Input.GetAxis("Horizontal");
    //public bool Walking => Input.GetButton("Walk");


    private void Awake()
    {
        if (Instance != null) Destroy(this);
        
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public void Tick()
    {
        if (Input.GetButtonDown("Walk"))
        {
            OnWalking?.Invoke(true);
        }
        else if (Input.GetButtonUp("Walk"))
        {
            OnWalking?.Invoke(false);
        }
    }
}