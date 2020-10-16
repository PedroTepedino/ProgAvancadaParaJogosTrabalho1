using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerVortex : MonoBehaviour
{
    [SerializeField] private float _forceMultiplier = 10f;
    private void OnTriggerEnter(Collider other)
    {
        var runner = other.GetComponent<AbstractRunner>();
        if (runner != null)
        {
            runner.ApplyForce(Quaternion.AngleAxis(Random.Range(-180f, 180f), Vector3.up) * (Vector3.forward * _forceMultiplier));
            runner.Stun();
        }
    }
}