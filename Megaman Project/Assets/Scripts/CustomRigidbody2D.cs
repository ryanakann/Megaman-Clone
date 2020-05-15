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
        float xSignedDist = otherCenter.x - myCenter.x;
        float xDist = Mathf.Abs(xSignedDist);
        float ySignedDist = otherCenter.y - myCenter.y;
        float yDist = Mathf.Abs(ySignedDist);
        float xTargetDist = (mySize.x + otherSize.x) / 2f;
        float yTargetDist = (mySize.y + otherSize.y) / 2f;
        if (xDist < xTargetDist && velocity.x * xSignedDist > 0f && xDist > yDist) {
            transform.position -= Vector3.right * (xTargetDist-xDist) * Mathf.Sign(xSignedDist);
            velocity.x = 0f;
        }
        if (yDist < yTargetDist && velocity.y * ySignedDist > 0f && yDist > xDist) {
            transform.position -= Vector3.up * (yTargetDist-yDist) * Mathf.Sign(ySignedDist);
            velocity.y = 0f;
        }

        //Grounded threshold
        if (ySignedDist < yTargetDist + 0.01f) {
            grounded = true;
        }
    } 
}