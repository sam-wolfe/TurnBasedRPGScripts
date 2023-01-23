using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;

public class GridSystem {

    private int width;
    private int height;
    private float cellSize;
    private GridCell[,] gridCellArray;
    
    public GridSystem(int width, int height, float cellSize) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridCellArray = new GridCell[width, height];

        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                GridPosition gridPosition = new GridPosition(x, z);
                gridCellArray[x, z] = new GridCell(this, gridPosition);
                // Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z) + Vector3.right * .2f, Color.white, 1000 );
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition) {
        return new Vector3(gridPosition.x, 0f, gridPosition.z) * cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPos) {

        return new GridPosition(
            Mathf.RoundToInt(worldPos.x / cellSize),
            Mathf.RoundToInt(worldPos.z / cellSize)
        );
    }

    public void CreateDebugObjects(Transform debugPrefab) {
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform debugGridIndex =
                    GameObject.Instantiate(
                        debugPrefab,
                        GetWorldPosition(gridPosition),
                        Quaternion.identity
                    );
                GridIndexVisual gVis = 
                    debugGridIndex.GetComponent<GridIndexVisual>();
                gVis.SetGridCell(GetGridCell(gridPosition));
            }
        }
    }

    public GridCell GetGridCell(GridPosition gridPosition) {
        return gridCellArray[gridPosition.x, gridPosition.z];
    }
    
}
