using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private Button _button;

    public void SetBaseAction(BaseAction baseAction) {
        _buttonText.text = baseAction.GetActionName().ToUpper();
        
        _button.onClick.AddListener(() => {
            UnitActionSystem.instance.SetSelectedAction(baseAction);
        });
    }

}
