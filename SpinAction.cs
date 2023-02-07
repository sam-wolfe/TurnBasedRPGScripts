using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : MonoBehaviour {

    private bool startSpinning;
    [SerializeField] private float spinAmount = 360;
    private Vector3 originalRotation;
    private float totalSpin;

    void Update() {
        if (startSpinning) {
            totalSpin += ProcessSpin();
            if (totalSpin >= spinAmount) {
                // we are done spinning
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
        startSpinning = false;
        transform.eulerAngles = originalRotation;
        totalSpin = 0;
    }

    public void Spin() {
        startSpinning = true;
        originalRotation = transform.eulerAngles;
    }
}
