using System;
using Grid;
using UnityEngine;

public class Unit : MonoBehaviour {

    [SerializeField] private Animator unitAnimator;
    
    private Vector3 targetPosition;
    private float stoppingDistance;
    private GridPosition _gridPosition;
    
    
    // --------------------------------------------------------------------
    // NOTE: my better way of doing this
    //    * Unit has its own event that it envokes when it is selected
    //    * Unit anything can cause a unit to be selected, so no coupling
    //    * Indicator doesn't care what selects the unit, just if a unit is selected
    
    
    public event Action OnUnitSelected;
    public event Action OnUnitDeSelected;

    public void Select() {
        OnUnitSelected?.Invoke();
    }
    
    public void DeSelect() {
        OnUnitDeSelected?.Invoke();
    }

    // --------------------------------------------------------------------

    private void Awake() {
        targetPosition = transform.position;
    }

    void Start() {
        _gridPosition = LevelGrid.instance.GetGridPosition(transform.position);
        LevelGrid.instance.AddUnitAtGridPosition(_gridPosition, this);
    }

    void Update() {
        if (Vector3.Distance(transform.position, targetPosition) > 0.1f) {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = 4f;

            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
            
            unitAnimator.SetBool("IsWalking", true);
        }
        else {
            unitAnimator.SetBool("IsWalking", false);
        }
        
        GridPosition newGridPosition = LevelGrid.instance.GetGridPosition(transform.position);
        if (newGridPosition != _gridPosition) {
            LevelGrid.instance.UnitMovedGridPosition(this, _gridPosition, newGridPosition);
            _gridPosition = newGridPosition;
        }
    }
    
    
    public void Move(Vector3 targetPosition) {
        this.targetPosition = targetPosition;

    }

}
