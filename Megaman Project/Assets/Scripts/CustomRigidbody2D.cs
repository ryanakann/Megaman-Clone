using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomRigidbody2D : MonoBehaviour {
    public Vector2 velocity;
    private Vector2 velocityLastFrame;
    public Vector2 acceleration;
    public float gravityScale;
    public bool grounded = false;

    protected virtual void FixedUpdate () {
        grounded = false;
        velocity += Physics2D.gravity * gravityScale * Time.fixedDeltaTime;
        transform.position += (Vector3)velocity * Time.fixedDeltaTime;

        acceleration = (velocity - velocityLastFrame) / Time.fixedDeltaTime;
        velocityLastFrame = velocity;
    }

    public virtual void HandleCollision2D(CustomCollider2D me, CustomCollider2D other) {
        Vector2 myCenter = (Vector2)transform.position + me.bounds.center;
        Vector2 mySize = me.bounds.size;
        Vector2 otherCenter = (Vector2)other.transform.position + other.bounds.center;
        Vector2 otherSize = other.bounds.size;
        float xDist = otherCenter.x - myCenter.x;
        float yDist = otherCenter.y - myCenter.y;
        float xTargetDist = (mySize.x + otherSize.x) / 2f;
        float yTargetDist = (mySize.y + otherSize.y) / 2f;
        if (Mathf.Abs(xDist) < xTargetDist && Mathf.Abs(xDist) > Mathf.Abs(yDist)) {
            transform.position -= Vector3.right * (xTargetDist - Mathf.Abs(xDist)) * Mathf.Sign(xDist);
            velocity.x = 0;
        }
        if (Mathf.Abs(yDist) < yTargetDist && Mathf.Abs(yDist) > Mathf.Abs(xDist)) {
            transform.position -= Vector3.up * (yTargetDist - Mathf.Abs(yDist)) * Mathf.Sign(yDist);
            velocity.y = 0;
        }

        //Grounded threshold
        if (yDist < yTargetDist + 0.01f) {
            grounded = true;
        }
    } 
}