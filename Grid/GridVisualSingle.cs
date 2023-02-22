using UnityEngine;

public class GridVisualSingle : MonoBehaviour {

    [SerializeField] private MeshRenderer visualMesh;
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(Material material) {
        visualMesh.enabled = true;
        visualMesh.material = material;
    }

    public void Hide() {
        visualMesh.enabled = false;
    }
}
