using UnityEngine;
using System.Collections;

public class CameraHandler : MonoBehaviour {
	
	tk2dCamera tkCam;
	public float topOffset;
	// Use this for initialization
	void Awake () {
		tkCam = GetComponent<tk2dCamera>();
	}
	
	// Update is called once per frame
	void Update () {
		GameObject target = GameObject.FindGameObjectWithTag("RootTip");
		if(target && GameManager.Instance.State == GameManager.GameState.Playing) {
			transform.position = new Vector3(transform.position.x, Mathf.Floor(target.transform.position.y) - tkCam.nativeResolutionHeight + topOffset, transform.position.z);
		}
	}
}
