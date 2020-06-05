using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CustomRigidbody2D), typeof(AnimatorController), typeof(SpriteRenderer))]
public class Player : Entity {
    private new SpriteRenderer renderer;
    private AnimatorController controller;
    private CustomRigidbody2D rb;
    
    [SerializeField]
    private GameObject bulletPrefab;

    [Header("Control")]
    public bool locked;

    [Header("Movement")]
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

    [Header("Shot Control")]
    public float shotMaxCD = 0.2f;
    private float shotCurCD;
    public int maxShotsPerInterval = 3;
    public float intervalSeconds = 1f;
    public float curIntervalOffset;
    public int shotsTakenThisInterval;
    public float timeSinceLastShot;

    protected override void Awake () {
        base.Awake();

        renderer = GetComponent<SpriteRenderer>();
        controller = GetComponent<AnimatorController>();
        rb = GetComponent<CustomRigidbody2D>();

        shotCurCD = shotMaxCD;
        shotsTakenThisInterval = 0;
        curIntervalOffset = 0f;
        timeSinceLastShot = Mathf.Infinity;

        StartCoroutine(StartupSequence());
    }

    protected IEnumerator StartupSequence () {
        locked = true;
        controller.SetBool("startup", true);
        Vector3 end = transform.position;
        Vector3 start = transform.position + Vector3.up * 10f;
        rb.gravityScale = 0f;
        transform.position = start;
        float t = 0f;
        while (t < 1f) {
            transform.position = Vector3.Lerp(start, end, t);
            t += Time.deltaTime * 2.5f;
            yield return new WaitForEndOfFrame();
        }
        rb.gravityScale = 1f;
        transform.position = end;
        yield return new WaitForSeconds(0.25f);
        locked = false;
        controller.SetBool("startup", false);
    }

    protected void Update () {
        if (shotCurCD <= 0) {
            shotCurCD = 0f;
        } else {
            shotCurCD -= Time.deltaTime;
        }
        timeSinceLastShot += Time.deltaTime;

        if (curIntervalOffset >= intervalSeconds || shotsTakenThisInterval == 0) {
            curIntervalOffset = 0f;
            shotsTakenThisInterval = 0;
        } else {
            curIntervalOffset += Time.deltaTime;
        }

        controller.SetInt("shoot", timeSinceLastShot < 1f ? 1 : 0);
        controller.SetInt("jump", !rb.grounded ? 1 : 0);
    }

    public void Shoot () {
        if (shotCurCD <= 0 && shotsTakenThisInterval < maxShotsPerInterval) {
            shotsTakenThisInterval++;
            timeSinceLastShot = 0f;
            shotCurCD = shotMaxCD;

            GameObject instance = Instantiate(bulletPrefab, transform.position + transform.right * (renderer.flipX ? -1 : 1), Quaternion.identity);
            instance.GetComponent<SpriteRenderer>().flipX = renderer.flipX;
        }
    }

    public void Move(float x, bool jumpDown, bool jumpStay) {
        if (locked) return;
        if (x > Mathf.Epsilon) {
            renderer.flipX = false;
        }
        if (x < -Mathf.Epsilon) {
            renderer.flipX = true;
        }

        rb.velocity.x = x * (rb.grounded ? groundSpeed : airSpeed);

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
