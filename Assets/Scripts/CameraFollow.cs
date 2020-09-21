using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _followTarget;
    [SerializeField] private Transform _nearFollowTarget;
    [SerializeField] private Transform _lookTarget;
    [SerializeField] private Transform _nearLookTarget;
    [SerializeField] private float _speed;
    [SerializeField] private float _lookSpeed;
    [SerializeField] private float _rotationSpeed;
    private float _zoomInterpolation = 0f;
    [SerializeField] private float _zoomSpeed;

    private Vector3 _realLookPosition =>
        Vector3.Lerp(_lookTarget.position, _nearLookTarget.position, _zoomInterpolation);

    private Vector3 _realLookDirectionVector => (_realLookPosition - this.transform.position).normalized;
    
    private Quaternion _realRotationToLookAt =>
        Quaternion.LookRotation( _realLookDirectionVector);
    
    private Vector3 _realFollowPosition => 
        Vector3.Lerp(_followTarget.position, _nearFollowTarget.position, _zoomInterpolation);
        

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        var horizontalCamInput = PlayerInput.Instance.HorizontalCam;
        if (Mathf.Abs(horizontalCamInput) > 0.1f)
        {
            _followTarget.RotateAround(_player.position, _player.up, horizontalCamInput * _rotationSpeed);
            _nearFollowTarget.RotateAround(_player.position, _player.up, horizontalCamInput * _rotationSpeed);
        }

        var forwardCamInput = PlayerInput.Instance.ForwardCam;
        _zoomInterpolation += forwardCamInput * _zoomSpeed;
        
        if (_zoomInterpolation > 1f) _zoomInterpolation = 1f;
        else if (_zoomInterpolation < 0f) _zoomInterpolation = 0f; 
    }


    private void FixedUpdate()
    {
        var viewAngle = Vector3.Angle(this.transform.forward, _realLookDirectionVector);

        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, _realRotationToLookAt,
            _lookSpeed * Time.fixedDeltaTime * viewAngle);
        this.transform.position = Vector3.Lerp(this.transform.position, _realFollowPosition, _speed * Time.fixedDeltaTime);
    }


    private void OnDrawGizmos()
    {
        if (_followTarget && _nearFollowTarget && _lookTarget && _nearLookTarget)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_followTarget.position, 0.2f);
            Gizmos.DrawWireSphere(_nearFollowTarget.position, 0.2f);
            Gizmos.DrawLine(_followTarget.position, _nearFollowTarget.position);

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(_lookTarget.position, 0.2f);
            Gizmos.DrawWireSphere(_nearLookTarget.position, 0.2f);
            Gizmos.DrawLine(_lookTarget.position, _nearLookTarget.position);
        }
    }
}
