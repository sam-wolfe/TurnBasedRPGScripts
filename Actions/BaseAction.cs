using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour {

    protected Unit _unit;
    protected bool _isActive;
    protected Action onActionComplete;
    protected static string _name = "IMPLEMENT_NAME";

    protected virtual void Awake() {
        _unit = GetComponent<Unit>();
    }
    
    public abstract void TakeAction(Action onActionStarted, Action onActionComplete, GridPosition targetPosition);
    
    public abstract string GetActionName();
    
    public abstract bool IsValidActionGridPosition(GridPosition gridPosition);

    public abstract List<GridPosition> GetValidActionGridPositions();

    public virtual int GetActionPointsCost() {
        return 1;
    }

}
