using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour {
    
    public static GridSystemVisual instance { get; private set; }

    [SerializeField] private Transform visualPrefab;

    private GridVisualSingle[,] _gridVisualSingles;

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
    }

    private void Update() {
        UpdateGridVisual();
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
}
