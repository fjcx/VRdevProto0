using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRStandardAssets.Utils;
using UnityEngine.UI;
using System;

public class StandingStoneUI : MonoBehaviour {

	public GameObject mainCamera;	// Used to position tombstones relative to direction mainCamera is looking
	public GameObject recticle;
	public GameObject tombstoneUI;
	public Text selectedModeTextPanel;
    public StandingStoneMenuOption[] childStones;

	//[SerializeField] public List<MenuOptionData> menuOptions = new List<MenuOptionData>();
	//private List<GameObject> menuOptionObjects = new List<GameObject>();
	[SerializeField] private VRInput m_VRInput;

	private string selectedMode;	// should be enum

	public Boolean canSlideUp = false;	// This is assuming menu is up when starting !!
	public Boolean canSlideDown = true;

	private float panelSlideDistance = 0.5f;
	private float panelSlideTime = 0.5f;

	private void OnEnable() {
        EventController.Instance.Subscribe<SlideStandingStonesEvent>(OnSlideStandingStonesEvent);
        m_VRInput.OnSwipe += HandSwipe;
	}

	private void OnDisable() {
        EventController.Instance.UnSubscribe<SlideStandingStonesEvent>(OnSlideStandingStonesEvent);
        m_VRInput.OnSwipe -= HandSwipe;
	}

    void Start()
    {
        childStones = GetComponentsInChildren<StandingStoneMenuOption>();
    }

	private void HandSwipe(VRInput.SwipeDirection swipeDir) {
		switch (swipeDir) {
		case VRInput.SwipeDirection.NONE:
			break;
		case VRInput.SwipeDirection.UP:
			SlideTombstonesUp ();
			break;
		case VRInput.SwipeDirection.DOWN:
			SlideTombstonesDown ();
			break;
		case VRInput.SwipeDirection.LEFT:
			break;
		case VRInput.SwipeDirection.RIGHT:
			break;
		}
	}

	// update positions of tombstones relative to the user
	// TODO: stop user movement !!! (until option selected)
	private void SlideTombstonesUp() {
		Debug.Log ("trying to slide stones up");
		if (canSlideUp) {
            MoveTombstonesToCameraPos ();
            float slideDist = mainCamera.transform.position.y - (panelSlideDistance * 2);
            StartCoroutine (SlideTombstones (slideDist));
		}
		canSlideDown = true;
	}

	// dismiss tombstone menu
	// TODO: stop user movement !!! (until option selected)
	private void SlideTombstonesDown() {
		Debug.Log ("trying to slide stones down");
		if (canSlideDown) {
            float slideDist = mainCamera.transform.position.y + panelSlideDistance;
            StartCoroutine (SlideTombstones (-slideDist));
		}
		canSlideUp = true;
    }

	private void MoveTombstonesToCameraPos() {
		transform.position = new Vector3 (mainCamera.transform.position.x, -panelSlideDistance, mainCamera.transform.position.z);
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, mainCamera.transform.eulerAngles.y, transform.eulerAngles.z);
	}

	private IEnumerator SlideTombstones(float distance) {
        // Show stones if they are hidden
        if (distance > 0)
        {
            foreach (StandingStoneMenuOption childStone in childStones)
            {
                childStone.ShowStone();
            }
        }
        canSlideUp = false;		// don't allow any sliding commands while in motion!!
		canSlideDown = false;
		Vector3 startPos = transform.position;
		Vector3 endPos = transform.position + new Vector3 (0, distance, 0);
		float startTime = Time.time;
		while(Time.time < startTime + panelSlideTime)
		{
			transform.position = Vector3.Lerp(startPos, endPos, (Time.time - startTime)/panelSlideTime);
			yield return null;
		}
		transform.position = endPos;
        // Hide standing stone child renderers (in case floating in air or such)
        if (distance < 0)
        {
            foreach (StandingStoneMenuOption childStone in childStones)
            {
                childStone.HideStone();
            }
        }

    }

	// TODO: make menuItem an enum
	public void SelectMenuItem(string menuItem) {
		selectedMode = menuItem;
		selectedModeTextPanel.text = "Mode: " + selectedMode;
    }

    private void OnSlideStandingStonesEvent(SlideStandingStonesEvent evt)
    {
        if ("down" == evt.slideStonesDirection) {
            SlideTombstonesDown();
        } else if ("up" == evt.slideStonesDirection) {
            SlideTombstonesUp();
        } else {
            Debug.Log(evt.slideStonesDirection + "is not a valid sliding direction!");
        }
    }

}
