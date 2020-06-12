using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform target;
    [Range(0f, 10f)]
    public float damp = 0.2f;
    [Range(0f, 10f)]
    public float velocityLeading = 0f;

    private Vector3 targetVelocity;
    private Vector3 targetPosition;
    private Vector3 targetPositionLF;
    private Vector2 smoothVelRef;
    private Vector3 zOffset;
    Camera cam;

    private Vector3 offset;

    private void Start() {
        cam = GetComponent<Camera>();

        zOffset = Vector3.forward * transform.position.z;
        transform.position = target.position;
        targetVelocity = Vector3.zero;
        targetPositionLF = transform.position;
    }

    private void FixedUpdate() {
        if (null == target) return;
        targetVelocity = target.position - targetPositionLF;
        targetPosition = new Vector3(target.position.x, target.position.y, zOffset.z) + targetVelocity * velocityLeading;
        Vector3 targetScreenPos = cam.WorldToViewportPoint(target.position);
        // print("TargetScreenPos: " + targetScreenPos);
        if (targetScreenPos.x < 0.1f || targetScreenPos.x > 0.9f || targetScreenPos.y < 0.1f || targetScreenPos.y > 0.9f) {
            //Outside screen
            // print("Outside");
            transform.position = (Vector3)((Vector2)target.position - (Vector2)offset) + zOffset;
        } else {
            //Inside screen
            // print("Inside");
            transform.position = (Vector3)Vector2.SmoothDamp(transform.position, targetPosition, ref smoothVelRef, 0.2f) + zOffset;
            offset = target.position - transform.position;
        }
        targetPositionLF = targetPosition;
    }
}