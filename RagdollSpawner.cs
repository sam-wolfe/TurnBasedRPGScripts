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
        
        
        // Randomly make int either 1 or -1
        int randomDirection = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;

        UnitRagdoll ragdoll = ragdollTransform.GetComponent<UnitRagdoll>();
        
        ragdoll.rbHips.AddTorque(new Vector3(0, randomDirection, 0) * 5000f, ForceMode.Impulse);
    }

}
