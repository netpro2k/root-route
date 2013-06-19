using UnityEngine;
using System.Collections;

public class Nutrient : MonoBehaviour {
	
	public AudioClip pickupSound;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) {
		if(other.CompareTag("RootTip")) {
			AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position);
			Destroy(gameObject);
		}
	}
}
