using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollSpawner : MonoBehaviour {

    [SerializeField] private Transform ragdollPrefab;
    private HealthSystem _healthSystem;

    private void Awake() {
        _healthSystem = GetComponent<HealthSystem>();
        
    }

}
