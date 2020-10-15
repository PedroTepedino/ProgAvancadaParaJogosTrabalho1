using UnityEngine;

public abstract class PowerUp
{
    protected RunnerPowerUpManager _owner;
    public PowerUp(RunnerPowerUpManager powerUpManager)
    {
        _owner = powerUpManager;
    }
    
    public abstract void Use();
}

public class SwiftnessPotion : PowerUp
{
    private readonly float _forceMultiplier = 20f;
    public SwiftnessPotion(RunnerPowerUpManager powerUpManager) : base(powerUpManager)
    {
    }
    
    public override void Use()
    {
        _owner.Runner.ApplyForce(_owner.transform.forward * _forceMultiplier);
    }
}

public class BowlingBall : PowerUp
{
    public BowlingBall(RunnerPowerUpManager powerUpManager) : base(powerUpManager)
    {
    }

    public override void Use()
    {
        var transform = _owner.transform;
        PoolingSystem.Instance.Spawn("BowlingBall", (transform.position + transform.forward + transform.up),transform.rotation);
    }
}

public class StunBox : PowerUp
{
    public StunBox(RunnerPowerUpManager powerUpManager) : base(powerUpManager)
    {
        
    }

    public override void Use()
    {
        var transform = _owner.transform;
        var obj = PoolingSystem.Instance.Spawn("StunCrate", (transform.position - (transform.forward * 1.7f) + transform.up), Quaternion.identity);
        obj.SetActive(true);
    }
}

