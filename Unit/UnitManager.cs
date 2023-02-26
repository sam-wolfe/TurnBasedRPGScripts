using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {
    
    public static UnitManager instance { get; private set; }

    private List<Unit> _units = new List<Unit>();
    private List<Unit> _friendlyUnits = new List<Unit>();
    private List<Unit> _enemyUnits = new List<Unit>();

    private void Start() {
        Unit.AnyUnitSpawned += HandleAnyUnitSpawned;
        Unit.AnyUnitDead += HandleAnyUnitDead;
    }

    private void Awake() {
        _units = new List<Unit>();
        _friendlyUnits = new List<Unit>();
        _enemyUnits = new List<Unit>();
        
        if (instance != null) {
            Debug.LogError("You added an extra UnitManager ya dingus: " + transform + " - " + instance);
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void OnDisable() {
        Unit.AnyUnitSpawned -= HandleAnyUnitSpawned;
        Unit.AnyUnitDead -= HandleAnyUnitDead;
    }

    private void HandleAnyUnitSpawned(Unit unit) {
        _units.Add(unit);
        if (unit.IsEnemy()) {
            _enemyUnits.Add(unit);
        }
        else {
            _friendlyUnits.Add(unit);
        }
    }
    
    private void HandleAnyUnitDead(Unit unit) {
        _units.Remove(unit);
        if (unit.IsEnemy()) {
            _enemyUnits.Remove(unit);
        }
        else {
            _friendlyUnits.Remove(unit);
        }
    }
    
    public List<Unit> GetUnits() {
        return _units;
    }
    
    public List<Unit> GetFriendlyUnits() {
        return _friendlyUnits;
    }
    
    public List<Unit> GetEnemyUnits() {
        return _enemyUnits;
    }

}
