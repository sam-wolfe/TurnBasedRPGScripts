using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdoll : MonoBehaviour {

    [SerializeField] private Transform _ragdollRoot;
    [SerializeField] public Rigidbody rbHips;

    public void Setup(Transform originalRoot) {
        MatchAllChildTransforms(originalRoot, _ragdollRoot);
    }

    private void MatchAllChildTransforms(Transform root, Transform clone) {
        foreach (Transform child in root) {
            Transform cloneChild = clone.Find(child.name);
            if (cloneChild != null) {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;
                MatchAllChildTransforms(child, cloneChild);
            }
        }
    }
    
}
