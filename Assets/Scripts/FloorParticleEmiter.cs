using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FloorParticleEmiter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PoolingSystem.Instance.Spawn("FloorParticle", this.transform.position);
    }
}