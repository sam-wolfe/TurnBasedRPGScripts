using System;
using Grid;
using UnityEngine;

public class Unit : MonoBehaviour {

    
    private GridPosition _gridPosition;
    private MoveAction _moveAction;
    private SpinAction _spinAction;
    
    // --------------------------------------------------------------------
    // NOTE: my better way of doing this
    //    * Unit has its own event that it envokes when it is selected
    //    * Unit anything can cause a unit to be selected, so no coupling
    //    * Indicator doesn't care what selects the unit, just if a unit is selected
    
    
    public event Action OnUnitSelected;
    public event Action OnUnitDeSelected;

    private void Awake() {
        _moveAction = GetComponent<MoveAction>();
        _spinAction = GetComponent<SpinAction>();
    }

    public void Select() {
        OnUnitSelected?.Invoke();
    }
    
    public void DeSelect() {
        OnUnitDeSelected?.Invoke();
    }

    public MoveAction GetMoveAction() {
        return _moveAction;
    }
    
    public SpinAction GetSpinAction() {
        return _spinAction;
    }

    public GridPosition GetGridPosition() {
        return _gridPosition;
    }

    void Start() {
        _gridPosition = LevelGrid.instance.GetGridPosition(transform.position);
        LevelGrid.instance.AddUnitAtGridPosition(_gridPosition, this);
    }

    void Update() {
        
        GridPosition newGridPosition = LevelGrid.instance.GetGridPosition(transform.position);
        if (newGridPosition != _gridPosition) {
            LevelGrid.instance.UnitMovedGridPosition(this, _gridPosition, newGridPosition);
            _gridPosition = newGridPosition;
        }
    }
    
    


}
