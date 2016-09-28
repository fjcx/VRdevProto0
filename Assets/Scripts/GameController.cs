using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using VRStandardAssets.Utils;

public class GameController : MonoBehaviour {

	[SerializeField] private GameObject mainCamera;
	[SerializeField] private Reticle m_Reticle;                         // This needs to be turned on and off and it's settings altered to be appropriate for target setting.
	[SerializeField] private FloorTargeting m_FloorTargeting;     		// This needs to be turned on and off when the game is running and not.

	[SerializeField] private VRInput m_VRInput;
	public Text currentModeText;
	private string currentMode;
	private MotionSicknessEffect mSicknessEffect;

	private void OnEnable() {
		m_VRInput.OnSwipe += HandSwipe;
		EventController.Instance.Subscribe<ModeUpdatedEvent>(OnModeUpdatedEvent);
		EventController.Instance.Subscribe<EnableMSicknessEffectEvent>(OnEnableMSicknessEffectEvent);
		mSicknessEffect = mainCamera.GetComponent<MotionSicknessEffect> ();
	}

	private void OnDisable() {
		m_VRInput.OnSwipe -= HandSwipe;
		EventController.Instance.UnSubscribe<ModeUpdatedEvent>(OnModeUpdatedEvent);
		EventController.Instance.UnSubscribe<EnableMSicknessEffectEvent>(OnEnableMSicknessEffectEvent);
	}


	private void OnEnableMSicknessEffectEvent(EnableMSicknessEffectEvent evt) {
		Debug.Log ("Enable MotionSickness: " + evt.enable);

		mSicknessEffect.enabled = evt.enable;

		if (mSicknessEffect.isActiveAndEnabled) {
			Debug.Log ("isEnabled: " + mSicknessEffect);
		} else {
			Debug.Log ("notEnabled: " + mSicknessEffect);
		}

		Debug.Log ("isEnabled: " + evt.enable);
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

	private void OnModeUpdatedEvent(ModeUpdatedEvent evt) {
		currentMode = evt.newMode;
		currentModeText.text = "Mode: " + currentMode;
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
