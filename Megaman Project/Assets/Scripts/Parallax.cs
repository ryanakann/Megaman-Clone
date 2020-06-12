
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {
	private Transform mainCam;
	public float multiplier = 1f;
	public bool background = true;
	
	public bool ignoreY = true;
	public bool boundY = true;
	public Vector2 textureVBounds;
	public Vector2 cameraYBounds;
	public float yRatio;

	private Vector2 target;
	private float targetX;
	private float targetY;

	private List<Transform> bgs;
	private List<Material> mats;

	private void Awake () {
		mainCam = Camera.main.transform;

		bgs = new List<Transform>();
		mats = new List<Material>();

		foreach (Transform child in transform) {
			// print($"Background: {background} \t Pos: {child.position.z} \t 1: {(background == true && child.position.z < 0f)} \t 2: {(background == false && child.position.z > 0f)}");
			if ((background == true && child.position.z >= 0f) ||
				(background == false && child.position.z <= 0f)) {
				bgs.Add(child);
				mats.Add(child.GetComponent<MeshRenderer>().material);
			} else {
				Debug.LogError($"Invalid child position for {transform.name}/{child.name}. Must be >= 0 if background, or <= 0 if foreground.");
			}
		}

		yRatio = (textureVBounds.y - textureVBounds.x) / (cameraYBounds.y - cameraYBounds.x);
	}

	private void Update () {
		for (int i = 0; i < bgs.Count; i++) {
			if (background) {
				targetX = mainCam.position.x * multiplier * 1 / (bgs[i].position.z + 1);
				targetY = (ignoreY ? 0 : (mainCam.position.y * multiplier - cameraYBounds.x) * yRatio) * 1 / (bgs[i].position.z + 1);
				targetY = Mathf.Clamp(targetY, textureVBounds.x, textureVBounds.y);
			} else {
				targetX = mainCam.position.x * multiplier * 1 * (-bgs[i].position.z + 1) / 50;
				targetY = (ignoreY ? 0 : (mainCam.position.y * multiplier - cameraYBounds.x) * yRatio) * 1 * (-bgs[i].position.z + 1);
				targetY = Mathf.Clamp(targetY, textureVBounds.x, textureVBounds.y);
			}
			
			target = new Vector2(targetX, targetY);

			if (bgs[i].GetComponent<ParallaxAutoOffset>()) {
				target += bgs[i].GetComponent<ParallaxAutoOffset>().offset;
			}
			mats[i].SetTextureOffset("_MainTex", target);
		}
	}
}
