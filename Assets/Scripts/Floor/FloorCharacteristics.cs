using UnityEngine;

[CreateAssetMenu(fileName = "NewFloor", menuName = "Floor Characteristic", order = 10)]
public class FloorCharacteristics : ScriptableObject
{
    [SerializeField] private string _floorTag;
    [SerializeField] private float _velocity;

    public string FloorTag => _floorTag;
    public float Velocity => _velocity;
}