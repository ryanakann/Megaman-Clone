using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tween : MonoBehaviour
{
    public AnimationCurve tween;

    [Tooltip("In seconds")]
    public float tweenDuration = 1f;
    private Vector3 startPos;
    private Vector3 endPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        endPos = transform.position + Vector3.down * 4f;

        InvokeRepeating("PlayTween", 1f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayTween () {
        StartCoroutine(PlayTweenCR());
    }

    IEnumerator PlayTweenCR () {
        float t = 0f;
        float tTween;
        while (t <= 1f) {
            tTween = tween.Evaluate(t);
            transform.position = Vector3.Lerp(startPos, endPos, tTween);
            t += Time.deltaTime / tweenDuration;
            yield return new WaitForEndOfFrame();
        }
    }
}
