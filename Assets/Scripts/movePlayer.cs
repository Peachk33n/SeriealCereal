using UnityEngine;
using System.Collections;

public class movePlayer : MonoBehaviour {

	public float playerSpeed = 1f;
	private float minItemDistance = 1f;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		float moveX = Input.GetAxisRaw("Horizontal");
		float moveY = Input.GetAxisRaw("Vertical");
		
        Vector2 rateX = Vector2.right * moveX * playerSpeed * Time.deltaTime;
        Vector2 rateY = Vector2.up * moveY * playerSpeed * Time.deltaTime;
        
        transform.Translate(rateX);
        transform.Translate(rateY);

        transform.Translate(Vector2.right * moveX * playerSpeed * Time.deltaTime);
		transform.Translate(Vector2.up * moveY * playerSpeed * Time.deltaTime);
		
        if (Input.GetMouseButton(0)) {
			// item selection using "mouse"
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
			if (hit.collider != null) { // if the raycast hit something
				var itemDistance = Vector2.Distance(transform.position, hit.collider.transform.position);
				Debug.Log("Trying to select " + hit.collider.transform.name 
					+ " which is " + itemDistance + " away");
				if (itemDistance <= minItemDistance) { // if select succesful
					Debug.Log("Acting upon: " + hit.collider.transform.name);
					// send the selected object "ActedUpon"
					hit.collider.gameObject.SendMessage("ActedUpon");
				}
			}
			// move player towards the current mouse position when mouse is down
			var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			targetPos.z = transform.position.z;
			transform.position = Vector3.MoveTowards(transform.position, targetPos, playerSpeed * Time.deltaTime);
            // TODO: Integrate mouse movement with AudioMaster
		}
        if (Mathf.Abs(rateX.x) > 0 | Mathf.Abs(rateY.y) > 0) {
			Debug.Log ("Moving PC at rateX of: " + rateX.x + " and rateY of: " + rateY.y);
			AudioMaster.instance.PlayFootsteps(Mathf.Abs(rateX.x) + Mathf.Abs(rateY.y));
		}
	}

	void ActedUpon() {
		Debug.Log("Cereal.");
	}
}
