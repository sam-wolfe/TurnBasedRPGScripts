using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell {

    private GridSystem _gridSystem;
    private GridPosition _gridPosition;

    public GridCell(GridSystem gridSystem, GridPosition gridPosition) {
        _gridPosition = gridPosition;
        _gridSystem = gridSystem;
    }

    public string GetPositionString() {
        return $"({_gridPosition.x},{_gridPosition.z})";
    }
}
