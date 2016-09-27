using System;
using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;

public class FloorTargeting : MonoBehaviour {

	public event Action<Transform> OnTargetSet;                     // This is triggered when a destination is set.

	[SerializeField] private Reticle m_Reticle;                     // This is used to reference the position and use it as the destination.
	[SerializeField] private VRInteractiveItem m_InteractiveItem;   // The VRInteractiveItem on the target area, used to detect double clicks on the target area.

	private bool m_Active;                                          // This determines whether the character can be given targets or not.

	private void OnEnable()
	{
		m_InteractiveItem.OnDoubleClick += HandleDoubleClick;
	}


	private void OnDisable()
	{
		m_InteractiveItem.OnDoubleClick -= HandleDoubleClick;
	}


	public void Activate ()
	{
		m_Active = true;
	}


	public void Deactivate ()
	{
		m_Active = false;
	}


	private void HandleDoubleClick()
	{
		Debug.Log ("Clicking floor here");
		EventController.Instance.Publish (new MoveToEvent (m_Reticle.ReticleTransform));
		//if (m_Active && OnTargetSet != null) {
			// If target setting is active and there are subscribers to OnTargetSet, call it.

			//OnTargetSet (m_Reticle.ReticleTransform);
		//}
	}
}
