using System;
using Grid;
using UnityEngine;

public class SpinAction : BaseAction {

    [SerializeField] private int actionPointCost = 0;
    private Vector3 _targetPosition;
    private float totalSpin;
    
    protected override string _name
    {
        get { return "Spin"; }
        set { }
    }

    void Update() {
        if (_isActive) {
            ProcessSpin();
        }
    }
    
    private void ProcessSpin() {
        float rotateSpeed = 10f;
        Vector3 spinDirection = (_targetPosition - transform.position).normalized;
        transform.forward = Vector3.Lerp(
            transform.forward, spinDirection, Time.deltaTime * rotateSpeed);
        
        // if transform.forward is close to spinDirection, then we can stop spinning
        float dotProduct = Vector3.Dot(transform.forward, spinDirection);
        if (dotProduct > Mathf.Abs(0.99f)) {
            CompleteAction();
        }
    }

    public override void TakeAction(Action onActionStarted, Action onActionComplete, GridPosition targetPosition) {
        _targetPosition = LevelGrid.instance.GetWorldPosition(targetPosition);

        // This action can't fail to run
        onActionStarted();
        InitiateAction(onActionComplete);
    }

    public override int GetActionPointsCost() {
        // Dev - remove this later
        return 1;
        return actionPointCost;
    }
    
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) {
        return new EnemyAIAction {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }
    
}
