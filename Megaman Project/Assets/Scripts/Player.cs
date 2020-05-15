using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CustomRigidbody2D), typeof(AnimatorController), typeof(SpriteRenderer))]
public class Player : MonoBehaviour {
    private new SpriteRenderer renderer;
    private AnimatorController controller;
    private CustomRigidbody2D rb;
    


    [Range(0.25f, 20f)]
    public float groundSpeed = 1f;
    [Range(0.25f, 20f)]
    public float airSpeed = 1f;
    [Range(0.25f, 20f)]
    public float maxJumpHeight = 1f;
    [Tooltip("How snappy a short hop is compared to a full jump")]
    [SerializeField] public float lowJumpMultiplier = 4f;
    [Tooltip("How quickly you fall after the apex of your jump")]
    [SerializeField] public float fallMultiplier = 2f;
    [Tooltip("Multiplier independent of above jump parameters")]
    [SerializeField] public float gravityMultiplier = 1f;

    private void Awake () {
        renderer = GetComponent<SpriteRenderer>();
        controller = GetComponent<AnimatorController>();
        rb = GetComponent<CustomRigidbody2D>();
    }

    public void Move(float x, bool jumpDown, bool jumpStay) {
        if (x > Mathf.Epsilon) {
            renderer.flipX = false;
        }
        if (x < -Mathf.Epsilon) {
            renderer.flipX = true;
        }

        rb.velocity.x = x * (rb.grounded ? groundSpeed : airSpeed);

        // if (jumpDown && rb.grounded) {
        //     rb.velocity.y = 10f;
        // }

        //JUMP MODIFIER
        /******************************************************************/
        // Moving upwards, and not holding jump
        if (!rb.grounded && !jumpStay && Vector2.Dot(Physics2D.gravity, rb.velocity) < 0f) {
            rb.gravityScale = lowJumpMultiplier;
        } 
        // Falling
        else if (!rb.grounded && Vector2.Dot(Physics2D.gravity, rb.velocity) > 0f) {
            rb.gravityScale = fallMultiplier;
        } 
        // Grounded
        else {
            rb.gravityScale = 1f;
        }
        rb.gravityScale *= gravityMultiplier;

        //JUMP CONTROL
        /******************************************************************/
        if (rb.grounded && jumpDown) {
            rb.grounded = false;

            rb.velocity = (Vector2)transform.right * rb.velocity.x; // Zero out jump velocity
            float jumpVelocity = Mathf.Sqrt(2.0f * Physics2D.gravity.magnitude * gravityMultiplier * maxJumpHeight);

            rb.velocity -= Physics2D.gravity.normalized * jumpVelocity;

            // controller.SetTrigger("jump");
        }

        controller.SetFloat("x", Mathf.Abs(x));
    }
}
