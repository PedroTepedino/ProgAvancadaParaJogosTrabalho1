using UnityEngine;

public static class RigidBodyExtensions
{
    public static bool MovingForward(this Rigidbody rigidbody)
    {
        // If not moving Calculates as if going forward
        if (rigidbody.velocity.magnitude < 0.1f && rigidbody.velocity.magnitude > -0.1f) return true;
        
        var groundVelocity = rigidbody.velocity;
        groundVelocity.y = 0f;
        return Vector3.Dot(groundVelocity, rigidbody.transform.forward) > 0;
    }

    public static float LocalForwardVelocity(this Rigidbody rigidbody)
    {
        float magnitude = Vector3.Project(rigidbody.GroundVelocity(), rigidbody.transform.forward).magnitude;
        if (magnitude < 0.1f) return 0f;

        return magnitude * (Vector3.Dot(rigidbody.GroundVelocity(), rigidbody.transform.forward) >= 0 ? 1f : -1f);
    }

    public static Vector3 GroundVelocity(this Rigidbody rigidbody)
    {
        Vector3 velocity = rigidbody.velocity;
        return new Vector3(velocity.x, 0, velocity.z);
    }

    public static float LocalSideVelocity(this Rigidbody rigidbody)
    {
        float magnitude = Vector3.Project(rigidbody.GroundVelocity(), rigidbody.transform.right).magnitude;
        if (magnitude < 0.1f) return 0f;

        return magnitude * (Vector3.Dot(rigidbody.GroundVelocity(), rigidbody.transform.right) >= 0 ? 1f : -1f);
    }
}