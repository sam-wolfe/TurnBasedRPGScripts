using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour {

    [SerializeField] private Unit _unit;
    [SerializeField] TextMeshProUGUI _actionPointsText;
    [SerializeField] private Image _healthBar;
    [SerializeField] private HealthSystem _healthSystem;

    private void Start() {
        Unit.OnAnyActionPointsChanged += UpdateActionPointsText;
        UpdateActionPointsText();
        
        _healthSystem.OnHealthChanged += UpdateHealthBar;
    }
    
    private void OnDisable() {
        Unit.OnAnyActionPointsChanged -= UpdateActionPointsText;
    }

    private void UpdateActionPointsText() {
        _actionPointsText.text = _unit.GetActionPoints().ToString();
    }
    
    private void UpdateHealthBar() {
        _healthBar.fillAmount = _healthSystem.GetHealthPercent();
    }
}
