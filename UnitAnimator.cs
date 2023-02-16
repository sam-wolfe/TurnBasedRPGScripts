using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour {

    [SerializeField] private Animator _animator;

    private void Awake() {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction)) {
            moveAction.OnMoveStart += HandleMoveStart;
            moveAction.OnMoveStop += HandleMoveStop;
            
        }
        if (TryGetComponent<ShootAction>(out ShootAction shootAction)) {
            shootAction.OnShoot += HandleShoot;
        }
    }
    
    private void HandleMoveStart() {
        _animator.SetBool("IsWalking", true);
    }
    
    private void HandleMoveStop() {
        _animator.SetBool("IsWalking", false);
    }
    
    private void HandleShoot() {
        _animator.SetTrigger("Shoot");
    }

}
