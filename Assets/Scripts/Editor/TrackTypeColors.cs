using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Track Type", menuName = "Track Type Color", order = 2)]
public class TrackTypeColors : ScriptableObject
{
    [SerializeField] private GameObject _trackPrefab;
    [SerializeField] private Color _color;

    public GameObject TrackPrefab => _trackPrefab;
    public Color Color => _color;
}
