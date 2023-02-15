using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;

public class SpinAction : BaseAction {

    [SerializeField] private int actionPointCost = 0;
    [SerializeField] private float _spinDuration = 1.5f;
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
        transform.forward = Vector3.Lerp(transform.forward, spinDirection, Time.deltaTime * rotateSpeed);
        
        // if transform.forward is close to spinDirection, then we can stop spinning
        float dotProduct = Vector3.Dot(transform.forward, spinDirection);
        if (dotProduct > Mathf.Abs(0.99f)) {
            CompleteAction();
        }
    }

    public override void TakeAction(Action onActionStarted, Action onActionComplete, GridPosition targetPosition) {
        InitiateAction(onActionComplete);
        
        _targetPosition = LevelGrid.instance.GetWorldPosition(targetPosition);

        // This action can't fail to run
        onActionStarted();
    }

    public override int GetActionPointsCost() {
        return actionPointCost;
    }
}
