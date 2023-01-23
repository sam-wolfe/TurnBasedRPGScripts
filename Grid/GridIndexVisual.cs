using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridIndexVisual : MonoBehaviour {

    private GridCell _gridCell;
    private TextMeshPro _text;

    private void Awake() {
        // TODO should have just serlized the field and dragged it
        //  in, in the inspector
        _text = GetComponentInChildren<TextMeshPro>();
    }

    public void SetGridCell(GridCell gridCell) {
        _gridCell = gridCell;
        _text.text = _gridCell.GetPositionString();
    }

    private void Update() {
        _text.text = _gridCell.GetPositionString();
    }

}
