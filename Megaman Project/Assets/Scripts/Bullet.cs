using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Entity {

    [Range(1f, 20f)]
    public float speed = 5f;
    public AudioClip shootSound;

    protected override void Awake () {
        base.Awake();
        if (shootSound != null) {
            AudioManager.Play(shootSound, false);
        }
        Destroy(gameObject, 10f / speed);
    }

    void Update () {
        transform.position += transform.right * (GetComponent<SpriteRenderer>().flipX ? -1 : 1) * speed * Time.deltaTime;
    }

    protected override void CustomOnCollisionEnter(CustomCollider2D other) {
        Destroy(gameObject);
    }
}