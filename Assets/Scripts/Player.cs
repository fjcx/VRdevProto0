using System;
using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;		// if updating use other namespace

public class Player : MonoBehaviour {
	
	[SerializeField] private FloorTargeting m_FloorTargeting;                 // This triggers an event when the target is set. 

	private AICharacterControl m_AiCharacter; 

	private void Awake()
	{
		// Setup references.
		m_AiCharacter = GetComponent<AICharacterControl>();
	}


	private void OnEnable ()
	{
		m_FloorTargeting.OnTargetSet += HandleSetTarget;
	}


	private void OnDisable()
	{
		m_FloorTargeting.OnTargetSet -= HandleSetTarget;
	}

	private void HandleSetTarget(Transform target)
	{
		// If the game isn't over set the destination of the AI controlling the character and the trail showing its path.
		//if (m_IsGameOver)
		//	return;

		//m_AiCharacter.SetTarget(target.position);
		m_AiCharacter.SetTarget(target);
		//m_AgentTrail.SetDestination();
	}
}
