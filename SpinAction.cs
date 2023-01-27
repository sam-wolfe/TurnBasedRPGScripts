using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : MonoBehaviour {

    private bool startSpinning;

    void Update() {
        if (startSpinning) {
            float addSpinAmount = 360f * Time.deltaTime;
            transform.eulerAngles += new Vector3(0, addSpinAmount,0);
        }
    }

    public void Spin() {
        startSpinning = true;
    }
}
