using System.Linq;
using UnityEditor;
using UnityEngine;

public class TrackBuilder : EditorWindow
{
    private Texture2D _trackTexture;
    private TrackTypeColors[] _trackTypeColors;

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
            Debug.Log(_trackTypeColors.Length);
            BuildTrack();    
        }
    }

    private void BuildTrack()
    {
        
    }
}
