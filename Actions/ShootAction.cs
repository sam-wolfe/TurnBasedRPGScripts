using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;

public class ShootAction : BaseAction {

    private enum State {
        Aiming,
        Shooting,
        Cooloff
    }
    
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private int actionPointCost = 1;
    [SerializeField] private int _maxRange = 4;

    [SerializeField] private float _aimingStateTime = 1f;
    [SerializeField] private float _shootingStateTime = 0.1f;
    [SerializeField] private float _cooloffStateTime = 0.5f;
    
    private Vector3 targetPosition;
    private State state;
    private float stateTimer;
    private Unit _targetUnit;
    private bool _canShootBullet;
    
    public event Action OnShoot;
    
    protected override string _name
    {
        get { return "Shoot"; }
        set { }
    }

    private void Start() {
        _unit = GetComponent<Unit>();
    }

    void Update() {
        ProcessShoot();
    }

    private void ProcessShoot() {
        if (!_isActive) {
            return;
        }

        stateTimer -= Time.deltaTime;
        
        switch (state) {
            case State.Aiming:
                float rotateSpeed = 10f;
                Vector3 shootDirection = (_targetUnit.transform.position - transform.position).normalized;
                transform.forward = Vector3.Lerp(transform.forward, shootDirection, Time.deltaTime * rotateSpeed);
                
                CheckNextStateTransition(State.Shooting, _shootingStateTime);
                break;
            case State.Shooting:
                if (_canShootBullet) {
                    Shoot();
                }
                
                Shoot();
                OnShoot?.Invoke();
                CheckNextStateTransition(State.Cooloff, _cooloffStateTime);
                break;
            case State.Cooloff:
                CheckNextStateTransition(State.Aiming, 0f);
                break;
        }
    }

    private void CheckNextStateTransition(State newState, float newStateTimer) {
        if (state == State.Cooloff && stateTimer <= 0f) {
            _isActive = false;
            CompleteAction();
        } else if (stateTimer <= 0f) {
            state = newState;
            stateTimer = newStateTimer;
        }
    }
    
    private void Shoot() {
        _targetUnit.Damage();
    }

    public override void TakeAction(Action onActionStarted, Action onActionComplete, GridPosition targetPosition) {
        
        if (IsValidActionGridPosition(targetPosition)) {
            InitiateAction(onActionComplete);
            
            state = State.Aiming;
            stateTimer = _aimingStateTime;
            onActionStarted();
        }
    }
    
    public override List<GridPosition> GetValidActionGridPositions() {
        List<GridPosition> validGridPositions = new List<GridPosition>();
        GridPosition unitGridPosition = _unit.GetGridPosition();

        for (int x = -_maxRange; x <= _maxRange; x++) {
            for (int z = -_maxRange; z <= _maxRange; z++) {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (!LevelGrid.instance.IsValidGridPosition(testGridPosition)) {
                    // Check if new position is outside bounds of grid
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > _maxRange) {
                    // Test if new position is outside of max range
                    continue;
                }
                
                if (testGridPosition == unitGridPosition) {
                    // Test if this is the current position of unit
                    continue;
                }
                if (!LevelGrid.instance.IsOccupied(testGridPosition)) {
                    // Test if any other unit is already in target position
                    continue;
                }
                
                Unit targetUnit = LevelGrid.instance.GetUnitAtGridPosition(testGridPosition);

                if (targetUnit != null) {
                    _targetUnit = targetUnit;
                } else {
                    continue;
                }

                if (_targetUnit.IsEnemy() == _unit.IsEnemy()) {
                    // Both units are on the same team
                    continue;
                }
                
                validGridPositions.Add(testGridPosition);
            }
        }
        
        return validGridPositions;
    }
    
    public override int GetActionPointsCost() {
        return actionPointCost;
    }

}
