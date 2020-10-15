#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

public enum Directions { North, South, East, West}

public class RotateFloorLetters : MonoBehaviour
{
    [SerializeField] private Directions _currentDirection = Directions.North;
    [SerializeField] private GameObject _letters;

    private Dictionary<Directions, float> DirectionAngleDictionary = new Dictionary<Directions, float>
    {
        {Directions.North, 180f},
        {Directions.East, 90f},
        {Directions.South, 0f},
        {Directions.West, 270f}
    };

    private void OnValidate()
    {
        if (_letters == null)
        {
            if (this.transform.childCount > 0)
            {
                _letters = this.transform.GetChild(0).gameObject;
            }
            else
            {
                _letters = this.gameObject;
            }
        }

        _letters.transform.rotation = Quaternion.Euler(new Vector3(-90, DirectionAngleDictionary[_currentDirection], 0));
    }
}
#endif