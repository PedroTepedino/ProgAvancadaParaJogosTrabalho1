using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BoostParticlesActivator : MonoBehaviour
{
    [SerializeField] private ParticleSystem _boostParticleSystem;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Runner _runner;

    private void Update()
    {
        if (_rigidbody.GroundVelocity().magnitude > _runner.MAXRunSpeed)
        {
            if (_boostParticleSystem.isStopped)
                _boostParticleSystem.Play();
        }
        else
        {
            if (_boostParticleSystem.isPlaying)
                _boostParticleSystem.Stop();
        }
    }

    private void OnValidate()
    {
        if (_boostParticleSystem == null)
        {
            _boostParticleSystem = this.GetComponent<ParticleSystem>();
        }

        if (_rigidbody == null)
        {
            _rigidbody = this.GetComponentInParent<Rigidbody>();
        }

        if (_runner == null)
        {
            _runner = this.GetComponentInParent<Runner>();
        }
    }
}
