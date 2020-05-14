using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimationState {
    [SerializeField]
    public new string name = "Animation";
    [SerializeField]
    public  bool loop = true;
    [SerializeField]
    public int framesPerSecond = 24;
    
    [HideInInspector]
    public float secondsPerFrame;

    [SerializeField]
    public Sprite[] frames;


    [HideInInspector]
    public bool playing;

    private void Awake () {
        UpdateFramerate();
    }

    public void UpdateFramerate () {
        secondsPerFrame = 1 / (float)framesPerSecond;
        playing = false;
    }

    public int GetFrameCount () {
        return (frames != null ? frames.Length : 0);
    }

    public float GetDuration () {
        return frames.Length / framesPerSecond * Time.deltaTime;

    }
}