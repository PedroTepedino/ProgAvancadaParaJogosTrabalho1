using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Waypoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Runner"))
        {
            other.GetComponent<Ai>().ReachedWayPoint(this.transform);
        }
    }
}