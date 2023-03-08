using UnityEngine;

public class TestGrid : MonoBehaviour {

    // [SerializeField] private Transform _gridDebugObjectPrefab;
    // private GridSystem _gridSystem;
    [SerializeField] private Unit unit;
    [SerializeField] private GridSystemVisual _gridSystemVisual;
    

    private void Update() {
        // Debug.Log(_gridSystem.GetGridPosition(MouseWorld.GetPosition()));
        if (Input.GetKeyDown(KeyCode.T)) {
            var gp = unit.GetAction<MoveAction>().GetValidActionGridPositions();
            // _gridSystemVisual.ShowGridPositionList(gp);
        }
        if (Input.GetKeyDown(KeyCode.Y)) {
            _gridSystemVisual.HideAllGridPositions();
        }
    }

}
