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
    
    [SerializeField] private int actionPointCost = 1;
    [SerializeField] private int _maxRange = 4;

    [SerializeField] private float _aimingStateTime = 1f;
    [SerializeField] private float _shootingStateTime = 0.1f;
    [SerializeField] private float _cooloffStateTime = 0.5f;
    
    private Vector3 targetPosition;
    private State state;
    private float stateTimer;
    private Unit _targetUnit;
    private bool _canShootBullet = true;
    
    public event EventHandler<ShootEventArgs> OnShoot;
    
    public class ShootEventArgs : EventArgs {
        public Unit targetUnit;
        public Unit shooterUnit;
    }
    
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
                    _canShootBullet = false;
                    Shoot();
                }
                
                CheckNextStateTransition(State.Cooloff, _cooloffStateTime);
                break;
            case State.Cooloff:
                _canShootBullet = true;
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
        OnShoot?.Invoke(this, new ShootEventArgs() {
            targetUnit = _targetUnit, 
            shooterUnit = _unit
        });
        
        // DEV: damage is hardcoded for now
        int damage = 40;
        _targetUnit.Damage(damage);
    }

    public override void TakeAction(Action onActionStarted, Action onActionComplete, GridPosition targetPosition) {
        

        
        if (IsValidActionGridPosition(targetPosition)) {
            _targetUnit = LevelGrid.instance.GetUnitAtGridPosition(targetPosition);
            
            if (_targetUnit == null) {
                throw new Exception("Valid position returned no unit! This should not happen!");
            }
            
            state = State.Aiming;
            stateTimer = _aimingStateTime;
            onActionStarted();
            InitiateAction(onActionComplete);
        }
    }

    public override List<GridPosition> GetValidActionGridPositions() {
        GridPosition unitGridPosition = _unit.GetGridPosition();
        return GetValidActionGridPositions(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositions(GridPosition unitGridPosition) {
        List<GridPosition> validGridPositions = new List<GridPosition>();

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

                if (targetUnit.IsEnemy() == _unit.IsEnemy()) {
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
    
    public Unit GetTargetUnit() {
        return _targetUnit;
    }
    
    public Unit GetShooterUnit() {
        return _unit;
    }
    
    public int GetRange() {
        return _maxRange;
    }
    
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) {
        return new EnemyAIAction {
            gridPosition = gridPosition,
            actionValue = 100,
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition) {
        return GetValidActionGridPositions(gridPosition).Count;
    }
}
