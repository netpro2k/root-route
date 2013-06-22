using UnityEngine;
using System.Collections;

public class WinZone : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		if(other.CompareTag("RootTip")) {
			GameManager.Instance.Win();
		}
	}
}
