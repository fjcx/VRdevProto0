using UnityEngine;
using System.Collections;

public class HomingTrail : MonoBehaviour {

	public float pSphereVelocity = 500f;
	public float turn = 0.01f;
	//public GameObject homingTrail;
	public GameObject pSphere;
	public ParticleSystem smokePrefab;
	public Transform target;
	public bool isDeactivated = false;
	//public float fuseDelay;
	//public AudioClip soundClip;

	public void Fire () {
		//homingTrail = transform.GetComponent<Rigidbody>();
		//yield return new WaitForSeconds(fuseDelay);
		//AudioSource.PlayClipAtPoint(soundClip, transform.position);

		/*float distance = Mathf.Infinity;
		float diff = (target.position - transform.position).sqrMagnitude;
		if (diff < distance) {
			distance = diff;
		}*/

	}

	public void Start() {

	}

	public void Update() {
		if (target == null || isDeactivated == true) {
			return;		
		}

		float diff = (target.position + new Vector3 (0f, 1f, 0f) - transform.position).sqrMagnitude;

		if (0.25f < diff) {
			//float moveStep = pSphereVelocity * Time.deltaTime;		// TODO: add back in deltaTime !!
			// adjusting to 1f above target (as assuming target is at feet)
			transform.position = Vector3.MoveTowards (transform.position, target.position + new Vector3 (0f, 1f, 0f), 0.1f);

			Vector3 targetDir = target.position + new Vector3 (0f, 1f, 0f) - transform.position;
			float step = pSphereVelocity * Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards (transform.forward, targetDir, step, 0.0F);
			Debug.DrawRay (transform.position, newDir, Color.red);
			transform.rotation = Quaternion.LookRotation (newDir);
		} else {
			// Hit target or is too close !!
			StartCoroutine(OnHitTarget());
		}

	}

	public void FixedUpdate() {
		/*if (target == null || homingTrail == null) {
			return;		
		}

		float step = pSphereVelocity;
		transform.position = Vector3.MoveTowards (transform.position, target.position, step);

		homingTrail.velocity = transform.forward * pSphereVelocity;

		Quaternion targetRotation = Quaternion.LookRotation (target.position - transform.position);
		homingTrail.MoveRotation (Quaternion.RotateTowards(transform.rotation, targetRotation, turn));*/
	}

	public IEnumerator OnHitTarget() {
		Debug.Log ("Collided!");
		smokePrefab.emissionRate = 0.0f;
		Destroy (pSphere.gameObject);
		EventController.Instance.Publish (new SwitchBodiesEvent (target));
		yield return new WaitForSeconds(4);
		Destroy (gameObject);
	}
}
