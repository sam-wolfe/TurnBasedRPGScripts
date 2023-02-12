using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour {

    public static TurnSystem instance { get; private set; }
    
    public event Action OnTurnChanged;
    
    private int turnNumber = 1;
    
    private void Awake() {
        if (instance != null) {
            Debug.LogError("You added an extra TurnSystem ya dingus: " + transform + " - " + instance);
            Destroy(gameObject);
            return;
        }
        instance = this;
        Debug.Log(this);
    }
    

    public void NextTurn() {
        turnNumber++;
        OnTurnChanged?.Invoke();
    }
    
    public int GetTurnNumber() {
        return turnNumber;
    }
}
