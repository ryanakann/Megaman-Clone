using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private AnimatorController controller;

    private void Awake () {
        controller = GetComponent<AnimatorController>();
    }

    private void Update () {
        controller.SetFloat("x", Input.GetAxisRaw("Horizontal"));
    }
}
