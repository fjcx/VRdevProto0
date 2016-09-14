using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRStandardAssets.Utils;
using System;

public class SlidingMenu : MonoBehaviour {

	public GameObject recticle;
	public GameObject slidingMenu;
	public GameObject menuItemPrefab;
	[SerializeField]
	public List<MenuOptionData> menuOptions = new List<MenuOptionData>();
	[SerializeField] private VRInput m_VRInput;
	private List<GameObject> menuOptionObjects = new List<GameObject>();

	public float defaultYpos = 0f;
	public float panelSpacing = 2f;
	public float backZSpacing = 1f;
	private int selectedIndex = 0;
	public string selectedMode;	// should be enum

	private void OnEnable() {
		m_VRInput.OnSwipe += HandSwipe;
	}

	private void OnDisable() {
		m_VRInput.OnSwipe -= HandSwipe;
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
				slideMenuLeft();
			break;
		case VRInput.SwipeDirection.RIGHT:
				slideMenuRight();
			break;
		}
	}

	// Use this for initialization
	void Awake () {
		Quaternion rotation = Quaternion.Euler (5, 0, 0);

		for (int i = 0; i < menuOptions.Count; i++) {
			float zPos = Math.Abs((i - selectedIndex)) * backZSpacing;
			if (selectedIndex != i) {
				// push background elements back slightly more
				zPos++;
			}
			float xPos = (i - selectedIndex) * panelSpacing;
			GameObject mOpt = (GameObject)Instantiate(menuItemPrefab, Vector3.zero, rotation);
			mOpt.transform.parent = slidingMenu.transform;
			mOpt.transform.localPosition = new Vector3 (xPos, defaultYpos, zPos);


			Renderer rend = mOpt.GetComponent<Renderer>();
			if (selectedIndex != i) {
				rend.material.SetColor ("_Color", Color.gray);
			} else {
				rend.material.SetColor ("_Color", Color.white);
			}

			MenuOption menuOpt = mOpt.GetComponent<MenuOption> ();
			menuOpt.menuText.text = menuOptions [i].description;
			//mOpt.menuText.text = menuOpt.description;
			//mOpt.mode = menuOpt.mode;

			// keep reference to instantiated menuItems
			menuOptionObjects.Add (mOpt);
		}

		updateSelectedMode();
	}

	private void updateSelectedMode() {
		MenuOption menuOpt = menuOptionObjects[selectedIndex].GetComponent<MenuOption> ();
		selectedMode = menuOptions [selectedIndex].description;
	}

	private void slideMenuLeft () {
		if (selectedIndex > 0) {
			selectedIndex--;
			movePanels ();
		} else {
			// at edge of menu // could loop instead
		}
	}

	private void slideMenuRight () {
		if (selectedIndex < (menuOptionObjects.Count - 1)) {
			selectedIndex++;
			movePanels ();
		} else {
			// at edge of menu // could loop instead
		}
	}

	private void movePanels() {
		//float zPos = defaultZpos;
		//foreach (GameObject menuOpt in menuOptionObjects) {
		for (int i = 0; i < menuOptionObjects.Count; i++) {

			float zPos = Math.Abs((i - selectedIndex)) * backZSpacing;
			if (selectedIndex != i) {
				// push background elements back slightly more
				zPos++;
			}

			float xPos = (i - selectedIndex) * panelSpacing;
			menuOptionObjects[i].transform.localPosition = new Vector3 (xPos, defaultYpos, zPos);

			Renderer rend = menuOptionObjects[i].GetComponent<Renderer>();
			if (selectedIndex != i) {
				rend.material.SetColor ("_Color", Color.gray);
			} else {
				rend.material.SetColor ("_Color", Color.white);
			}
		}

		updateSelectedMode();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
