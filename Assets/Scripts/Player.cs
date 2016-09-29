using System;
using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;		// if updating use other namespace
using VRStandardAssets.Utils;

public class Player : MonoBehaviour {
	
	//[SerializeField] private FloorTargeting m_FloorTargeting;                 // This triggers an event when the target is set. 

	[SerializeField] private VRInteractiveItem m_InteractiveItem;
	private AIPlayerControl m_AiCharacter;
	public string playerName;
	public Boolean isSelected = false;
	public Boolean isCameraHolder = false;
	public GameObject selectedCircle;

	private string mode;	// TODO: move this to centralling controlled area (and perhaps movement also!!)

	private void Awake()
	{
		// Setup references.
		m_AiCharacter = GetComponent<AIPlayerControl>();
	}
		
	private void OnEnable ()
	{
		//m_FloorTargeting.OnTargetSet += HandleSetTarget;
		EventController.Instance.Subscribe<MoveToEvent>(OnMoveToEvent);
		EventController.Instance.Subscribe<PlayerSelectedEvent>(OnPlayerSelectedEvent);
		EventController.Instance.Subscribe<ModeUpdatedEvent>(OnModeUpdatedEvent);
		EventController.Instance.Subscribe<TeleportPlayerEvent>(OnTeleportPlayerEvent);

		m_InteractiveItem.OnDoubleClick += HandleDoubleClick;
		m_InteractiveItem.OnOver += HandleOver;
		m_InteractiveItem.OnOut += HandleOut;
	}


	private void OnDisable()
	{
		//m_FloorTargeting.OnTargetSet -= HandleSetTarget;
		EventController.Instance.UnSubscribe<MoveToEvent>(OnMoveToEvent);
		EventController.Instance.UnSubscribe<PlayerSelectedEvent>(OnPlayerSelectedEvent);
		EventController.Instance.UnSubscribe<ModeUpdatedEvent>(OnModeUpdatedEvent);
		EventController.Instance.UnSubscribe<TeleportPlayerEvent>(OnTeleportPlayerEvent);
		m_InteractiveItem.OnDoubleClick -= HandleDoubleClick;
		m_InteractiveItem.OnOver -= HandleOver;
		m_InteractiveItem.OnOut -= HandleOut;
	}

	private void HandleDoubleClick()
	{
		Debug.Log ("Clicking player here");
		if (!isCameraHolder) {
			if (mode == "BodySwitchTeleport") {
				EventController.Instance.Publish (new SwitchBodiesEvent (transform));
			} else {
				EventController.Instance.Publish (new PlayerSelectedEvent (playerName));
			}
		}
	}

	private void HandleOver()
	{
		if (!isSelected && !isCameraHolder) {
			selectedCircle.SetActive (true);
		}
	}

	private void HandleOut()
	{
		if (!isSelected && !isCameraHolder) {
			selectedCircle.SetActive (false);
		}
	}

	private void OnPlayerSelectedEvent(PlayerSelectedEvent evt) {
		if (evt.playerName == playerName) {
			Debug.Log ("Selected!" + playerName + "/" + evt.playerName);
			isSelected = true;
			selectedCircle.SetActive (true);

			if (mode == "BodySwitchTeleport") {
				isCameraHolder = true;
			}

		} else {
			Debug.Log ("DeSelected!" + playerName + "/" + evt.playerName);
			isSelected = false;
			selectedCircle.SetActive (false);

			if (mode == "BodySwitchTeleport") {
				isCameraHolder = false;
			}
		}
	}

	private void OnMoveToEvent(MoveToEvent evt) {
		if (isSelected == true ) {
			if (mode == "BlinkTeleport") {
				// Blink Move to point
				// TODO: expose values for blink !!!
				EventController.Instance.Publish (new PlayBlinkEffectEvent (true, evt.moveToTransform, 1.1f, 6f, 0f));
			} else {
				// Move animation and travesal to point
				Debug.Log ("playerName: " + playerName + " isSelected and moving");
				HandleSetTarget (evt.moveToTransform);
			}
		}
	}

	private void OnTeleportPlayerEvent(TeleportPlayerEvent evt) {
		if (isSelected == true ) {
			transform.position = evt.moveTo.position;	// is this the best way to teleport?
		}
	}

	private void OnModeUpdatedEvent(ModeUpdatedEvent evt) {
		mode = evt.newMode;

		if (isCameraHolder) {
			if (mode == "MotionSicknessMovement") {
				EventController.Instance.Publish (new EnableMSicknessEffectEvent (true));
				EventController.Instance.Publish (new PlayerSelectedEvent (playerName));
			} else {
				EventController.Instance.Publish (new EnableMSicknessEffectEvent (false));	// TODO: move these publishes elsewhere!!
			}

			// TODO: remove redundant checks!!
			if (mode == "BlinkTeleport" || mode == "BodySwitchTeleport") {		// TODO: move these checks to better location, GameController)
				EventController.Instance.Publish (new PlayerSelectedEvent (playerName));
			}
		}

	}

	private void HandleSetTarget(Transform target)
	{
		// If the game isn't over set the destination of the AI controlling the character and the trail showing its path.
		//if (m_IsGameOver)
		//	return;
		//m_AiCharacter.SetTarget(target.position);
		m_AiCharacter.SetTarget(target.position);
		//m_AgentTrail.SetDestination();
	}
}
