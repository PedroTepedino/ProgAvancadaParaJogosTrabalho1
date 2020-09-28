using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TrackBuilder : EditorWindow
{
    private Texture2D _trackTexture;
    private TrackTypeColors[] _trackTypeColors;
    private Dictionary<Color, GameObject> _trackDictionary;

    [MenuItem("Tools/Track Builder")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<TrackBuilder>("Track Builder");
    }

    private void OnGUI()
    {
        _trackTexture = (Texture2D) EditorGUILayout.ObjectField("Track Texture Base", _trackTexture, typeof(Texture2D), false);
        
        _trackTypeColors = Resources.LoadAll<TrackTypeColors>("TrackColorTypes/");

        if (GUILayout.Button("Build Track") && _trackTexture != null)
        {
            _trackDictionary = _trackTypeColors.ToDictionary(track => track.Color, track => track.TrackPrefab);
            BuildTrack();    
        }
    }

    private void BuildTrack()
    {
        var parentObject = new GameObject("Track");
        parentObject.transform.position = Vector3.zero;
        parentObject.transform.rotation = Quaternion.identity;
        var initialI = 5 * _trackTexture.width / 2;
        var initialJ = 5 * _trackTexture.height / 2;
        for (int i = 0; i < _trackTexture.width; i++)
        {
            for (int j = 0; j < _trackTexture.height; j++)
            {
                var color = _trackTexture.GetPixel(i, j);
                if (_trackDictionary.ContainsKey(color))
                {
                    var obj = PrefabUtility.InstantiatePrefab(_trackDictionary[color]) as GameObject;
                    // var obj = GameObject.Instantiate(_trackDictionary[color], parentObject.transform, true);
                    obj.transform.position = new Vector3(i * 5f - (initialI), 0f, j * 5f - (initialJ));
                    obj.transform.rotation = Quaternion.identity;
                    obj.transform.SetParent(parentObject.transform);
                    obj.name = $"{i}x{j}";
                }
            }
        }
    }
}
