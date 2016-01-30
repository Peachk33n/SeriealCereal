using UnityEngine;
using System.Collections;

public class EatingGameManager : MonoBehaviour {
	private static EatingGameManager instance = null;
	public static EatingGameManager Instance { get { return instance; } }

	private Gyroscope mGyro = new Gyroscope();


	void Awake(){
		if(instance != null && instance != this)
		{
			Destroy(this.gameObject);
			return;
		}
		else
		{
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}

	void Start(){
		mGyro.
	}
}
