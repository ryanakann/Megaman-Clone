using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    protected virtual void Awake () {
        CustomCollider2D col = GetComponent<CustomCollider2D>();

        if (null != col) {
            // print("Subscribed");
            col.OnCollisionEnter += CustomOnCollisionEnter;
            col.OnCollisionStay += CustomOnCollisionStay;
            col.OnCollisionExit += CustomOnCollisionExit;
            col.OnTriggerEnter += CustomOnTriggerEnter;
            col.OnTriggerStay += CustomOnTriggerStay;
            col.OnTriggerExit += CustomOnTriggerExit;
        }
    }

    protected virtual void CustomOnCollisionEnter (CustomCollider2D other) {
        // print($"{gameObject.name} entered collider {other.name}");
    }

    protected virtual void CustomOnCollisionStay (CustomCollider2D other) {
        // print($"{gameObject.name} stayed in collider {other.name}");
    }

    protected virtual void CustomOnCollisionExit (CustomCollider2D other) {
        // print($"{gameObject.name} exited collider {other.name}");
    }

    protected virtual void CustomOnTriggerEnter (CustomCollider2D other) {
        // print($"{gameObject.name} entered trigger {other.name}");
    }

    protected virtual void CustomOnTriggerStay (CustomCollider2D other) {
        // print($"{gameObject.name} stayed in trigger {other.name}");
    }

    protected virtual void CustomOnTriggerExit (CustomCollider2D other) {
        // print($"{gameObject.name} exited trigger {other.name}");
    }

    protected void OnDestroy () {
        foreach(CustomCollider2D collider in GetComponentsInChildren<CustomCollider2D>()) {
            CustomCollider2D.allActiveColliders.Remove(collider);
        }
    }
}