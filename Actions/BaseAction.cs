using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour {

    protected Unit _unit;
    protected bool _isActive;

    protected virtual void Awake() {
        _unit = GetComponent<Unit>();
    }

}