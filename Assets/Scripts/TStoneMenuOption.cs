using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;

public class TStoneMenuOption : MonoBehaviour {

	public GameObject tombstoneUI;

	[SerializeField] private Material m_NormalMaterial;                
	[SerializeField] private Material m_OverMaterial;                  
	//[SerializeField] private Material m_ClickedMaterial;               
	[SerializeField] private Material m_DoubleClickedMaterial;         
	[SerializeField] private VRInteractiveItem m_InteractiveItem;
	[SerializeField] private Renderer m_Renderer;

	public string mode;

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
	}


	private void OnDisable()
	{
		m_InteractiveItem.OnOver -= HandleOver;
		m_InteractiveItem.OnOut -= HandleOut;
	//	m_InteractiveItem.OnClick -= HandleClick;
		m_InteractiveItem.OnDoubleClick -= HandleDoubleClick;
	}


	//Handle the Over event
	private void HandleOver()
	{
		Debug.Log("Show over state");
		m_Renderer.material = m_OverMaterial;
	}


	//Handle the Out event
	private void HandleOut()
	{
		Debug.Log("Show out state");
		m_Renderer.material = m_NormalMaterial;
	}


	//Handle the Click event
	/*private void HandleClick()
	{
		Debug.Log("Show click state");
		m_Renderer.material = m_ClickedMaterial;
	}*/


	//Handle the DoubleClick event
	private void HandleDoubleClick()
	{
		Debug.Log("Show double click");
		m_Renderer.material = m_DoubleClickedMaterial;
		updateSelectedMode (mode);
	}

	private void updateSelectedMode (string selectedMode) {
		TombstoneUI tstoneUIScript = tombstoneUI.GetComponent<TombstoneUI>();
		tstoneUIScript.SelectMenuItem (selectedMode);
	}
}
