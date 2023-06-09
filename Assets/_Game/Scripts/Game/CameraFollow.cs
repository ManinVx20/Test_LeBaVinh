using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : CustomMonoBehaviour
{
    [SerializeField]
    private Vector3 _offset = new Vector3(0.0f, 0.0f, -10.0f);
    [SerializeField]
    private float _smoothTime = 0.25f;

    private Transform _targetTransform;
    private Vector3 _currentVelocity;

    private void LateUpdate()
    {
        if (_targetTransform != null)
        {
            GetTransform().position = Vector3.SmoothDamp(GetTransform().position, _targetTransform.position + _offset, ref _currentVelocity, _smoothTime);
        }
    }

    public void SetTarget(Transform targetTransform)
    {
        _targetTransform = targetTransform;

        if (_targetTransform != null)
        {
            _currentVelocity = Vector3.zero;

            GetTransform().position = _targetTransform.position + _offset;
        }
    }
}
