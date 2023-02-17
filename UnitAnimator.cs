using System;
using UnityEngine;

public class UnitAnimator : MonoBehaviour {

    [SerializeField] private Animator _animator;
    [SerializeField] Transform bulletProjectilePrefab;
    [SerializeField] Transform bulletSpawnPoint;

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
    
    private void HandleShoot(object sender, ShootAction.ShootEventArgs e) {
        _animator.SetTrigger("Shoot");

        Transform bulletTransform = Instantiate(bulletProjectilePrefab, bulletSpawnPoint.position, Quaternion.identity);
        BulletProjectile bullet = bulletTransform.GetComponent<BulletProjectile>();
        
        bullet.Setup(e.targetUnit.GetWorldPosition());
    }

}
