using UnityEngine;

public class KoPipe : MonoBehaviour
{
    [SerializeField] private float _shotInterval = 5f;
    private float _timer = 0;

    private void Awake()
    {
        _timer = _shotInterval;
    }

    private void Update()
    {
        if (_timer <= 0)
        {
            _timer = _shotInterval + Random.Range(-2f, 2f);
            ShotBoxingGlove();
        }
        else
        {
            _timer -= Time.deltaTime;
        }
    }

    private void ShotBoxingGlove()
    {
        PoolingSystem.Instance.Spawn("KoGlove", this.transform.position, this.transform.rotation);
    }
}