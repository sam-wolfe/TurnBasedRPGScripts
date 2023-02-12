using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour {
    
    [SerializeField] private Transform _actionButtonPrefab;
    [SerializeField] private Transform _actionButtonContainer;
    
    private List<ActionButtonUI> _actionButtons = new List<ActionButtonUI>();
    
    private void Start() {
    }

    private void OnEnable() {
        UnitActionSystem.instance.OnSelectedUnitChanged += HandleSelectedUnitChanged;
        UnitActionSystem.instance.OnSelectedActionChanged += HandleSelectedActionChanged;
    }
    
    private void OnDisable() {
        UnitActionSystem.instance.OnSelectedUnitChanged -= HandleSelectedUnitChanged;
        UnitActionSystem.instance.OnSelectedActionChanged -= HandleSelectedActionChanged;
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
            _actionButtons.Add(button);
            button.SetSelectedVisual(action == UnitActionSystem.instance.GetSelectedAction);
        }
    }
    
    private void ClearActionUnitButtons() {
        _actionButtons.Clear();
        foreach (Transform child in _actionButtonContainer) {
            Destroy(child.gameObject);
        }
    }

    private void HandleSelectedActionChanged() {
        foreach (ActionButtonUI button in _actionButtons) {
            button.SetSelectedVisual(button.action == UnitActionSystem.instance.GetSelectedAction);
        }
        
    }

}
