using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PushObstacle : MonoBehaviour
{
    [SerializeField] private float _forceToApply;

    private void OnTriggerEnter(Collider other)
    {
        var runner = other.GetComponent<AbstractRunner>();
        if (runner != null)
        {
            runner.ApplyForce(this.transform.forward * _forceToApply);
        }
    }

    private void OnValidate()
    {
        this.GetComponent<Collider>().isTrigger = true;
    }
}