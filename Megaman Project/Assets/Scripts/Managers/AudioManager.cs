using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static void Play (AudioClip clip, bool isMusic) {
        GameObject obj = new GameObject(clip.name); //Special constructor that instantiates a new empty GameObject into the scene
        AudioSource source = obj.AddComponent<AudioSource>();
        source.clip = clip;

        if (isMusic) {
            source.loop = true;
        } else {
            source.loop = false;
            Destroy(obj, clip.length + 0.1f);
        }
        source.Play();
    }
}