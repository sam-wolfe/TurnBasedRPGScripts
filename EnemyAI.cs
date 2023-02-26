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
                    _state = State.Busy;
                    TakeAction(SetStateTakingTurn);
                }
                break;
            case State.Busy:
                
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SetStateTakingTurn() {
        timer = 0.5f;
        _state = State.TakingTurn;
    }

    private void HandleTurnChange() {
        if (!TurnSystem.instance.IsPlayerTurn()) {
            _state = State.TakingTurn;
            timer = 3;
        }
    }

    private void TakeAction(Action onEnemyActionComplete) {
        
    }
}
