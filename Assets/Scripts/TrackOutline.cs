#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TrackOutline : MonoBehaviour
{
    [SerializeField] private GameObject[] _objects;
    [SerializeField] private string _tag;
    
    [ContextMenu("Replace Everything")]
    public void ReplaceOfftrackColliders()
    {
        var sceneObjects = GameObject.FindGameObjectsWithTag(_tag);

        foreach (var obj in sceneObjects)
        {
            var newObj = PrefabUtility.InstantiatePrefab(_objects[Random.Range(0, _objects.Length)]) as GameObject;
            newObj.name = obj.name;
            newObj.transform.parent = obj.transform.parent;
            newObj.transform.position = obj.transform.position;
            newObj.transform.rotation = obj.transform.rotation;
        }
        
        sceneObjects.ToList().ForEach(DestroyImmediate);
    }
}
#endif