using UnityEngine;

public class LookAtCamera : MonoBehaviour {

    [SerializeField] private bool invert = true;
    private Transform CameraTransform;

    private void Awake() {
        CameraTransform = Camera.main.transform;
    }

    private void LateUpdate() {
        if (invert) {
            var position = transform.position;
            Vector3 dirToCamera = (CameraTransform.position - position).normalized;
            transform.LookAt(position + dirToCamera * -1);
        } else if (!invert) {
            transform.LookAt(CameraTransform);
        }
    }

}
