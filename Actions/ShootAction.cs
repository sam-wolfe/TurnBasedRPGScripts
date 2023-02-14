using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;

public class ShootAction : BaseAction {
    
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private int actionPointCost = 1;
    [SerializeField] private int _maxRange = 4;
    private Vector3 targetPosition;
    private Unit _unit;
    private Action onShootComplete;
    
    protected override string _name
    {
        get { return "Shoot"; }
        set { }
    }

    private void Start() {
        _unit = GetComponent<Unit>();
    }

    void Update() {
        if (_isActive) {
            Debug.Log("Bang Bang!");
            EndShoot();
        }
    }

    private void EndShoot() {
        _isActive = false;
        onActionComplete();
    }

    public override void TakeAction(Action onActionStarted, Action onActionComplete, GridPosition targetPosition) {
        _isActive = true;
        this.onActionComplete = onActionComplete;
        
        // This action can't fail to run
        onActionStarted();
    }
    
    public override List<GridPosition> GetValidActionGridPositions() {
        List<GridPosition> validGridPositions = new List<GridPosition>();
        GridPosition unitGridPosition = _unit.GetGridPosition();

        for (int x = -_maxRange; x <= _maxRange; x++) {
            for (int z = -_maxRange; z <= _maxRange; z++) {
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
                if (!LevelGrid.instance.IsOccupied(testGridPosition)) {
                    // Test if any other unit is already in target position
                    continue;
                }
                
                Unit targetUnit = LevelGrid.instance.GetUnitAtGridPosition(testGridPosition);

                // This assumes that we checked above that the position is occupied
                // If we didn't check that, we could get a null reference exception here
                if (targetUnit.IsEnemy() == _unit.IsEnemy()) {
                    // Both units are on the same team
                    continue;
                }
                
                validGridPositions.Add(testGridPosition);
            }
        }
        
        return validGridPositions;
    }

}
