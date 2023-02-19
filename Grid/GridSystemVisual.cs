using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour {
    
    [SerializeField] private Transform visualPrefab;
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterials;
    
    public static GridSystemVisual instance { get; private set; }
    private GridVisualSingle[,] _gridVisualSingles;

    public enum GridVisualColours {
        White,
        Red,
        Green,
        Blue,
        Yellow,
        Orange,
        Purple,
    }
    
    [Serializable]
    public struct GridVisualTypeMaterial {
        public GridVisualColours colour;
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
        
        BaseAction selectedAction = UnitActionSystem.instance.GetSelectedAction;
        
        if (selectedAction != null) {
            var gp = selectedAction.GetValidActionGridPositions();
            HideAllGridPositions();
            ShowGridPositionList(gp);    
        }
    }

    public void HideAllGridPositions() {
        foreach (var gridVisual in _gridVisualSingles) {
            gridVisual.Hide();
        }
    }

    public void ShowGridPositionList(List<GridPosition> gridPositions) {
        foreach (var gridPosition in gridPositions) {
            _gridVisualSingles[gridPosition.x, gridPosition.z].Show();
        }
    }
    
    private Material GetMaterialFromColour(GridVisualColours colour) {
        foreach (var gridVisualTypeMaterial in gridVisualTypeMaterials) {
            if (gridVisualTypeMaterial.colour == colour) {
                return gridVisualTypeMaterial.material;
            }
        }
        
        throw new Exception("No material found for colour: " + colour);
        return null;
    }
}
