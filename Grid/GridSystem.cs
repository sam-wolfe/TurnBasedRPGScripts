using Grid;
using System;
using UnityEngine;

public class GridSystem<TGridCell> {

    public int width {  get; private set; }
    public int height {  get; private set; }
    private float cellSize;
    private TGridCell[,] gridCellArray;
    
    public GridSystem(int width, int height, float cellSize, Func<GridSystem<TGridCell>, GridPosition, TGridCell> createGridCell) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridCellArray = new TGridCell[width, height];

        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                GridPosition gridPosition = new GridPosition(x, z);
                gridCellArray[x, z] = createGridCell(this, gridPosition);
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
        GameObject gridParent = GameObject.FindWithTag("GridParent");
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform debugGridIndex =
                    GameObject.Instantiate(
                        debugPrefab,
                        GetWorldPosition(gridPosition),
                        Quaternion.identity
                    );
                debugGridIndex.parent = gridParent.transform;
                GridIndexVisual gVis = 
                    debugGridIndex.GetComponent<GridIndexVisual>();
                gVis.SetGridCell(GetGridCell(gridPosition) as GridCell);
            }
        }
    }

    public TGridCell GetGridCell(GridPosition gridPosition) {
        return gridCellArray[gridPosition.x, gridPosition.z];
    }

    public bool IsValidGridPosition(GridPosition gridPosition) {
        return gridPosition.x >= 0 && gridPosition.z >= 0 && gridPosition.x < width && gridPosition.z < height;
    }
    
}
