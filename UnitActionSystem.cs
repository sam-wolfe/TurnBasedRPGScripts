using System;
using Grid;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour {

    public static UnitActionSystem instance { get; private set; }
    
    public event Action<Unit> OnSelectedUnitChanged;
    public event Action OnSelectedActionChanged;
    public event Action<bool> OnBusyChanged;

    [SerializeField]
    private Unit selectedUnit;
    // TODO learn to implement this
    // public Unit SelectedUnit => selectedUnit;
    
    [SerializeField] 
    private LayerMask unitLayerMask;

    private BaseAction _selectedAction;

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
            // Sanity check
            return;
        }

        if (!EventSystem.current.IsPointerOverGameObject()) {
            HandleUnitSelection();
            HandleSelectedAction();    
        }
    }
    
    private void SetBusy() {
        _isBusy = true;
        OnBusyChanged?.Invoke(true);
    }
    
    private void ClearBusy() {
        _isBusy = false;
        OnBusyChanged?.Invoke(false);
    }

    private void HandleSelectedAction() {
        if (selectedUnit == null || _selectedAction == null) {
            // Sanity check
            return;
        }
        
        if (Input.GetMouseButtonDown(1)) {
            GridPosition mouseGridPosition = LevelGrid.instance.GetGridPosition(MouseWorld.GetPosition());

            _selectedAction.TakeAction(SetBusy, ClearBusy, mouseGridPosition);
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
        if (selectedUnit == unit) {
            // Sanity , unit is already the selected unit
            return;
        }
        
        selectedUnit?.DeSelect();
        unit.Select();
        
        selectedUnit = unit;
        SetSelectedAction(unit.GetMoveAction());
        
        // Added this back in later to communicate to classes that arent
        // children of Unit.
        OnSelectedUnitChanged?.Invoke(unit);
        
        
        
        // CodeMonkey did it this way with a singleton pattern, my way has the unit as the abstraction between 
        // the action system and the indicator.
        // OnSelectedUnitChanged?.Invoke(selectedUnit);
        // selectedUnit = unit;
    }
    
    public void SetSelectedAction(BaseAction action) {
        _selectedAction = action;
        OnSelectedActionChanged?.Invoke();
    }
    
    public BaseAction GetSelectedAction => _selectedAction;

    public Unit GetSelectedUnit => selectedUnit;

    
}
