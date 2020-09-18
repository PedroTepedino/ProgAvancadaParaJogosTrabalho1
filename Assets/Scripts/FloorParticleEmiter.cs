using Unity.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FloorParticleEmiter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        string particleToSpawn = "RoadParticle";

        if (other.CompareTag("Dirt"))
        {
            particleToSpawn = "DirtParticle";
        }
        else if (other.CompareTag("Grass"))
        {
            particleToSpawn = "GrassParticle";
        }
        else if (other.CompareTag("Mud"))
        {
            particleToSpawn = "MudParticle";
        }
        else if (other.CompareTag("WetFloor"))
        {
            particleToSpawn = "WetFloorParticle";
        }

        PoolingSystem.Instance.Spawn(particleToSpawn, this.transform.position);
    }
}