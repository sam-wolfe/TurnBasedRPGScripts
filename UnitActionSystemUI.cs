using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour {
    
    [SerializeField] private Transform _actionButtonPrefab;
    [SerializeField] private Transform _actionButtonContainer;

    private void Start() {
    }

    private void OnEnable() {
        UnitActionSystem.instance.OnSelectedUnitChanged += HandleSelectedUnitChanged;
    }
    
    private void OnDisable() {
        UnitActionSystem.instance.OnSelectedUnitChanged -= HandleSelectedUnitChanged;
    }
    
    private void HandleSelectedUnitChanged(Unit unit) {
        ClearActionUnitButtons();
        CreateActionUnitButtons(unit);
    }

    private void CreateActionUnitButtons(Unit unit) {
        foreach (BaseAction action in unit.GetBaseActions()) {
            Transform buttonTransform = Instantiate(_actionButtonPrefab, _actionButtonContainer);
            ActionButtonUI button = buttonTransform.GetComponent<ActionButtonUI>();
            button.SetBaseAction(action);
        }
    }
    
    private void ClearActionUnitButtons() {
        foreach (Transform child in _actionButtonContainer) {
            Destroy(child.gameObject);
        }
    }

}
