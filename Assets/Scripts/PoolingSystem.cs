using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct Pool
{
    public GameObject prefab;
    public string tag;
    public int size;
}

public class PoolingSystem : MonoBehaviour
{
     public static PoolingSystem Instance { get; private set; }
     
     [SerializeField] private Pool[] _pools;
     private Dictionary<string, Queue<GameObject>> _poolDictionary = null;
     private Dictionary<string, GameObject> _prefabDictionary = null;

     private void Awake()
     {
         if (Instance != null)
         {
             Destroy(this.gameObject);
         }
         else
         { 
             Instance = this; 
             DontDestroyOnLoad(this);
         }
     }

     private void Start()
     {
         if (_poolDictionary == null)
         {
             _poolDictionary = new Dictionary<string, Queue<GameObject>>();
             _prefabDictionary = new Dictionary<string, GameObject>();

             for (int i = 0; i < _pools.Length; i++)
             {
                 Queue<GameObject> newPool = new Queue<GameObject>();

                 _prefabDictionary.Add(_pools[i].tag, _pools[i].prefab);
                 
                 for (int j = 0; j < _pools[i].size; j++)
                 {
                     newPool.Enqueue(CreateNewObject(_pools[i].tag));
                 }

                 _poolDictionary.Add(_pools[i].tag, newPool);
             }
             
             // Needs to be done here
             // The functions requires the creation of the pool dictionary;
             SceneManager.activeSceneChanged += TurnOffAllObjects;
         }
     }

     public GameObject Spawn(string objectTag, Vector3 worldPosition, Quaternion rotation)
     {
         if (!_poolDictionary.ContainsKey(objectTag)) return null;

         GameObject objectToSpawn = GetNextObjectInPool(objectTag);

         objectToSpawn.transform.position = worldPosition;
         objectToSpawn.transform.rotation = rotation;
         objectToSpawn.SetActive(true);
         
         // Puts the selected object on the end os the queue
         if (_poolDictionary[objectTag].Contains(objectToSpawn))
         {
             _poolDictionary[objectTag].Enqueue(_poolDictionary[objectTag].Dequeue());
         }
         else
         {
            _poolDictionary[objectTag].Enqueue(objectToSpawn);   
         }

         return objectToSpawn;
     }

     private GameObject GetNextObjectInPool(string objectTag)
     {
         if (_poolDictionary[objectTag].Count == 0 || _poolDictionary[objectTag].Peek().activeInHierarchy)
         {
             return CreateNewObject(objectTag);
         }
         else
         {
             return _poolDictionary[objectTag].Peek();
         }
     }

     private GameObject CreateNewObject(string objectTag)
     {
         GameObject newGameObject = Instantiate(_prefabDictionary[objectTag]);
         newGameObject.SetActive(false);
         DontDestroyOnLoad(newGameObject);
         return newGameObject;
     }

     private void TurnOffAllObjects(Scene from, Scene to)
     {
         foreach (var poolTagPair in _poolDictionary)
         {
             foreach (var obj in poolTagPair.Value)
             {
                 obj.SetActive(false);
             }
         }
     }
}