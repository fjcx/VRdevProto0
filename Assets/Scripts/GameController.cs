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
	private BlinkEffect blinkEffect;
	private PossesionTrailLauncher pTrailLauncher;
	private bool canBlink = true;

	public Transform homingTarget;

	private void OnEnable() {
		m_VRInput.OnSwipe += HandSwipe;
		EventController.Instance.Subscribe<ModeUpdatedEvent>(OnModeUpdatedEvent);
		EventController.Instance.Subscribe<EnableMSicknessEffectEvent>(OnEnableMSicknessEffectEvent);
		EventController.Instance.Subscribe<PlayBlinkEffectEvent>(OnPlayBlinkEffectEvent);
		EventController.Instance.Subscribe<SwitchBodiesEvent>(OnSwitchBodiesEvent);
		mSicknessEffect = mainCamera.GetComponent<MotionSicknessEffect> ();
		blinkEffect = mainCamera.GetComponent<BlinkEffect> ();
		pTrailLauncher = mainCamera.GetComponent<PossesionTrailLauncher> ();
	}

	private void OnDisable() {
		m_VRInput.OnSwipe -= HandSwipe;
		EventController.Instance.UnSubscribe<ModeUpdatedEvent>(OnModeUpdatedEvent);
		EventController.Instance.UnSubscribe<EnableMSicknessEffectEvent>(OnEnableMSicknessEffectEvent);
		EventController.Instance.UnSubscribe<PlayBlinkEffectEvent>(OnPlayBlinkEffectEvent);
		EventController.Instance.Subscribe<SwitchBodiesEvent>(OnSwitchBodiesEvent);
	}


	private void OnEnableMSicknessEffectEvent(EnableMSicknessEffectEvent evt) {
		Debug.Log ("Enable MotionSickness: " + evt.enable);

		mSicknessEffect.enabled = evt.enable;

		if (mSicknessEffect.isActiveAndEnabled) {
			Debug.Log ("MSick isEnabled: " + mSicknessEffect);
		} else {
			Debug.Log ("MSick notEnabled: " + mSicknessEffect);
		}

		Debug.Log ("MSick isEnabled: " + evt.enable);
	}

	private void OnPlayBlinkEffectEvent(PlayBlinkEffectEvent evt) {
		Debug.Log ("Trying to Blink");
		if (canBlink) {
			canBlink = false;	// don't allow any blinking commands while in motion!!
			blinkEffect.enabled = evt.enable;
			StartCoroutine (CloseEyes (evt.moveTo, evt.closeTimeSpreader, evt.openTimeSpreader, evt.blinkWait));
			// TODO: disable blink effect when not in use ??
		}
	}

	private IEnumerator CloseEyes(Transform moveTo, float closeTimeSpreader, float openTimeSpreader, float blinkWait) {
		Debug.Log ("CloseEyes!");
		float minMask = 0.0f;
		float maxMask = 1.3f;
		float currMask = minMask;

		while (currMask < maxMask) {
			blinkEffect.maskValue = Mathf.Lerp(minMask, maxMask, currMask);
			currMask += closeTimeSpreader * Time.deltaTime;
			yield return null;
		}

		blinkEffect.maskValue = maxMask;

		EventController.Instance.Publish (new TeleportPlayerEvent (moveTo));
		yield return new WaitForSeconds(blinkWait);
		StartCoroutine (OpenEyes(openTimeSpreader));
	}

	private IEnumerator OpenEyes(float openTimeSpreader) {
		Debug.Log ("OpenEyes!");
		float minMask = 0.0f;
		float maxMask = 1.3f;
		float currMask = maxMask;

		while (currMask > minMask) {
			blinkEffect.maskValue = Mathf.Lerp(minMask, maxMask, currMask);
			currMask -= openTimeSpreader * Time.deltaTime;
			yield return null;
		}

		blinkEffect.maskValue = minMask;
		canBlink = true;
	}

	private void OnSwitchBodiesEvent(SwitchBodiesEvent evt) {
		Debug.Log ("Switching bodies!");
		mainCamera.transform.parent = null;
		mainCamera.transform.position = evt.newBodyTransform.position + new Vector3 (0f, 1.5f, 0f); // adjusting for new body height !!! // TODO: take the actual height
		mainCamera.transform.rotation = evt.newBodyTransform.rotation;
		mainCamera.transform.SetParent (evt.newBodyTransform.root);
		Player targetPlayer = evt.newBodyTransform.root.GetComponent<Player> ();
		EventController.Instance.Publish (new PlayerSelectedEvent (targetPlayer.playerName));
	}

	private void HandSwipe(VRInput.SwipeDirection swipeDir) {
		switch (swipeDir) {
		case VRInput.SwipeDirection.NONE:
		case VRInput.SwipeDirection.UP:
		case VRInput.SwipeDirection.DOWN:
		case VRInput.SwipeDirection.LEFT:
			break;
		case VRInput.SwipeDirection.RIGHT:
			//pTrailLauncher.Launch (homingTarget);
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
