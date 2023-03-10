using System;
using Grid;
using UnityEngine;

public class Unit : MonoBehaviour {

    
    private GridPosition _gridPosition;
    private BaseAction[] _baseActions;
    private HealthSystem _healthSystem;
    [SerializeField] private int _actionPoints;
    [SerializeField] private int _maxActionPoints;

    [SerializeField] private bool _isEnemy;
    
    // --------------------------------------------------------------------
    // NOTE: my better way of doing this
    //    * Unit has its own event that it envokes when it is selected
    //    * Unit anything can cause a unit to be selected, so no coupling
    //    * Indicator doesn't care what selects the unit, just if a unit is selected

    public event Action OnUnitSelected;
    public event Action OnUnitDeSelected;
    public static event Action OnAnyActionPointsChanged;
    public static event Action<Unit> AnyUnitSpawned;
    public static event Action<Unit> AnyUnitDead;
    

    private void Awake() {
        _baseActions = GetComponents<BaseAction>();
        _healthSystem = GetComponent<HealthSystem>();
        
        _actionPoints = _maxActionPoints;
    }
    
    void Start() {
        _gridPosition = LevelGrid.instance.GetGridPosition(transform.position);
        LevelGrid.instance.AddUnitAtGridPosition(_gridPosition, this);
        
        TurnSystem.instance.OnTurnChanged += HandleTurnChanged;
        
        _healthSystem.OnDeath += HandleDeath;
        
        AnyUnitSpawned?.Invoke(this);
    }

    void Update() {
        
        GridPosition newGridPosition = LevelGrid.instance.GetGridPosition(transform.position);
        if (newGridPosition != _gridPosition) {
            
            var oldGridPosition = _gridPosition;
            _gridPosition = newGridPosition;
            LevelGrid.instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);

        }
    }

    public void Select() {
        OnUnitSelected?.Invoke();
    }
    
    public void DeSelect() {
        OnUnitDeSelected?.Invoke();
    }
    
    public T GetAction<T>() where T : BaseAction {
        foreach (var action in _baseActions) {
            if (action is T) {
                return (T) action;
            }
        }

        return null;
    }

    public GridPosition GetGridPosition() {
        return _gridPosition;
    }
    public GridPosition GetGridPositionDev() {
        var fakeGrid = _gridPosition;
        fakeGrid.x += 1;
        return fakeGrid;
    }

    public Vector3 GetWorldPosition() {
        return LevelGrid.instance.GetWorldPosition(_gridPosition);
    }

    public BaseAction[] GetBaseActions() {
        return _baseActions;
    }
    
    public bool HasActionPoints() {
        return _actionPoints > 0;
    }
    
    public int GetActionPoints() {
        return _actionPoints;
    }

    
    public bool HasActionPointsForAction(BaseAction action) {
        if (_actionPoints >= action.GetActionPointsCost()) {
            return true;
        }

        return false;
    }
    
    private void SpendActionPoints(int amount) {
        _actionPoints -= amount;
    }

    public bool TrySpendActionPoints(BaseAction action) {
        if (HasActionPointsForAction(action)) {
            SpendActionPoints(action.GetActionPointsCost());
            OnAnyActionPointsChanged?.Invoke();
            return true;
        }

        return false;
    }
    
    private void HandleTurnChanged() {
        if ((IsEnemy() && !TurnSystem.instance.IsPlayerTurn()) 
            || (!IsEnemy() && TurnSystem.instance.IsPlayerTurn())) {
            _actionPoints = _maxActionPoints;
            OnAnyActionPointsChanged?.Invoke();
        }
    }
    
    public bool IsEnemy() {
        return _isEnemy;
    }

    public void Damage(int damageAmount) {
        Debug.Log(transform + " was damaged!");
        _healthSystem.Damage(damageAmount);
    }
    
    private void HandleDeath(Unit unit) {
        Debug.Log(transform + " died!");
        LevelGrid.instance.RemoveUnitAtGridPosition(_gridPosition, this);
        Destroy(gameObject);
        AnyUnitDead?.Invoke(this);
    }

}
