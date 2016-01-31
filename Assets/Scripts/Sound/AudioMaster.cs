using UnityEngine;
using System.Collections;
using DG.Tweening;

[System.Serializable]
public class AudioPackage
{
	public AudioClip theClip;
	[Range(0f, 1f)]
	[Tooltip("Initial amplitude of the associated clip, where 1 = unity gain, 0.5 = -6dB gain, 0.25 = -12dB gain, etc.")]
	// [Tooltip("Volume = " + 20 * Mathf.Log(baseVolume, 10f) + "dB")]
	public float baseVolume = 1f;

	// Controls pausing/unpausing and mixer routing.
	// public sfxTypes sfxType = sfxTypes.Default;

	[Range(0f, 0.5f)]
	[Tooltip("Sets the range for the random value (minimum = 1) that base volume is divided by.")]
	public float volRandom = 0f;
	[Range(0f, 2f)]
	[Tooltip("Initial pitch (i.e. playback speed) of the associated clip, where 1 = normal, 0.5 = half, 2 = double, etc.")]
	public float basePitch = 1f;
	[Range(0f, 0.5f)]
	[Tooltip("Sets the total size of the random range above and below base pitch.")]
	public float pitchRandom = 0f;
	[Range(0f, 1f)]
	[Tooltip("The higher the value, the more the audio is affected by 3D position and spread.")]
	public float spatialBlend = 0f;

	[System.NonSerialized]
	public int voiceCount = 0;
	[Range(0,5)]
	public int voiceMax = 1;
	[Range(0f,6f)]
	public float voiceDuration = 0f;

//	[System.NonSerialized]
//	public Tweener volFade;

//	[System.NonSerialized]
	public AudioSource theSource;

	//		[Range(0f, 1f)]
	//		[Tooltip("Relative weight of this element when randomly selecting layers.")]
	//		public float layerProbability = 0f;
}


public class AudioMaster : MonoBehaviour {

	public static AudioMaster instance;

	// System.Collections.Generic.Queue<AudioSource> inactiveSources = new System.Collections.Generic.Queue<AudioSource>();

	Transform theTransform;

	public UnityEngine.Audio.AudioMixer theMixer;
	public UnityEngine.Audio.AudioMixerGroup theGroup;

	public AnimationCurve spreadCurve;
	// Spread affects the stereo width of the clip. At 0 degrees, the clip is effectively mono before 3D positioning.
	// At 180 degrees, the clip maintains its original stereo width, meaning the AudioSource's 3D position has no effect on panning.
	// At 360 degrees, the clip acts like 0 degrees, but with flipped panning. Sources from the left play louder in the right, and vice versa.
	// My first instinct is to draw the spreadCurve to decrease from 180 degrees (at a distance of zero) to ~20 degrees (at MasterDistanceMax).
	// The result will be sounds that narrow, but become increasingly directional, as their distance increases.

	[Header("Menus/UI")]

	public AudioPackage dialogueA;
	public AudioPackage dialogueB;

	[Header("In-Game")]

	public AudioPackage spoonClink;
	public AudioPackage bowlClink;
	public AudioPackage cerealBox;
	public AudioPackage pouringCereal;
	public AudioPackage pouringMilk;
	public AudioPackage chewing;
	public AudioPackage[] footsteps;
	public AudioPackage fridgeOpen;
	public AudioPackage fridgeClose;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			Destroy(this.gameObject);
		}

		theTransform = this.transform;

//		foreach (Transform t in theTransform)
//		{
//			if (t.gameObject.name == "MusicPlayer")
//			{
//				musicPlayer = t.gameObject.GetComponent<MusicPlayer>();
//			}
//		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// Press "Q" to test a sound!
		if (Input.GetKeyDown(KeyCode.Q)) {
			PlayAudioPackage(pouringCereal, theTransform.position);
		}
	}


	AudioSource CreateNewSource(AudioPackage thePackage) {

		string theName = (thePackage.theClip != null ? thePackage.theClip.name : "AudioSource");
		GameObject newSource = new GameObject(theName);
		newSource.transform.parent = theTransform;
		newSource.transform.localPosition = Vector3.zero;

		AudioSource theAudioSource = newSource.AddComponent<AudioSource>();
		theAudioSource.outputAudioMixerGroup = theGroup;
		theAudioSource.playOnAwake = false;
		theAudioSource.spread = 90f;
		// theAudioSource.spread = MasterSpread;
		// theAudioSource.SetCustomCurve(AudioSourceCurveType.Spread, spreadCurve);
		theAudioSource.rolloffMode = AudioRolloffMode.Logarithmic;
		// theAudioSource.minDistance = MasterDistanceMin;
		// theAudioSource.maxDistance = MasterDistanceMax;

		thePackage.theSource = theAudioSource;

		return theAudioSource;
	}

	AudioSource PlayAudioPackage(AudioPackage thePackage, Vector3 position, float volParam = 1f, float delay = 0f, float pan = 0f)
	{
		AudioSource theSource;

		if (thePackage.theSource != null) {
			theSource = thePackage.theSource;
		} else {
			theSource = CreateNewSource (thePackage);
		}

		if (thePackage.voiceCount < thePackage.voiceMax) {
			
				theSource.clip = thePackage.theClip;
				theSource.volume = ((thePackage.baseVolume * volParam) / Random.Range (1f, 1f + thePackage.volRandom));
				theSource.pitch = Random.Range (thePackage.basePitch - thePackage.pitchRandom / 2f, thePackage.basePitch + thePackage.pitchRandom / 2f);
				theSource.spatialBlend = thePackage.spatialBlend;
				theSource.panStereo = pan;
				theSource.transform.position = position;

				theSource.PlayDelayed (delay);

				++thePackage.voiceCount;
				// Debug.Log(thePackage.ToString() + " playing at voiceCount: " + thePackage.voiceCount);
				StartCoroutine(FreeVoice(thePackage));

				// float playTime = (thePackage.theClip.length / theSource.pitch) + delay;
				// Debug.Log("Audio package voiced for playTime (s): " + playTime);
				// theSource.gameObject.GetComponent<ActiveAudioSource> ().Invoke ("ReturnSourceToQueue", playTime);
				// StartCoroutine (NullPackage (thePackage, playTime));
		}
		return theSource;
	}

	// "Frees up" a voice within the given package, allowing the audio to be played again.
	IEnumerator FreeVoice(AudioPackage thePackage)
	{
		yield return new WaitForSeconds(thePackage.voiceDuration);
		if (thePackage.voiceCount > 0) {
			--thePackage.voiceCount;
		}
		yield return null;
	}

	Coroutine FootstepRoutine;
	int stepIndex = 0;

	public void PlayFootsteps(float rate) {
		if (FootstepRoutine == null) {
			FootstepRoutine = StartCoroutine (FootstepTimer(rate));
		}
	}

	IEnumerator FootstepTimer(float rate) {
		yield return new WaitForSeconds(0.01f / rate);
		PlayAudioPackage(footsteps[stepIndex], theTransform.position);
		stepIndex = (stepIndex + 1) % footsteps.Length;
		FootstepRoutine = null;
		yield return null;
	}
}
