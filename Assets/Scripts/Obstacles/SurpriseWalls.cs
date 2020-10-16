using TMPro;
using UnityEngine;

public class SurpriseWalls : MonoBehaviour
{
    [SerializeField] private HiddenWall[] _hiddenWalls;
    [SerializeField] private float _timeBetweenChangeCall = 3f;
    private float _timer = 0f;
    [SerializeField] private float _chanceToChangeWallState = 0.5f;

    private void Update()
    {
        if (_timer <= 0)
        {
            _timer = _timeBetweenChangeCall;
            ChangeHiddenWallsState();
        }
        else
        {
            _timer -= Time.deltaTime;
        }
    }

    private void ChangeHiddenWallsState()
    {
        foreach (var hiddenWall in _hiddenWalls)
        {
            hiddenWall.SetHiddenState(Random.Range(0f, 1f) <= _chanceToChangeWallState);
        }
    }

    private void OnValidate()
    {
        if (_hiddenWalls == null || _hiddenWalls.Length == 0)
        {
            _hiddenWalls = this.GetComponentsInChildren<HiddenWall>();
        }
    }
}