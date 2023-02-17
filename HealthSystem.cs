using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour {

    [SerializeField] private int _health;
    [SerializeField] private int _maxHealth = 100;

    public event Action<Unit> OnDeath; 

    private void Start() {
        _health = _maxHealth;
    }

    public void Damage(int damageAmount) {
        _health -= damageAmount;
        if (_health <= 0) {
            _health = 0;
            Die();
        }
    }

    private void Die() {
        OnDeath?.Invoke(GetComponent<Unit>());
    }
}
