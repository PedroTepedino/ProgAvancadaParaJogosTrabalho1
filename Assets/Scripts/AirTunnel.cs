using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AirTunnel : MonoBehaviour
{
    [SerializeField] private float _forceMultiplier;
    [SerializeField] private Vector3 _forceDirection;

    private void OnTriggerStay(Collider other)
    {
        var rigid = other.GetComponent<Rigidbody>();
        if (rigid)
        {
            rigid.AddForce(_forceDirection.normalized * _forceMultiplier);
        }
    }
}