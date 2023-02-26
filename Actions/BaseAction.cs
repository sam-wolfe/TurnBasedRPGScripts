using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour {

    protected Unit _unit;
    protected bool _isActive;
    protected Action onActionComplete;
    protected abstract string _name { get; set;}

    public static event Action<BaseAction> OnAnyActionStart;
    public static event Action<BaseAction> OnAnyActionComplete;

    protected virtual void Awake() {
        _unit = GetComponent<Unit>();
    }
    
    public abstract void TakeAction(Action onActionStarted, Action onActionComplete, GridPosition targetPosition);
    
    public virtual List<GridPosition> GetValidActionGridPositions() {
        List<GridPosition> validGridPositions = new List<GridPosition>();
        GridPosition unitGridPosition = _unit.GetGridPosition();

        // Hardcoded 1's as we only want to show adjacent positions
        for (int x = -1; x <= 1; x++) {
            for (int z = -1; z <= 1; z++) {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (!LevelGrid.instance.IsValidGridPosition(testGridPosition)) {
                    // Check if new position is outside bounds of grid
                    continue;
                }
                if (testGridPosition == unitGridPosition) {
                    // Test if this is the current position of unit
                    continue;
                }
                if (LevelGrid.instance.IsOccupied(testGridPosition)) {
                    // Test if any other unit is already in target position
                    continue;
                }
                validGridPositions.Add(testGridPosition);
            }
        }
        
        return validGridPositions;
    }
    
    public virtual bool IsValidActionGridPosition(GridPosition gridPosition) {
        List<GridPosition> validGridPositionList = GetValidActionGridPositions();
        return validGridPositionList.Contains(gridPosition);
    }

    public virtual int GetActionPointsCost() {
        return 1;
    }
    
    public virtual string GetActionName() {
        return _name;
    }

    protected void InitiateAction(Action onActionComplete) {
        _isActive = true;
        this.onActionComplete = onActionComplete;
        
        OnAnyActionStart?.Invoke(this);
    }
    
    protected void CompleteAction() {
        _isActive = false;
        onActionComplete();
        
        OnAnyActionComplete?.Invoke(this);
    }

    public EnemyAIAction GetBestEnemyAIAction() {
        // Method name is a bit misleading, method actually looks for the best
        // target position and returns an EnemyAIAction with that position
        
        List<EnemyAIAction> enemyAIActions = new List<EnemyAIAction>();
        
        List<GridPosition> validGridPositions = GetValidActionGridPositions();

        foreach (GridPosition gridPosition in validGridPositions) {
            EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);
            enemyAIActions.Add(enemyAIAction);
        }
        
        enemyAIActions.Sort((a, b) => b.actionValue - a.actionValue);
        
        
        if (enemyAIActions.Count != 0) {
            return enemyAIActions[0];
        } else {
            return null;
        }
    }
    
    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);

}
