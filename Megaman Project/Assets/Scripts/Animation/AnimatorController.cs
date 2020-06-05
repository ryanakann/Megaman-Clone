using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatorController : MonoBehaviour {
    public string defaultState;
    public string currentState;
    [HideInInspector]
    public AnimationState currentAnimation;

    public AnimationProperty[] properties;
    public AnimationState[] animations;
    public AnimationTransition[] transitions;
    

    public Dictionary<string, AnimationProperty> propertyMap;    
    public Dictionary<string, AnimationState> animationStateMap;
    public Dictionary<AnimationState, List<AnimationTransition>> stateToTransitionMap;

    private new SpriteRenderer renderer;
    

    private void Awake () {
        InitPropertyMap();
        InitAnimationStateMap();
        InitStateToTransitionMap();
        print(stateToTransitionMap[animationStateMap["idle"]].Count);
        renderer = GetComponent<SpriteRenderer>();

        currentState = defaultState;
        currentAnimation = GetAnimation(currentState);
        Play();
    }

    private void Update () {
        foreach(AnimationTransition transition in stateToTransitionMap[currentAnimation]) {
            Debug.Log($"{transition.entryState}-{transition.exitState}...");
            if (true == transition.Evaluate(this)) {
                if (currentState != transition.exitState) {
                    currentState = transition.exitState;
                    currentAnimation = GetAnimation(currentState);
                    Play();
                    break;
                }
            }
        }
    }


    private void InitPropertyMap () {
        propertyMap = new Dictionary<string, AnimationProperty>();

        foreach (AnimationProperty property in properties) {
            propertyMap.Add(property.name, property);
        }
    }

    private void InitAnimationStateMap () {
        animationStateMap = new Dictionary<string, AnimationState>();

        foreach(AnimationState animation in animations) {
            animationStateMap.Add(animation.name, animation);
        }
    }

    private void InitStateToTransitionMap () {
        stateToTransitionMap = new Dictionary<AnimationState, List<AnimationTransition>>();

        foreach (AnimationTransition transition in transitions) {
            AnimationState state = GetAnimation(transition.entryState);
            if (state == null) {
                Debug.Log($"{transition.entryState}-{transition.exitState} not registered. State was null!");
                continue;
            }
            if (stateToTransitionMap.ContainsKey(state)) {
                stateToTransitionMap[state].Add(transition);
            } else {
                List<AnimationTransition> newTransitions = new List<AnimationTransition>();
                newTransitions.Add(transition);
                stateToTransitionMap.Add(state, newTransitions); 
            }
            Debug.Log($"{transition.entryState}-{transition.exitState} transition registered! Count: {stateToTransitionMap[state].Count}");
        }
    }

    public AnimationState GetAnimation (string state) {
        if (animationStateMap.ContainsKey(state)) {
            return animationStateMap[state];
        } else {
            return null;
        }
    }

    public void SetFloat (string name, float value) {
        propertyMap[name].value = value;
    }

    public void SetInt (string name, int value) {
        propertyMap[name].value = value;
    }

    public void SetBool (string name, bool value) {
        propertyMap[name].value = (value ? 1f : 0f);
    }

    public void SetTrigger(string name) {
        StartCoroutine(SetTriggerCoroutine(name));
    }

    //Ensures that trigger is only true for one frame;
    private IEnumerator SetTriggerCoroutine (string name) {
        SetBool(name, true);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        SetBool(name, false);
    }

    public float GetFloat(string name) {
        try {
            return (float)propertyMap[name].value;
        } catch {
            return 0f;
        }
    }

    public int GetInt(string name) {
        try {
            return (int)propertyMap[name].value;
        } catch {
            return 0;
        }
    }

    public bool GetBool(string name) {
        try {
            return propertyMap[name].value > 0f;
        } catch {
            return false;
        }
    }

    public bool GetTrigger(string name) {
        try {
            return propertyMap[name].value > 0f;
        } catch {
            return false;
        }
    }

    public System.Type GetPropertyType(string name)
    {
        var prop = propertyMap[name].value;
        return prop.GetType();
    }

    public void Play () {
        Stop();
        currentAnimation.playing = true;
        currentAnimation.UpdateFramerate();
        StartCoroutine(PlayCoroutine(currentAnimation));
    }

    public void Stop () {
        currentAnimation.playing = false;
        StopAllCoroutines();
    }

    private IEnumerator PlayCoroutine (AnimationState state) {
        int numFrames = state.GetFrameCount();
        int frameIndex = 0;
        bool repeat = true;
        while (repeat) {
            renderer.sprite = state.frames[frameIndex];
            yield return new WaitForSeconds(state.secondsPerFrame);
            frameIndex = (frameIndex + 1) % numFrames;
            if (!state.loop && frameIndex == 0) {
                repeat = false;
            } 
        }
        state.playing = false;
    }
}