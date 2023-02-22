using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    private enum State {
        WaitingForTurn,
        TakingTurn,
        Busy
    }
    private State _state;
    
    public float timer;

    private void Awake() {
        _state = State.WaitingForTurn;
    }

    private void OnEnable() {
        TurnSystem.instance.OnTurnChanged += HandleTurnChange;
    }
    
    private void OnDisable() {
        TurnSystem.instance.OnTurnChanged -= HandleTurnChange;
    }

    void Update() {

        if (TurnSystem.instance.IsPlayerTurn()) {
            
            return;
        }
        
        switch (_state) {
            case State.WaitingForTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0) {
                    _state = State.WaitingForTurn;
                    TurnSystem.instance.NextTurn();
                }
                break;
            case State.Busy:
                
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }


    }

    private void HandleTurnChange() {
        if (!TurnSystem.instance.IsPlayerTurn()) {
            _state = State.TakingTurn;
            timer = 3;
        }
    }
}
