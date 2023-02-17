using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour {

    private Vector3 _targetPositon;
    private Vector3 _moveDirection;
    [SerializeField] private Transform bulletHitVFX;

    public void Setup(Vector3 targetPosition) {
        _targetPositon = targetPosition;
        
        // Remove y component from target position
        _targetPositon.y = transform.position.y;
        
        _moveDirection = (_targetPositon - transform.position).normalized;
        
    }

    private void Update() {

        float moveSpeed = 140f;
        
        float distanceBeforeMoving = Vector3.Distance(transform.position, _targetPositon);
            
        transform.position += _moveDirection * moveSpeed * Time.deltaTime;
        
        float distanceAfterMoving = Vector3.Distance(transform.position, _targetPositon);
        
        if (distanceBeforeMoving < distanceAfterMoving ) {
            transform.position = _targetPositon;
            gameObject.GetComponentInChildren<TrailRenderer>().transform.parent = null;
            Destroy(gameObject);
            Instantiate(bulletHitVFX, _targetPositon, Quaternion.identity);
        }
        
    }

}
