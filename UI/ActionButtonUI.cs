using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _selectedVisual;
    
    public BaseAction action { get; private set;}
    
    private Color _initialTextColor;

    private void Start() {
        _initialTextColor = _buttonText.color;
    }

    public void SetBaseAction(BaseAction baseAction) {
        _buttonText.text = baseAction.GetActionName().ToUpper();
        action = baseAction;
        
        _button.onClick.AddListener(() => {
            UnitActionSystem.instance.SetSelectedAction(baseAction);
        });
    }
    
    public void SetSelectedVisual() {
        _selectedVisual.SetActive(action == UnitActionSystem.instance.GetSelectedAction);
    }
    
    public void SetDisabled(bool disabled) {
        if (disabled) {
            _button.interactable = false;
            
            var textColor = _buttonText.color;
            textColor.a = 0.1f;
            _buttonText.color = textColor;
            
            _selectedVisual.SetActive(false);
        } else {
            _button.interactable = true;  
            
            _buttonText.color = _initialTextColor;
        }
        
    }

}
