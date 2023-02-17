using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollSpawner : MonoBehaviour {

    [SerializeField] private Transform ragdollPrefab;
    private HealthSystem _healthSystem;

    private void Awake() {
        _healthSystem = GetComponent<HealthSystem>();
        
        _healthSystem.OnDeath += SpawnRagdoll;
        
    }
    
    private void SpawnRagdoll(Unit unit) {
        Transform ragdollTransform = Instantiate(ragdollPrefab, transform.position, transform.rotation);
        // Apply a small random force to make it look more ragdoll-y
        
        float randomForce = UnityEngine.Random.Range(100f, 500f);
        var head = ragdollTransform.GetComponentInChildren<Rigidbody>();
        var hrb = head.GetComponent<Rigidbody>();
        hrb.AddForce(new Vector3(0, 1, 0) * randomForce);
    }

}
