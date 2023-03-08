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

    private BaseAction _currentAction;
    private Unit _currentUnit;

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
                    if (!TryTakeAction(SetStateTakingTurn)) {
                        // No more actions to take, end turn
                        TurnSystem.instance.NextTurn();
                    }
                }
                break;
            case State.Busy:
                
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void SetBusy() {
        _state = State.Busy;
    }

    private void OnTakingAction() {
        SetBusy();
        
        bool pointsWereSpent = _currentUnit.TrySpendActionPoints(_currentAction);
        
        if (!pointsWereSpent) {
            throw new Exception("Unit tried to spend action points but failed! \n" +
                                "This should never happen, check the logic.");
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

    private bool TryTakeAction(Action onEnemyActionComplete) {
        foreach (Unit unit in UnitManager.instance.GetEnemyUnits()) {
            if(TryEnemyTakeAction(unit, onEnemyActionComplete)) {
                return true;
            }
        }

        return false;
    }
    
    private bool TryEnemyTakeAction(Unit unit, Action onEnemyActionComplete) {

        EnemyAIAction bestAction = null;
        BaseAction bestBaseAction = null;
        
        foreach (BaseAction possibleAction in unit.GetBaseActions()) {
            if (!unit.HasActionPointsForAction(possibleAction)) {
                // Cant afford this action, skip it
                continue;
            }
            
            if (bestAction == null) {
                // First iteration, set best action
                bestAction = possibleAction.GetBestEnemyAIAction();
                bestBaseAction = possibleAction;
                continue;
            }

            EnemyAIAction tempAction = possibleAction.GetBestEnemyAIAction();
            if (tempAction != null && tempAction.actionValue > bestAction.actionValue) {
                bestAction = tempAction;
                bestBaseAction = possibleAction;
            }
        }

        if (bestAction != null && unit.HasActionPointsForAction(bestBaseAction)) {

            _currentAction = bestBaseAction;
            _currentUnit = unit;
            
            Debug.Log("Action taken!");
            bestBaseAction.TakeAction(OnTakingAction, onEnemyActionComplete, bestAction.gridPosition);
            return true;
            
        }

        Debug.Log("Not enough points or no valid target position for actions, no action taken.");
        return false;

    }
}
