using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using VRStandardAssets.Utils;

public class GameController : MonoBehaviour {

	[SerializeField] private VRInput m_VRInput;
	private bool isHidden = false;
	public GameObject slidingMenu;
	public Text currentModeText;
	private string currentMode;

	private void OnEnable() {
		m_VRInput.OnSwipe += HandSwipe;
		m_VRInput.OnClick += HandleClick;	// perhaps should disable when menu is not visible
	}

	private void OnDisable() {
		m_VRInput.OnSwipe -= HandSwipe;
		m_VRInput.OnClick -= HandleClick;	// perhaps should disable when menu is not visible
	}

	private void HandleClick() {
		Debug.Log ("Clicked" + isHidden);
		if (!isHidden) {
			UpdateCurrentMode();
			HideSlidingMenu();
			isHidden = true;
		}
	}

	private void HideSlidingMenu() {
		slidingMenu.SetActive (false);
	}

	private void ShowSlidingMenu() {
		slidingMenu.SetActive (true);
	}

	private void HandSwipe(VRInput.SwipeDirection swipeDir) {
		switch (swipeDir) {
		case VRInput.SwipeDirection.NONE:
			break;
		case VRInput.SwipeDirection.UP:
			break;
		case VRInput.SwipeDirection.DOWN:
			break;
		case VRInput.SwipeDirection.LEFT:
		case VRInput.SwipeDirection.RIGHT:
			if (isHidden) {
				ShowSlidingMenu();
				isHidden = false;
			}
			break;
		}
	}

	private void UpdateCurrentMode() {
		SlidingMenu sMenu = slidingMenu.GetComponent<SlidingMenu>();
		currentMode = sMenu.selectedMode;
		currentModeText.text = "Mode: " + currentMode;
	}

	// Use this for initialization
	void Start () {
		UpdateCurrentMode();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
