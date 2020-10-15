using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class RunnerPowerUpManager : MonoBehaviour
{
    public Rigidbody Rigidbody { get; private set; }

    private PowerUp _currentPowerUp;

    public Action<PowerUp> OnPowerUpChanged;

    private AbstractRunner _runner;

    public AbstractRunner Runner => _runner;

    private void Awake()
    {
        Rigidbody = this.GetComponent<Rigidbody>();
        _runner = this.GetComponent<AbstractRunner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUpBox"))
        {
            var box = other.GetComponent<PickUpBoxes>();
            if (box != null)
            {
                if (_currentPowerUp == null)
                {
                    _currentPowerUp = box.PickUpPower(this);
                    OnPowerUpChanged?.Invoke(_currentPowerUp);
                }
                box.PickUp();
            }
        }
    }

    public void UsePowerUp()
    {
        if (_currentPowerUp == null) return;

        _currentPowerUp.Use();
        _currentPowerUp = null;
        OnPowerUpChanged?.Invoke(_currentPowerUp);
    }

}
