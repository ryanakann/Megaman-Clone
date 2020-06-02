using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CollisionEvent(CustomCollider2D other);

public class CustomCollider2D : MonoBehaviour {
    public static HashSet<CustomCollider2D> allActiveColliders;

    private HashSet<CustomCollider2D> collisions;
    private HashSet<CustomCollider2D> collisionsLF;
    private List<CustomCollider2D> collisionsLFList;

    private HashSet<CustomCollider2D> triggerCollisions;
    private HashSet<CustomCollider2D> triggerCollisionsLF;
    private List<CustomCollider2D> triggerCollisionsLFList;


    public event CollisionEvent OnCollisionEnter;
    public event CollisionEvent OnCollisionStay;
    public event CollisionEvent OnCollisionExit;

    public event CollisionEvent OnTriggerEnter;
    public event CollisionEvent OnTriggerStay;
    public event CollisionEvent OnTriggerExit;

    private CustomRigidbody2D rb;

    //Inspector variables
    /*********************************/
    public new string tag = "Default"; 
    public bool trigger = false;
    public Bounds2D bounds;
    /*********************************/

    protected virtual void Awake () {
        if (allActiveColliders == null) {
            allActiveColliders = new HashSet<CustomCollider2D>();
        }
        allActiveColliders.Add(this);
        rb = GetComponent<CustomRigidbody2D>();

        InitEvents();
    }

    protected virtual void InitEvents () {
        collisions = new HashSet<CustomCollider2D>();
        collisionsLF = new HashSet<CustomCollider2D>();
        collisionsLFList = new List<CustomCollider2D>();
        triggerCollisions = new HashSet<CustomCollider2D>();
        triggerCollisionsLF = new HashSet<CustomCollider2D>();
        triggerCollisionsLFList = new List<CustomCollider2D>();
    }

    //Called every physics frame (physics loop instead of game loop)
    protected virtual void FixedUpdate () {
        UpdateCollisions();
    }

    public virtual void UpdateCollisions () {
        collisions.Clear();
        triggerCollisions.Clear();
        
        foreach (CustomCollider2D collider in allActiveColliders) {
            if (collider.gameObject == gameObject) continue; //Ignore self-collision
            float xDistance = Mathf.Abs(transform.position.x + bounds.center.x - (collider.transform.position.x + collider.bounds.center.x));
            float yDistance = Mathf.Abs(transform.position.y + bounds.center.y- (collider.transform.position.y + collider.bounds.center.y));
            float minXDistance = (bounds.size.x / 2f * transform.localScale.x + collider.bounds.size.x / 2f * collider.transform.localScale.x);
            float minYDistance = (bounds.size.y / 2f * transform.localScale.y + collider.bounds.size.y / 2f * collider.transform.localScale.y);

            if (xDistance < minXDistance && yDistance < minYDistance) {
                //Resolve collisions if applicable
                rb?.HandleCollision2D(this, collider);

                if (false == collider.trigger) {
                    collisions.Add(collider);
                    if (false == collisionsLF.Contains(collider)) {
                        OnCollisionEnter?.Invoke(collider);
                    } else {
                        OnCollisionStay?.Invoke(collider);
                    }
                } else {
                    if (false == triggerCollisionsLF.Contains(collider)) {
                        OnTriggerEnter?.Invoke(collider);
                    } else {
                        OnTriggerStay?.Invoke(collider);
                    }
                }
            }
        }

        //Exit detection
        foreach(CustomCollider2D collider in collisionsLFList) {
            if (false == collisions.Contains(collider)) {
                OnCollisionExit?.Invoke(collider);
            }
        }
        foreach(CustomCollider2D collider in triggerCollisionsLFList) {
            if (false == triggerCollisions.Contains(collider)) {
                OnTriggerExit?.Invoke(collider);
            }
        }

        //Update collisions from previous frame
        collisionsLF = new HashSet<CustomCollider2D>(collisions);
        triggerCollisionsLF = new HashSet<CustomCollider2D>(triggerCollisions);
        collisionsLFList = new List<CustomCollider2D>(collisionsLF);
        triggerCollisionsLFList = new List<CustomCollider2D>(triggerCollisionsLF);
    }

    private void OnDrawGizmos () {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + (Vector3)bounds.center, Vector3.Scale((Vector3)bounds.size, transform.localScale));
    } 
}