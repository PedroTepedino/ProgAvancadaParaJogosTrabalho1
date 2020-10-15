using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private float _forwardDeviation = 10f;
    [SerializeField] private float _radius = 10f;
    [SerializeField] private LayerMask _layers;

    private Collider[] _objectsColliders = new Collider[10];

    [SerializeField] private Ai _ai;

    private Transform _currentPowerUp = null;

    public Transform CurrentPowerUp => _currentPowerUp;
    

    public Transform FindPowerUp()
    {
        _objectsColliders = new Collider[10];
        var currentPosition = this.transform.position;
        int objectsFound =
            Physics.OverlapSphereNonAlloc(currentPosition + (this.transform.forward * _forwardDeviation), _radius, _objectsColliders, _layers);
        var aux = _objectsColliders.Where(obj => obj != null && obj.CompareTag("PickUpBox")).ToArray();
        if (aux.Length > 0)
        {
            
            var closest = _objectsColliders[0];
            for (var index = 0; index < aux.Length; index++)
            {
                var col = aux[index];
                if ((col.transform.position - currentPosition).magnitude <
                    (closest.transform.position - currentPosition).magnitude
                    && Vector3.Dot(col.transform.position - currentPosition,
                        (_ai.CurrentWayPoint.position - currentPosition)) > 0)
                {
                    closest = col;
                }
            }

            /*if (!(Vector3.Dot(closest.transform.position - currentPosition,
                (_ai.CurrentWayPoint.position - currentPosition)) > 0))*/
            {
                return closest.transform;
            }
        }

        return null;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position + (this.transform.forward * _forwardDeviation), _radius);
        
    }

    private void OnValidate()
    {
        if (_ai == null)
        {
            _ai = this.GetComponent<Ai>();
        }
    }
}
