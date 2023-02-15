using System.Collections.Generic;
using System.Linq;
using Grid;

public class GridCell {

    private GridSystem _gridSystem;
    private GridPosition _gridPosition;
    public List<Unit> _unitList { get; private set; }


    public GridCell(GridSystem gridSystem, GridPosition gridPosition) {
        _gridPosition = gridPosition;
        _gridSystem = gridSystem;
        _unitList = new List<Unit>();
    }

    public string GetPositionString() {
        string unitString = "";
        foreach (var u in _unitList) {
            unitString = $"{unitString}\n{u}";
        }
        
        return $"{this}{unitString}";
    }

    public override string ToString() {
        return $"({_gridPosition.x},{_gridPosition.z})";
    } 

    public void AddUnit(Unit unit) {
        _unitList.Add(unit);
    }

    public bool RemoveUnit(Unit unit) {
        return _unitList.Remove(unit);
    }
    
    public List<Unit> GetUnits() {
        return _unitList;
    }
    
    public bool ClearUnits() {
        if (_unitList.Count == 0) {
            return false;
        }
        else {
            _unitList.Clear();
            return true;
        }
    }

    public bool HasAnyUnit() {
        return _unitList.Any();
    }
    
    public Unit GetFirstUnit() {
        // This is written assuming that there is only one unit per cell 
        // at the time an action is taken.
        // If in the future we want to allow multiple units per cell,
        // we will need to change this.

        if (HasAnyUnit()) {
            return _unitList[0];
        } else {
            return null;
        }
    }
}
