using UnityEngine;
using UnityEngine.AI;

public static class NavmeshAgentExtensions
{
    public static bool MovingForward(this NavMeshAgent agent)
    {
        // If not moving Calculates as if going forward
        if (agent.velocity.magnitude < 0.1f && agent.velocity.magnitude > -0.1f) return true;
        
        var groundVelocity = agent.velocity;
        groundVelocity.y = 0f;
        return Vector3.Dot(groundVelocity, agent.transform.forward) > 0;
    }

    public static float LocalForwardVelocity(this NavMeshAgent agent)
    {
        float magnitude = Vector3.Project(agent.GroundVelocity(), agent.transform.forward).magnitude;
        if (magnitude < 0.1f) return 0f;

        return magnitude * (Vector3.Dot(agent.GroundVelocity(), agent.transform.forward) >= 0 ? 1f : -1f);
    }

    public static Vector3 GroundVelocity(this NavMeshAgent agent)
    {
        Vector3 velocity = agent.velocity;
        return new Vector3(velocity.x, 0, velocity.z);
    }

    public static float LocalSideVelocity(this NavMeshAgent agent)
    {
        float magnitude = Vector3.Project(agent.GroundVelocity(), agent.transform.right).magnitude;
        if (magnitude < 0.1f) return 0f;

        return magnitude * (Vector3.Dot(agent.GroundVelocity(), agent.transform.right) >= 0 ? 1f : -1f);
    }
}