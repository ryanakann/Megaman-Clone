using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour {
    private Player player;

    private void Awake () {
        player = GetComponent<Player>();
    }

    private void Update () {
        player.Move(Input.GetAxisRaw("Horizontal"), Input.GetButtonDown("Jump"), Input.GetButton("Jump"));

        if (Input.GetButtonDown("Fire1")) {
            player.Shoot();
        }
    }
}
