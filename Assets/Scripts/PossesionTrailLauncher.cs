using UnityEngine;
using System.Collections;

public class PossesionTrailLauncher : MonoBehaviour {

	public GameObject pTrail;
	private GameObject currentLaunched;
	//private bool isDestroyedInFlight = false;

	public void Launch (Transform target) {
		if (currentLaunched != null) {
			GameObject toBeDestroyed = currentLaunched;
			currentLaunched = null;
			HomingTrail destroyTrail = toBeDestroyed.GetComponent<HomingTrail> ();
			destroyTrail.isDeactivated = true;
			StartCoroutine (DestroyInFlightTrail(toBeDestroyed, destroyTrail));
		}

		currentLaunched = (GameObject) Instantiate (pTrail, transform.position - new Vector3(0f, 1f, 0f), transform.rotation);
		HomingTrail homingTrail = pTrail.GetComponent<HomingTrail>();
		homingTrail.target = target;
		homingTrail.Fire ();
	}

	public IEnumerator DestroyInFlightTrail(GameObject toBeDestroyed, HomingTrail destroyTrail) {
		Debug.Log ("Destroying in flight sphere");
		destroyTrail.smokePrefab.emissionRate = 0.5f;
		Destroy (destroyTrail.pSphere.gameObject);
		yield return new WaitForSeconds(4);
		Destroy (toBeDestroyed);
	}

}
