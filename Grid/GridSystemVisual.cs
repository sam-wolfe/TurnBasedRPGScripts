using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour {
    
    [SerializeField] private Transform visualPrefab;
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterials;
    
    public static GridSystemVisual instance { get; private set; }
    private GridVisualSingle[,] _gridVisualSingles;

    public enum GridVisualColour {
        White,
        Red,
        Green,
        Blue,
        Yellow,
        Orange,
        Purple,
        RedTransparent,
    }
    
    [Serializable]
    public struct GridVisualTypeMaterial {
        public GridVisualColour colour;
        public Material material;
    }
    
    

    private void Awake() {
        if (instance != null) {
            Debug.LogError("You added an extra GridSystemVisual ya dingus: " + transform + " - " + instance);
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    
    void Start() {
        var width = LevelGrid.instance.GetWidth();
        var height = LevelGrid.instance.GetHeight();

        _gridVisualSingles = new GridVisualSingle[width, height];
        
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                GridPosition gp = new GridPosition(x, z);
                Transform gsv = Instantiate(visualPrefab, LevelGrid.instance.GetWorldPosition(gp), Quaternion.identity);
                gsv.transform.parent = transform;
                _gridVisualSingles[x, z] = gsv.GetComponent<GridVisualSingle>();
            }
        }
        HideAllGridPositions();
        UpdateGridVisual();
    }

    private void OnEnable() {
        UnitActionSystem.instance.OnSelectedActionChanged += HandleSelectedActionChanged;
        LevelGrid.instance.OnUnitMovePosition += HandleUnitMovePosition;
    }
    
    private void OnDisable() {
        UnitActionSystem.instance.OnSelectedActionChanged -= HandleSelectedActionChanged;
        LevelGrid.instance.OnUnitMovePosition -= HandleUnitMovePosition;
    }
    
    private void HandleSelectedActionChanged() {
        UpdateGridVisual();
    }
    
    private void HandleUnitMovePosition(Unit unit) {
        UpdateGridVisual();
    }

    private void Update() {
        // UpdateGridVisual();
    }

    private void UpdateGridVisual() {
        
        Unit selectedUnit = UnitActionSystem.instance.GetSelectedUnit;
        BaseAction selectedAction = UnitActionSystem.instance.GetSelectedAction;
        
        if (selectedAction != null) {
            var gp = selectedAction.GetValidActionGridPositions();
            HideAllGridPositions();
            
            GridVisualColour colour;

            switch (selectedAction) {
                default:
                case MoveAction moveAction:
                    colour = GridVisualColour.White;
                    break;
                case SpinAction spinAction:
                    colour = GridVisualColour.Blue;
                    break;
                case ShootAction shootAction:
                    colour = GridVisualColour.Red;
                    ShowGridPositionRange(
                        selectedUnit.GetGridPosition(), 
                        shootAction.GetRange(),
                        GridVisualColour.RedTransparent
                    );
                    break;
            }
            ShowGridPositionList(gp, colour);    
        }
    }

    public void HideAllGridPositions() {
        foreach (var gridVisual in _gridVisualSingles) {
            gridVisual.Hide();
        }
    }

    public void ShowGridPositionList(List<GridPosition> gridPositions, GridVisualColour colour) {
        foreach (var gridPosition in gridPositions) {
            _gridVisualSingles[gridPosition.x, gridPosition.z].Show(GetMaterialFromColour(colour));
        }
    }
    
    private void ShowGridPositionRange(GridPosition gp, int range, GridVisualColour colour) {
        List<GridPosition> validGridPositions = new List<GridPosition>();

        for (int x = -range; x <= range; x++) {
            for (int z = -range; z <= range; z++) {
                GridPosition gp2 = new GridPosition(x, z);
                
                GridPosition testGridPosition = gp + gp2;
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                
                if (testDistance > range) {
                    // Test if new position is outside of max range
                    continue;
                }
                
                if (!LevelGrid.instance.IsValidGridPosition(testGridPosition)) {
                    // Test if new position is outside of grid
                    continue;
                }
                
                validGridPositions.Add(testGridPosition);
            }
        }
        ShowGridPositionList(validGridPositions, colour);
    }
    
    private Material GetMaterialFromColour(GridVisualColour colour) {
        foreach (var gridVisualTypeMaterial in gridVisualTypeMaterials) {
            if (gridVisualTypeMaterial.colour == colour) {
                return gridVisualTypeMaterial.material;
            }
        }
        
        throw new Exception("No material found for colour: " + colour);
        return null;
    }
}
