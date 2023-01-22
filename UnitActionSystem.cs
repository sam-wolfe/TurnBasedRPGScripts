using System;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour {

    public static UnitActionSystem instance { get; private set; }
    
    public event Action<Unit> OnSelectedUnitChanged;

    [SerializeField]
    private Unit selectedUnit;
    // TODO learn to implement this
    // public Unit SelectedUnit => selectedUnit;
    
    [SerializeField] 
    private LayerMask unitLayerMask;

    private void Awake() {
        if (instance != null) {
            Debug.LogError("You added an extra unit action system ya dingus: " + transform + " - " + instance);
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void Update() {
        HandleUnitSelection();
        HandleMovement();
    }

    private void HandleMovement() {
        if (Input.GetMouseButtonDown(1) && selectedUnit != null) {
            selectedUnit.Move(MouseWorld.GetPosition());
        }
    }

    private void HandleUnitSelection() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
            {
                if (raycastHit.transform.TryGetComponent(out Unit unit)) {
                    SetSelectedUnit(unit);
                }
            }
            
            
        }
    }

    private void SetSelectedUnit(Unit unit) {
        selectedUnit?.DeSelect();
        unit.Select();

        selectedUnit = unit;
        
        // CodeMonkey did it this way with a singleton pattern, my way has the unit as the abstraction between 
        // the action system and the indicator.
        // OnSelectedUnitChanged?.Invoke(selectedUnit);
        // selectedUnit = unit;
    }
    
}
