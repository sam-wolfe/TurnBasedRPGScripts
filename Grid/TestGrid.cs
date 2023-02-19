using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGrid : MonoBehaviour {

    // [SerializeField] private Transform _gridDebugObjectPrefab;
    // private GridSystem _gridSystem;
    [SerializeField] private Unit unit;
    [SerializeField] private GridSystemVisual _gridSystemVisual;
    
    void Start() {
        // _gridSystem = new GridSystem(10, 10, 2f);
        // _gridSystem.CreateDebugObjects(_gridDebugObjectPrefab);

        // Debug.Log(new GridPosition(5, 7));
    }

    private void Update() {
        // Debug.Log(_gridSystem.GetGridPosition(MouseWorld.GetPosition()));
        if (Input.GetKeyDown(KeyCode.T)) {
            var gp = unit.GetMoveAction().GetValidActionGridPositions();
            // _gridSystemVisual.ShowGridPositionList(gp);
        }
        if (Input.GetKeyDown(KeyCode.Y)) {
            _gridSystemVisual.HideAllGridPositions();
        }
    }

}
