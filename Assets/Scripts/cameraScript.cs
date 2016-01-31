using UnityEngine;
using System.Collections;

public class cameraScript : MonoBehaviour {

	public Transform target;
	public float cameraDistance = -10f;

	// Use this for initialization
	void Start () {
		if (target == null) {
			target = GameObject.Find("player1").transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = target.position + new Vector3(0, 0, cameraDistance);
	}
}
