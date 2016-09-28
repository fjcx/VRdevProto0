using UnityEngine;
using System.Collections;

public class EyeBlink : MonoBehaviour {

	public GameObject topEyeLid;
	public GameObject bottomEyeLid;

	private void OnEnable() {
		EventController.Instance.Subscribe<MoveEyeLidsStartEvent>(OnMoveEyeLidsStartEvent);
		EventController.Instance.Subscribe<EyeLidsVisibleEvent>(OnEyeLidsVisibleEvent);
	}

	private void OnDisable() {
		EventController.Instance.UnSubscribe<MoveEyeLidsStartEvent>(OnMoveEyeLidsStartEvent);
		EventController.Instance.UnSubscribe<EyeLidsVisibleEvent>(OnEyeLidsVisibleEvent);
	}

	private void OnEyeLidsVisibleEvent(EyeLidsVisibleEvent evt) {
		topEyeLid.SetActive (evt.isVisible);
		bottomEyeLid.SetActive (evt.isVisible);
	}

	private void OnMoveEyeLidsStartEvent(MoveEyeLidsStartEvent evt) {
		Debug.Log (evt.movePhase + ": blinking!");
		StartCoroutine (SlideEyeLids (evt.movePhase, evt.moveTo, evt.slideDistance, evt.blinkTime, evt.blinkWait));
	}

	private IEnumerator SlideEyeLids(string movePhase, Transform moveTo, float distance, float slideTime, float blinkWait) {
		Vector3 topStartPos = topEyeLid.transform.position;
		Vector3 bottomStartPos = bottomEyeLid.transform.position;
		Vector3 topEndPos = topEyeLid.transform.position + new Vector3 (0, distance, 0);
		Vector3 bottomEndPos = bottomEyeLid.transform.position - new Vector3 (0, distance, 0);

		float startTime = Time.time;
		while(Time.time < startTime + slideTime)
		{
			topEyeLid.transform.position = Vector3.Lerp(topStartPos, topEndPos, (Time.time - startTime)/slideTime);
			bottomEyeLid.transform.position = Vector3.Lerp(bottomStartPos, bottomEndPos, (Time.time - startTime)/slideTime);
			yield return null;
		}
		topEyeLid.transform.position = topEndPos;
		bottomEyeLid.transform.position = bottomEndPos;

		Debug.Log (movePhase + ": waiting!");
		yield return new WaitForSeconds(blinkWait);
		Debug.Log (movePhase + ": publishing!");
		EventController.Instance.Publish (new MoveEyeLidsEndEvent (movePhase, moveTo));
	}

}
