using TMPro;
using UnityEngine;

public class UnitWorldUI : MonoBehaviour {

    [SerializeField] private Unit _unit;
    [SerializeField] TextMeshProUGUI _actionPointsText;

    private void Start() {
        Unit.OnAnyActionPointsChanged += UpdateActionPointsText;
        UpdateActionPointsText();
    }

    private void UpdateActionPointsText() {
        _actionPointsText.text = _unit.GetActionPoints().ToString();
    }
}
