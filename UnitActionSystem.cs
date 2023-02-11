using System;
using Grid;
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

    private bool _isBusy;

    private void Awake() {
        if (instance != null) {
            Debug.LogError("You added an extra unit action system ya dingus: " + transform + " - " + instance);
            Destroy(gameObject);
            return;
        }
        instance = this;
        Debug.Log(this);
    }

    void Update() {
        if (_isBusy) {
            // TODO early return is from lessons. Refactor if reused in
            //  production code.
            return;
        }
        HandleUnitSelection();
        HandleMovement();
        HandleSpin();
    }
    
    private void SetBusy() {
        _isBusy = true;
    }
    
    private void ClearBusy() {
        _isBusy = false;
    }

    private void HandleMovement() {
        if (Input.GetMouseButtonDown(1) && selectedUnit != null) {

            GridPosition mouseGridPosition = LevelGrid.instance.GetGridPosition(MouseWorld.GetPosition());

            if (selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition)) {
                SetBusy();
                selectedUnit.GetMoveAction().Move(ClearBusy, mouseGridPosition);
            }
        }
    }

    private void HandleSpin() {
        if (Input.GetKeyDown(KeyCode.R)) {
            SetBusy();
            selectedUnit.GetSpinAction().Spin(ClearBusy);
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
        
        // Added this back in later to communicate to classes that arent
        // children of Unit.
        OnSelectedUnitChanged?.Invoke(unit);
        
        
        
        // CodeMonkey did it this way with a singleton pattern, my way has the unit as the abstraction between 
        // the action system and the indicator.
        // OnSelectedUnitChanged?.Invoke(selectedUnit);
        // selectedUnit = unit;
    }

    public Unit GetSelectedUnit => selectedUnit;

    
}
