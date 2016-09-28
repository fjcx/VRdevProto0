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
	private bool canBlink = true;

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
		EventController.Instance.Subscribe<MoveEyeLidsEndEvent>(OnMoveEyeLidsEndEvent);

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
		EventController.Instance.UnSubscribe<MoveEyeLidsEndEvent>(OnMoveEyeLidsEndEvent);
		m_InteractiveItem.OnDoubleClick -= HandleDoubleClick;
		m_InteractiveItem.OnOver -= HandleOver;
		m_InteractiveItem.OnOut -= HandleOut;
	}

	private void HandleDoubleClick()
	{
		Debug.Log ("Clicking player here");
		if (!isCameraHolder) {
			EventController.Instance.Publish (new PlayerSelectedEvent (playerName));
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
		} else {
			Debug.Log ("DeSelected!" + playerName + "/" + evt.playerName);
			isSelected = false;
			selectedCircle.SetActive (false);
		}
	}

	private void OnMoveToEvent(MoveToEvent evt) {
		if (isSelected == true ) {
			if (mode == "FadePointTargeting") {
				// Blink Move to point
				Debug.Log ("trying to blink");
				if (canBlink) {
					canBlink = false;	// don't allow any sliding commands while in motion!!
					EventController.Instance.Publish (new MoveEyeLidsStartEvent ("FirstClose", evt.moveToTransform, -0.4f, 0.1f, 0f));
				}
			} else {
				// Move animation and travesal to point
				Debug.Log ("playerName: " + playerName + " isSelected and moving");
				HandleSetTarget (evt.moveToTransform);
			}
		}
	}

	private void OnMoveEyeLidsEndEvent(MoveEyeLidsEndEvent evt) {
		if (isSelected == true) {			// TODO: tidy up to have less conditional checks !!!
			Debug.Log ("EndPhase: " + evt.movePhase);
			switch (evt.movePhase) {
			case "FirstClose":
				EventController.Instance.Publish (new MoveEyeLidsStartEvent ("FirstOpen", evt.moveTo, 0.4f, 0.1f, 0.3f));
				break;
			case "FirstOpen":
				EventController.Instance.Publish (new MoveEyeLidsStartEvent ("SecondClose", evt.moveTo, -0.4f, 1.0f, 0.6f));
				break;
			case "SecondClose":
				transform.position = evt.moveTo.position;	// is this the best way to teleport?
				EventController.Instance.Publish (new MoveEyeLidsStartEvent ("SecondOpen", evt.moveTo, 0.4f, 0.3f, 0f));
				break;
			case "SecondOpen":
				canBlink = true;
				break;
			default:
				break;
			}
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

			if (mode == "FadePointTargeting") {		// TODO: move these checks to better location, GameController)
				EventController.Instance.Publish (new PlayerSelectedEvent (playerName));
				EventController.Instance.Publish (new EyeLidsVisibleEvent (true));
			} else {
				EventController.Instance.Publish (new EyeLidsVisibleEvent (false));
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
