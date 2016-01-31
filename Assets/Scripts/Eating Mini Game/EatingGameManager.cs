using UnityEngine;
using System.Collections;

public class EatingGameManager : MonoBehaviour {
	private static EatingGameManager instance = null;
	public static EatingGameManager Instance { get { return instance; } }

	private bool gyroInitialized = false;

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
		/*mGyro.updateInterval = 1.0f;
		Debug.Log("enabled: " + mGyro.enabled);
		Debug.Log("updateInterval: " + mGyro.updateInterval);*/
	}

	void Update(){
		/*Debug.Log("attitude: " + mGyro.attitude);
		Debug.Log("gravity: " + mGyro.gravity);
		Debug.Log("rotationRate: " + mGyro.rotationRate);
		Debug.Log("rotationRateUnbiased: " + mGyro.rotationRateUnbiased);
		Debug.Log("userAcceleration: " + mGyro.userAcceleration);*/

		// Debug.Log("the Quat: " + Get());
	}

	public bool HasGyroscope{
		get{
			return SystemInfo.supportsGyroscope;
		}
	}

	public Quaternion Get(){
		if(!gyroInitialized){
			InitGyro();
		}
		return HasGyroscope
			? ReadGyroScopeRotation()
				: Quaternion.identity;
	}

	private void InitGyro(){
		if(HasGyroscope){
			Input.gyro.enabled = true; //enable the gyroscope
			Input.gyro.updateInterval = 0.0167f; //the highest update value (60 Hz)
		}
		gyroInitialized = true;
	}

	private Quaternion ReadGyroScopeRotation(){
		return new Quaternion(0.5f, 0.5f, -0.5f, 0.05f) * Input.gyro.attitude * new Quaternion(0, 0, 1, 0);
	}
}
