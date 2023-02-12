using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;

public class SpinAction : BaseAction {

    [SerializeField] private float spinAmount = 360;
    private Vector3 originalRotation;
    private float totalSpin;
    protected static string _name = "Spin";

    void Update() {
        if (_isActive) {
            totalSpin += ProcessSpin();
            if (totalSpin >= spinAmount) {
                EndSpin();
            }
        }
    }
    
    private float ProcessSpin() {
        float addSpinAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, addSpinAmount,0);
        return addSpinAmount;
    }
    
    private void EndSpin() {
        _isActive = false;
        transform.eulerAngles = originalRotation;
        totalSpin = 0;
        onActionComplete();
    }

    public override void TakeAction(Action onActionStarted, Action onActionComplete, GridPosition targetPosition) {
        _isActive = true;
        originalRotation = transform.eulerAngles;
        this.onActionComplete = onActionComplete;
        
        // TODO where you left off
        // TODO use the target position to determine the direction to spin
        // TODO update validation to check if the target position is a valid direction to spin
        // TODO make sure onAactionStarted is only called if the action is valid

        // This action can't fail to run
        onActionStarted();
    }
    
    public override string GetActionName() {
        return _name;
    }
    
    public override bool IsValidActionGridPosition(GridPosition gridPosition) {
        List<GridPosition> validGridPositionList = GetValidActionGridPositions();
        return validGridPositionList.Contains(gridPosition);
    }
    
    public override List<GridPosition> GetValidActionGridPositions() {
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
}
