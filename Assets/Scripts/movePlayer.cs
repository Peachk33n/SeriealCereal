using UnityEngine;
using System.Collections;

public class movePlayer : MonoBehaviour {

	public float playerSpeed = 1f;

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

		if (Mathf.Abs(rateX.x) > 0 | Mathf.Abs(rateY.y) > 0) {
			Debug.Log ("Moving PC at rateX of: " + rateX.x + " and rateY of: " + rateY.y);
			AudioMaster.instance.PlayFootsteps(Mathf.Abs(rateX.x) + Mathf.Abs(rateY.y));
		}
	}
}
