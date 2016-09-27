﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using VRStandardAssets.Utils;

public class GameController : MonoBehaviour {

	[SerializeField] private Reticle m_Reticle;                         // This needs to be turned on and off and it's settings altered to be appropriate for target setting.
	[SerializeField] private FloorTargeting m_FloorTargeting;     		// This needs to be turned on and off when the game is running and not.

	[SerializeField] private VRInput m_VRInput;
	//private bool isHidden = false;
	//public Text currentModeText;
	//private string currentMode;

	private void OnEnable() {
		m_VRInput.OnSwipe += HandSwipe;
	}

	private void OnDisable() {
		m_VRInput.OnSwipe -= HandSwipe;
	}

	private void HandSwipe(VRInput.SwipeDirection swipeDir) {
		switch (swipeDir) {
		case VRInput.SwipeDirection.NONE:
		case VRInput.SwipeDirection.UP:
		case VRInput.SwipeDirection.DOWN:
		case VRInput.SwipeDirection.LEFT:
		case VRInput.SwipeDirection.RIGHT:
			break;
		}
	}

	private void UpdateCurrentMode() {
		/*SlidingMenu sMenu = slidingMenu.GetComponent<SlidingMenu>();
		currentMode = sMenu.selectedMode;
		currentModeText.text = "Mode: " + currentMode;*/
	}

	// Use this for initialization
	void Start () {
		//UpdateCurrentMode();

		m_FloorTargeting.Activate ();
		m_Reticle.Show ();
		m_Reticle.UseNormal = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
