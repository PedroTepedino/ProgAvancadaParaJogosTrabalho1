using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Waypoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Runner"))
        {
            AbstractRunner runner = other.GetComponent<AbstractRunner>();
            if (runner != null)
            {
                runner.ReachedWayPoint(this.transform);
            }
        }
    }
}