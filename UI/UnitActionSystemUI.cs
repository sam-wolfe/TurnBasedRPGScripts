using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour {
    
    [SerializeField] private Transform _actionButtonPrefab;
    [SerializeField] private Transform _actionButtonContainer;
    [SerializeField] private TextMeshProUGUI _actionPointsText;
    [SerializeField] private string _actionPointsTextFormat = "Action Points: {0}";
    
    private List<ActionButtonUI> _actionButtons = new List<ActionButtonUI>();
    
    
    private void Start() {
        UpdateActionPoints();
    }

    private void OnEnable() {
        UnitActionSystem.instance.OnSelectedUnitChanged += HandleSelectedUnitChanged;
        UnitActionSystem.instance.OnSelectedActionChanged += HandleSelectedActionChanged;
        UnitActionSystem.instance.OnBusyChanged += HandleBusyChanged;
        TurnSystem.instance.OnTurnChanged += HandleTurnChanged;
        Unit.OnAnyActionPointsChanged += HandleAnyActionPointsChanged;
    }
    
    private void OnDisable() {
        UnitActionSystem.instance.OnSelectedUnitChanged -= HandleSelectedUnitChanged;
        UnitActionSystem.instance.OnSelectedActionChanged -= HandleSelectedActionChanged;
        UnitActionSystem.instance.OnBusyChanged -= HandleBusyChanged;
        TurnSystem.instance.OnTurnChanged -= HandleTurnChanged;
        Unit.OnAnyActionPointsChanged -= HandleAnyActionPointsChanged;
    }
    
    private void HandleSelectedUnitChanged(Unit unit) {
        ClearActionUnitButtons();
        CreateActionUnitButtons(unit);
        UpdateActionPoints();
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

    private void HandleBusyChanged(bool busy) {
        foreach (ActionButtonUI button in _actionButtons) {
            button.SetDisabled(busy);
            if (!busy) {
                // Re-select the action if it was selected before
                button.SetSelectedVisual(button.action == UnitActionSystem.instance.GetSelectedAction);
            } else {
                // An action was performed, update the UI
                UpdateActionPoints();
            }
        }
    }

    private void UpdateActionPoints() {
        if (UnitActionSystem.instance.GetSelectedUnit == null) {
            // No unit selected
            _actionPointsText.text = "";
        }
        else {
            _actionPointsText.text = String.Format(
                _actionPointsTextFormat, 
                UnitActionSystem.instance.GetSelectedUnit.GetActionPoints()
            );
        }
    }
    
    private void HandleTurnChanged() {
        UpdateActionPoints();
    }
    
    private void HandleAnyActionPointsChanged() {
        UpdateActionPoints();
    }

}
