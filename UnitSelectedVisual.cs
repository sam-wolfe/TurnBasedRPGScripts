using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour {


    [SerializeField] private Unit unit;

    private MeshRenderer _meshRenderer;

    private void Awake() {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start() {
        // Codemonkey did it this way, but I prefer it my way.
        // UnitActionSystem.instance.OnSelectedUnitChanged += HandleSelectedUnitChanged;
        
        unit.OnUnitSelected += HandleSelectedUnitSelected;
        unit.OnUnitDeSelected += HandleSelectedUnitDeSelected;
        
        // Show in edit mode, disable when play mode starts
        _meshRenderer.enabled = false;
    }

    private void HandleSelectedUnitSelected() {
        _meshRenderer.enabled = true;
    }
    
    private void HandleSelectedUnitDeSelected() {
        _meshRenderer.enabled = false;
    }

}
