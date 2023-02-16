using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;

public class MoveAction : BaseAction
{
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private int actionPointCost = 1;
    private float stoppingDistance = 0.1f;
    private Vector3 targetPosition;

    // For dev
    [SerializeField] private int maxMoveDistance = 4;

    public event Action OnMoveStart;
    public event Action OnMoveStop;
    
    protected override string _name
    {
        get { return "Move"; }
        set { }
    }
    
    protected override void Awake() {
        base.Awake();
        targetPosition = transform.position;
    }

    void Update() {
        ProcessMove();
    }

    private void ProcessMove() {
        if (_isActive) {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;

            if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance) {
                float moveSpeed = 4f;

                transform.position += moveDirection * moveSpeed * Time.deltaTime;

            }
            else {
                CompleteAction();
                OnMoveStop?.Invoke();
            }
        
            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
        }
    }
    
    public override void TakeAction(Action onActionStarted, Action onActionComplete, GridPosition targetPosition) {
        
        if (IsValidActionGridPosition(targetPosition)) {
            this.targetPosition = LevelGrid.instance.GetWorldPosition(targetPosition);
            InitiateAction(onActionComplete);
            onActionStarted();
            OnMoveStart?.Invoke();
        }
    }

    public override List<GridPosition> GetValidActionGridPositions() {
        List<GridPosition> validGridPositions = new List<GridPosition>();
        GridPosition unitGridPosition = _unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++) {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++) {
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
    
    public override int GetActionPointsCost() {
        return actionPointCost;
    }
}
