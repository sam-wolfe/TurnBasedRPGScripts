using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Cinemachine;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CameraController : MonoBehaviour {

    [SerializeField] private float cameraPanSpeed = 5;
    [SerializeField] private float cameraRotateSpeed = 5;
    [SerializeField] private float cameraZoomSpeed = 1;
    [SerializeField] private float cameraZoomMin = 2;
    [SerializeField] private float cameraZoomMax = 12;

    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private CinemachineTransposer _cinemachineTransposer;
    
    private Vector3 _followOffset;
    
    
    private void Start() {
        _cinemachineTransposer =
            _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        _followOffset = _cinemachineTransposer.m_FollowOffset;
    }

    void Update() {
        ProcessMoveOldInput();
        ProcessZoomOldInput();
        ProcessRotationOldInput();
    }

    private void ProcessZoomOldInput() {

        if (Input.mouseScrollDelta.y > 0) {
            _followOffset.y -= cameraZoomSpeed;
        } else if (Input.mouseScrollDelta.y < 0) {
            _followOffset.y += cameraZoomSpeed;
        }

        
        _followOffset.y = Mathf.Clamp(_followOffset.y, cameraZoomMin, cameraZoomMax);
        
        _cinemachineTransposer.m_FollowOffset = Vector3.Lerp(
            _cinemachineTransposer.m_FollowOffset, _followOffset, Time.deltaTime * 5f);

    }

    private void ProcessMoveOldInput() {
        Vector3 inputMoveDir = new Vector3();

        if (Input.GetKey(KeyCode.W)) {
            inputMoveDir.z = 1f;
        }
        if (Input.GetKey(KeyCode.S)) {
            inputMoveDir.z = -1f;
        }
        if (Input.GetKey(KeyCode.A)) {
            inputMoveDir.x = -1f;
        }
        if (Input.GetKey(KeyCode.D)) {
            inputMoveDir.x = 1f;
        }


        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        transform.position += moveVector * cameraPanSpeed * Time.deltaTime;
        


    }

    private void ProcessRotationOldInput() {
        Vector3 inputRotateDir = new Vector3();
                
        if (Input.GetKey(KeyCode.Q)) {
            inputRotateDir.y = -1f;
        }
        if (Input.GetKey(KeyCode.E)) {
            inputRotateDir.y = 1f;
        }
        Vector3 rotateVector = new Vector3(0, inputRotateDir.y, 0);

        transform.eulerAngles += rotateVector * cameraRotateSpeed * Time.deltaTime;
    }
}
