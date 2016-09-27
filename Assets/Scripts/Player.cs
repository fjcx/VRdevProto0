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
		if (isSelected == true) {
			Debug.Log ("playerName: " + playerName + " isSelected and moving");
			HandleSetTarget (evt.moveToTransform);
		}
	}

	private void OnModeUpdatedEvent(ModeUpdatedEvent evt) {
		mode = evt.newMode;

		if (isCameraHolder) {
			if (mode == "MotionSicknessMovement") {		// TODO: move these checks to better location
				EventController.Instance.Publish (new PlayerSelectedEvent (playerName));
				EventController.Instance.Publish (new EnableMSicknessEffectEvent (true));
			} else {
				EventController.Instance.Publish (new EnableMSicknessEffectEvent (false));	// TODO: move these publishes elsewhere!!
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
