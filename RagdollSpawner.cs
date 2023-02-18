using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollSpawner : MonoBehaviour {

    [SerializeField] private Transform ragdollPrefab;
    [SerializeField] private Transform originalRoot;
    private HealthSystem _healthSystem;

    private void Awake() {
        _healthSystem = GetComponent<HealthSystem>();
        
        _healthSystem.OnDeath += SpawnRagdoll;
        
    }
    
    private void SpawnRagdoll(Unit unit) {
        Transform ragdollTransform = Instantiate(ragdollPrefab, transform.position, transform.rotation);

        // Copy the position and rotation of the unit to the ragdoll using Setup 
        ragdollTransform.GetComponent<UnitRagdoll>().Setup(originalRoot);
        
        float randomForce = UnityEngine.Random.Range(100f, 500f);
        var head = ragdollTransform.GetComponentInChildren<Rigidbody>();
        var hrb = head.GetComponent<Rigidbody>();
        hrb.AddForce(new Vector3(0, 1, 0) * randomForce);
        
        hrb.AddTorque(new Vector3(0, 1, 0) * randomForce);
    }

}
