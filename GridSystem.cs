using System.Collections;
using System.Collections.Generic;
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

        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                GridPosition gridPosition = new GridPosition(x, z);

                // Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z) + Vector3.right * .2f, Color.white, 1000 );
            }
        }
    }

    public Vector3 GetWorldPosition(int x, int z) {
        return new Vector3(x, 0f, z) * cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPos) {

        return new GridPosition(
            Mathf.RoundToInt(worldPos.x / cellSize),
            Mathf.RoundToInt(worldPos.z / cellSize)
        );
    }
    
    
    
}
