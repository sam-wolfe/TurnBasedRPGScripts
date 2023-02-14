using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour {
    
    [SerializeField] private TextMeshProUGUI _turnNumberText;
    [SerializeField] private Button _button;
    [SerializeField] private string _turnNumbertFormat = "Turn {0}";
    
    private TextMeshProUGUI _buttonText;
    private Color _initialTextColor;

    private void Start() {
        _turnNumberText.text = String.Format(
            _turnNumbertFormat, TurnSystem.instance.GetTurnNumber());
        
        _button.onClick.AddListener(() => {
            TurnSystem.instance.NextTurn();
        });
        
        _buttonText = _button.GetComponentInChildren<TextMeshProUGUI>();
        _initialTextColor = _buttonText.color;
    }

    private void OnEnable() {
        TurnSystem.instance.OnTurnChanged += HandleTurnChange;
    }
    
    private void OnDisable() {
        TurnSystem.instance.OnTurnChanged -= HandleTurnChange;
    }

    private void HandleTurnChange() {
        _turnNumberText.text = String.Format(
            _turnNumbertFormat, TurnSystem.instance.GetTurnNumber());
        UpdateEndTurnButton();
    }
    
    private void UpdateEndTurnButton() {
        if (!TurnSystem.instance.IsPlayerTurn()) {
            _button.interactable = false;

            var textColor = _buttonText.color;
            textColor.a = 0.1f;
            _buttonText.color = textColor;
        } else {
            _button.interactable = true;  
            
            _buttonText.color = _initialTextColor;
        }
    }

}
