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
        _owner.Rigidbody.AddForce(_owner.transform.forward * _forceMultiplier, ForceMode.Impulse);   
    }
}

public class BowlingBall : PowerUp
{
    public BowlingBall(RunnerPowerUpManager powerUpManager) : base(powerUpManager)
    {
    }

    public override void Use()
    {
        // TODO: launch bowling ball
    }
}

public class StunBox : PowerUp
{
    public StunBox(RunnerPowerUpManager powerUpManager) : base(powerUpManager)
    {
    }

    public override void Use()
    {
        // TODO: Drop Stun Box
    }
}