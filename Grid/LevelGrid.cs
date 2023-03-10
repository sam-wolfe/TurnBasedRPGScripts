using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;

public class LevelGrid : MonoBehaviour {
    
    public static LevelGrid instance { get; private set; }
    
    [SerializeField] private Transform _gridObjectPrefab;
    private GridSystem<GridCell> _gridSystem;

    public event Action<Unit> OnUnitMovePosition;

    private void Awake() {
        if (instance != null) {
            Debug.LogError("You added an extra LevelGrid ya dingus: " + transform + " - " + instance);
            Destroy(gameObject);
            return;
        }
        instance = this;
        
        _gridSystem = new GridSystem<GridCell>(10, 10, 2f, 
            (GridSystem<GridCell> g, GridPosition gridPosition) => new GridCell(g, gridPosition));
        _gridSystem.CreateDebugObjects(_gridObjectPrefab);
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit) {
        GridCell cell = _gridSystem.GetGridCell(gridPosition);
        cell.AddUnit(unit);
    }

    public List<Unit> GetUnitsAtGridPosition(GridPosition gridPosition) {
        GridCell cell = _gridSystem.GetGridCell(gridPosition);
        return cell._unitList;
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit) {
        GridCell cell = _gridSystem.GetGridCell(gridPosition);
        cell.RemoveUnit(unit);
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition from, GridPosition to) {
        RemoveUnitAtGridPosition(from, unit);
        AddUnitAtGridPosition(to, unit);
        OnUnitMovePosition?.Invoke(unit);
    }

    public bool IsOccupied(GridPosition gridPosition) {
        // Returns true if the cell is already occupied by a unit
        GridCell cell = _gridSystem.GetGridCell(gridPosition);
        return cell.HasAnyUnit();
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => _gridSystem.GetGridPosition(worldPosition);
    public Vector3 GetWorldPosition(GridPosition gridPosition) => _gridSystem.GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => _gridSystem.IsValidGridPosition(gridPosition);
    public int GetWidth() => _gridSystem.width;
    public int GetHeight() => _gridSystem.height;
    
    public Unit GetUnitAtGridPosition(GridPosition gridPosition) {
        GridCell cell = _gridSystem.GetGridCell(gridPosition);
        return cell.GetFirstUnit();
    }

}
