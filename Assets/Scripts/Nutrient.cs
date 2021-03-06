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
			AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position, 0.5f);
			GameManager.Instance.NutrientCollected();
			Destroy(gameObject);
		}
	}
}
