using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour {

    public static TurnSystem instance { get; private set; }
    
    public event Action OnTurnChanged;
    
    private int turnNumber = 1;
    
    private bool _isPlayerTurn = true;
    
    private void Awake() {
        if (instance != null) {
            Debug.LogError("You added an extra TurnSystem ya dingus: " + transform + " - " + instance);
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    

    public void NextTurn() {
        turnNumber++;
        _isPlayerTurn = !_isPlayerTurn;
        OnTurnChanged?.Invoke();
    }
    
    public int GetTurnNumber() {
        return turnNumber;
    }
    
    public bool IsPlayerTurn() {
        return _isPlayerTurn;
    }
}
