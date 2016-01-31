using UnityEngine;
using System.Collections;

public class itemScript : MonoBehaviour {

	// I [Colin] added this enum so we can use the same script while distinguishing between different items (e.g. I use it to play different sounds when different items are acted upon).
	public enum itemType {spoon, cerealBox, bowl, milk, fridge};

	public itemType theItem;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void ActedUpon() {
		Debug.Log("I was acted upon by the player!");
		// Do stuff
		switch (theItem) {
		case itemType.bowl:
			AudioMaster.instance.PlayAudioPackage(AudioMaster.instance.bowlClink, transform.position);
			break;
		case itemType.cerealBox:
			AudioMaster.instance.PlayAudioPackage(AudioMaster.instance.cerealBox, transform.position);
			break;
		case itemType.spoon:
			AudioMaster.instance.PlayAudioPackage(AudioMaster.instance.spoonClink, transform.position);
			break;
		case itemType.milk:
			AudioMaster.instance.PlayAudioPackage(AudioMaster.instance.pouringMilk, transform.position);
			break;
		case itemType.fridge:
			AudioMaster.instance.PlayAudioPackage(AudioMaster.instance.fridgeOpen, transform.position);
			break;
		default:
			AudioMaster.instance.PlayAudioPackage(AudioMaster.instance.bowlClink, transform.position);
			break;
		}
	}
}
