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
		transform.Translate(Vector2.right * moveX * playerSpeed * Time.deltaTime);
		transform.Translate(Vector2.up * moveY * playerSpeed * Time.deltaTime);
	}
}
