using UnityEngine;
using System.Collections;
using System;

//[ExecuteInEditMode]
public class MotionSicknessEffect : MonoBehaviour {

	public Material mat;

	void OnRenderImage(RenderTexture src, RenderTexture dest) {
		// src is the full rendered scene that you would normally send directly to the monitor. 
		// We are intercepting this so we can do a bit more work, before passing it on.
		Graphics.Blit (src, dest, mat);
	}
}
