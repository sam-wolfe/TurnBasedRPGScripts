using System.Collections;
using System.Collections.Generic;
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

    public void Show() {
        visualMesh.enabled = true;
    }

    public void Hide() {
        visualMesh.enabled = false;
    }
}
