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

    private void Start() {
        _turnNumberText.text = String.Format(_turnNumbertFormat, TurnSystem.instance.GetTurnNumber());
        
        _button.onClick.AddListener(() => {
            TurnSystem.instance.NextTurn();
            _turnNumberText.text = String.Format(_turnNumbertFormat, TurnSystem.instance.GetTurnNumber());
        });
    }

}
