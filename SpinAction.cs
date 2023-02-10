using System;
using System.Collections;
using System.Collections.Generic;
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

    public void Spin(Action onActionComplete) {
        _isActive = true;
        originalRotation = transform.eulerAngles;
        this.onActionComplete = onActionComplete;
    }
    
    public override string GetActionName() {
        return _name;
    }
}
