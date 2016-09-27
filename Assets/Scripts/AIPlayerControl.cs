using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (NavMeshAgent))]
[RequireComponent(typeof (ThirdPersonCharacter))]
public class AIPlayerControl : MonoBehaviour
{
    public NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
    public ThirdPersonCharacter character { get; private set; } // the character we are controlling
    //public Transform target;                                    // target to aim for
	public Vector3 targetPos;
	private Boolean isTravelling = false;

    private void Start()
    {
        // get the components on the object we need ( should not be null due to require component so no need to check )
        agent = GetComponentInChildren<NavMeshAgent>();
        character = GetComponent<ThirdPersonCharacter>();

        agent.updateRotation = false;
        agent.updatePosition = true;
    }


    private void Update()
    {
		if (isTravelling != false) {
			agent.SetDestination (targetPos);
		}

		if (agent.remainingDistance > agent.stoppingDistance) {
			character.Move (agent.desiredVelocity, false, false);
		} else {
			isTravelling = false;
			character.Move (Vector3.zero, false, false);
		}
    }


    //public void SetTarget(Transform target)
	public void SetTarget(Vector3 targetPos)
    {
		isTravelling = true;
		this.targetPos = targetPos;
    }
}

