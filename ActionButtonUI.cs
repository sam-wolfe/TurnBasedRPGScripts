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

}
