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

        // This action can't fail to run
        onActionStarted();
    }
    
    public override string GetActionName() {
        return _name;
    }
}
