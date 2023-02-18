using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    [SerializeField] private GameObject _actionCamera;
    [SerializeField] private GameObject _mainCamera;

    private void OnEnable() {
        BaseAction.OnAnyActionStart += HandleActionStarted;
        BaseAction.OnAnyActionComplete += HandleActionEnded;
    }

    private void OnDisable() {
        BaseAction.OnAnyActionStart -= HandleActionStarted;
        BaseAction.OnAnyActionComplete -= HandleActionEnded;
    }

    public void SwitchToActionCamera() {
        _actionCamera.SetActive(true);
    }

    public void SwitchToMainCamera() {
        _actionCamera.SetActive(false);
    }

    private void HandleActionStarted(BaseAction action) {
        // TODO - refactor to use a property on the action that stores camera behaviour
        // Using a switch is a bit of a code smell here
        switch (action) {
            case ShootAction shootAction:
                Unit unit = shootAction.GetShooterUnit();
                Unit target = shootAction.GetTargetUnit();
                Vector3 cameraHeight = Vector3.up * 1.63f;
                
                Vector3 shootDirection = (target.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                
                Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDirection * 0.5f;
                
                Vector3 actionCameraPosition = 
                    unit.GetWorldPosition() + cameraHeight + shoulderOffset + (shootDirection * -1);
                
                _actionCamera.transform.position = actionCameraPosition;
                _actionCamera.transform.LookAt(unit.GetWorldPosition() + cameraHeight);
                
                SwitchToActionCamera();
                break;
        }
    }
    
    private void HandleActionEnded(BaseAction action) {
        // TODO - refactor to use a property on the action that stores camera behaviour
        // Using a switch is a bit of a code smell here
        switch (action) {
            case ShootAction shootAction:
                
                SwitchToMainCamera();
                break;
        }
    }
        
}
