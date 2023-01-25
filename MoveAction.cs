using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;
    private float stoppingDistance = 0.1f;
    private Vector3 targetPosition;
    private Unit _unit;
    
    // For dev
    [SerializeField] private int maxMoveDistance = 4;
    
    private void Awake() {
        targetPosition = transform.position;
        _unit = GetComponent<Unit>();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance) {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = 4f;

            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
            
            unitAnimator.SetBool("IsWalking", true);
        }
        else {
            unitAnimator.SetBool("IsWalking", false);
        }
    }
    
    public void Move(GridPosition targetPosition) {
        this.targetPosition = LevelGrid.instance.GetWorldPosition(targetPosition);
    }

    public bool IsValidActionGridPosition(GridPosition gridPosition) {
        List<GridPosition> validGridPositionList = GetValidActionGridPositions();
        return validGridPositionList.Contains(gridPosition);
    }

    public List<GridPosition> GetValidActionGridPositions() {
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
                Debug.Log(testGridPosition);
            }
        }
        
        return validGridPositions;
    }
}
