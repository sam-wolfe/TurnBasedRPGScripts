using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;

public class LevelGrid : MonoBehaviour {
    
    public static LevelGrid instance { get; private set; }
    
    [SerializeField] private Transform _gridObjectPrefab;
    private GridSystem _gridSystem;

    private void Awake() {
        if (instance != null) {
            Debug.LogError("You added an extra LevelGrid ya dingus: " + transform + " - " + instance);
            Destroy(gameObject);
            return;
        }
        instance = this;
        
        _gridSystem = new GridSystem(10, 10, 2f);
        _gridSystem.CreateDebugObjects(_gridObjectPrefab);
    }

    private void Start() {
    }

    public void SetUnitAtGridPosition(GridPosition gridPosition, Unit unit) {
        GridCell cell = _gridSystem.GetGridCell(gridPosition);
        cell.SetUnit(unit);
    }

    public Unit GetUnitAtGridPosition(GridPosition gridPosition) {
        GridCell cell = _gridSystem.GetGridCell(gridPosition);
        return cell.unit;
    }

    public void ClearUnitAtGridPosition(GridPosition gridPosition) {
        GridCell cell = _gridSystem.GetGridCell(gridPosition);
        cell.ClearUnit();
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition from, GridPosition to) {
        ClearUnitAtGridPosition(from);
        SetUnitAtGridPosition(to, unit);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => _gridSystem.GetGridPosition(worldPosition);

}
