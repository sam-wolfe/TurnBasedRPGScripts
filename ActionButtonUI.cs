using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _selectedVisual;
    
    public BaseAction action { get; private set;}
    
    private Color _initialNormalColor;
    private Color _initialTextColor;

    private void Start() {
        _initialNormalColor = _button.colors.normalColor;
        
        _initialTextColor = _buttonText.color;
    }

    public void SetBaseAction(BaseAction baseAction) {
        _buttonText.text = baseAction.GetActionName().ToUpper();
        action = baseAction;
        
        _button.onClick.AddListener(() => {
            UnitActionSystem.instance.SetSelectedAction(baseAction);
        });
    }
    
    public void SetSelectedVisual(bool selected) {
        _selectedVisual.SetActive(selected);
    }
    
    public void SetDisabled(bool disabled) {
        if (disabled) {
            _button.interactable = false;
            
            var colors = _button.colors;
            var buttonColor = _button.colors.normalColor;
            buttonColor.a = 0.1f;
            colors.normalColor = buttonColor;
            _button.colors = colors;
            
            var textColor = _buttonText.color;
            textColor.a = 0.1f;
            _buttonText.color = textColor;
            
            _selectedVisual.SetActive(false);
        } else {
            _button.interactable = true;  
            
            var colors = _button.colors;
            colors.normalColor = _initialNormalColor;
            _button.colors = colors;
            
            _buttonText.color = _initialTextColor;
            
        }
        
    }

}
