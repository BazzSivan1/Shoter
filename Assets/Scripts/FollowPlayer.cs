using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform _pivotX;
    [SerializeField] private Transform _pivotY;
    [SerializeField] private Transform _target;
    [SerializeField] private bool _lookAt;
    [SerializeField] private float _rotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_lookAt)
        {
            _pivotX.transform.LookAt(_target.position);
        }
        else
        {
            if (_pivotX)
            {
                Vector3 lookDirection = _target.position - _pivotX.position;
                lookDirection.y = 0;
                Quaternion lookRotation = Quaternion.LookRotation(lookDirection.normalized);

                _pivotX.rotation = Quaternion.RotateTowards(_pivotX.rotation, lookRotation, _rotateSpeed * Time.deltaTime);
            }

            if (_pivotY)
            {
                Vector3 lookDirection = _target.position - _pivotY.position;
                lookDirection.x = 0;
                lookDirection.z = 0;
                Quaternion lookRotation = Quaternion.LookRotation(lookDirection.normalized);

                _pivotY.localRotation = Quaternion.RotateTowards(_pivotY.rotation, lookRotation, _rotateSpeed * Time.deltaTime);
            }
        }
    }
}
