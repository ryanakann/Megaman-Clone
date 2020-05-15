using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCollider2D : MonoBehaviour {
    public static List<CustomCollider2D> allActiveColliders;
    public List<CustomCollider2D> allColliders;

    [HideInInspector]
    public List<CustomCollider2D> collisions;

    public bool trigger = false;
    public Bounds2D bounds;

    private CustomRigidbody2D rb;

    protected virtual void Awake () {
        if (allActiveColliders == null) {
            allActiveColliders = new List<CustomCollider2D>();
        }
        allActiveColliders.Add(this);

        collisions = new List<CustomCollider2D>();
        rb = GetComponent<CustomRigidbody2D>();
    }

    protected virtual void FixedUpdate () {
        allColliders = allActiveColliders;
        UpdateCollisions();
    }

    public virtual void UpdateCollisions () {
        collisions.Clear();
         
        foreach (CustomCollider2D collider in allActiveColliders) {
            if (collider.gameObject == gameObject) continue; //Ignore self-collision
            float xDistance = Mathf.Abs(transform.position.x + bounds.center.x - (collider.transform.position.x + collider.bounds.center.x));
            float yDistance = Mathf.Abs(transform.position.y + bounds.center.y- (collider.transform.position.y + collider.bounds.center.y));
            float minXDistance = (bounds.size.x / 2f * transform.localScale.x + collider.bounds.size.x / 2f * collider.transform.localScale.x);
            float minYDistance = (bounds.size.y / 2f * transform.localScale.y + collider.bounds.size.y / 2f * collider.transform.localScale.y);

            if (xDistance < minXDistance &&
                yDistance < minYDistance) {
                if (rb != null) {
                    rb.HandleCollision2D(this, collider);
                } else {
                }
            }

        }
    }

    private void OnDrawGizmos () {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + (Vector3)bounds.center, Vector3.Scale((Vector3)bounds.size, transform.localScale));
    }





    [System.Serializable]
    public struct Bounds2D {
        public Vector2 center;
        public Vector2 size;
    }    
}