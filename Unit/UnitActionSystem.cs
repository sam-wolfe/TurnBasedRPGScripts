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
    private Unit _selectedUnit;
    
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
    }

    void Update() {
        if (_isBusy) {
            // Sanity check
            return;
        }

        if (!EventSystem.current.IsPointerOverGameObject() && TurnSystem.instance.IsPlayerTurn()) {
            HandleUnitSelection();
            HandleSelectedAction();    
        }
    }
    
    private void SetBusy() {
        _isBusy = true;
        bool pointsWereSpent = _selectedUnit.TrySpendActionPoints(_selectedAction);
        
        if (!pointsWereSpent) {
            throw new Exception("Unit tried to spend action points but failed! \n" +
                                "This should never happen, check the logic.");
        }
        
        OnBusyChanged?.Invoke(true);
    }
    
    private void ClearBusy() {
        if (_isBusy) {
            // Don't fire event if we're not busy
            _isBusy = false;
            OnBusyChanged?.Invoke(false);
        }
    }

    private void HandleSelectedAction() {
        if (_selectedUnit == null || _selectedAction == null) {
            // Sanity check
            return;
        }
        
        if (Input.GetMouseButtonDown(1)) {
            GridPosition mouseGridPosition = LevelGrid.instance.GetGridPosition(MouseWorld.GetPosition());

            // Check if unit has enough AP to perform action
            if (_selectedUnit.HasActionPointsForAction(_selectedAction)) {
                _selectedAction.TakeAction(SetBusy, ClearBusy, mouseGridPosition);
            }
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
        if (_selectedUnit == unit || unit.IsEnemy()) {
            // Sanity , unit is already the selected unit, or is an enemy
            return;
        }
        
        _selectedUnit?.DeSelect();
        unit.Select();
        
        _selectedUnit = unit;
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

    public Unit GetSelectedUnit => _selectedUnit;

    
}
