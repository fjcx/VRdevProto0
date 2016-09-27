using UnityEngine;
using System.Collections;
using System;
using VRStandardAssets.Utils;

public class TStoneMenuOption : MonoBehaviour {

	[SerializeField] private Material m_NormalMaterial;                
	[SerializeField] private Material m_OverMaterial;                  
	//[SerializeField] private Material m_ClickedMaterial;               
	[SerializeField] private Material m_DoubleClickedMaterial;         
	[SerializeField] private VRInteractiveItem m_InteractiveItem;
	[SerializeField] private Renderer m_Renderer;

	public string mode;
	public Boolean isCurrentMode = false;

	private void Awake ()
	{
		m_Renderer.material = m_NormalMaterial;
	}


	private void OnEnable()
	{
		m_InteractiveItem.OnOver += HandleOver;
		m_InteractiveItem.OnOut += HandleOut;
	//	m_InteractiveItem.OnClick += HandleClick;
		m_InteractiveItem.OnDoubleClick += HandleDoubleClick;
		EventController.Instance.Subscribe<ModeUpdatedEvent>(OnModeUpdatedEvent);
	}


	private void OnDisable()
	{
		m_InteractiveItem.OnOver -= HandleOver;
		m_InteractiveItem.OnOut -= HandleOut;
	//	m_InteractiveItem.OnClick -= HandleClick;
		m_InteractiveItem.OnDoubleClick -= HandleDoubleClick;
		EventController.Instance.UnSubscribe<ModeUpdatedEvent>(OnModeUpdatedEvent);
	}

	private void Start() {
		if (isCurrentMode) {
			m_Renderer.material = m_DoubleClickedMaterial;
			updateSelectedMode ();
		}
	}

	private void OnModeUpdatedEvent(ModeUpdatedEvent evt) {
		if (evt.newMode == mode) {
			isCurrentMode = true;
		} else {
			isCurrentMode = false;
			m_Renderer.material = m_NormalMaterial;
		}
	}
		
	private void HandleOver()
	{
		Debug.Log("Show over state");
		m_Renderer.material = m_OverMaterial;
	}
		
	private void HandleOut()
	{
		Debug.Log("Show out state");
		if (isCurrentMode) {
			m_Renderer.material = m_DoubleClickedMaterial;
		} else {
			m_Renderer.material = m_NormalMaterial;
		}
	}

	private void HandleDoubleClick()
	{
		Debug.Log("Show double click");
		m_Renderer.material = m_DoubleClickedMaterial;
		updateSelectedMode ();
	}


	//Handle the Click event
	/*private void HandleClick()
	{
		Debug.Log("Show click state");
		m_Renderer.material = m_ClickedMaterial;
	}*/

	private void updateSelectedMode () {
		EventController.Instance.Publish (new ModeUpdatedEvent (mode));
	}
}
