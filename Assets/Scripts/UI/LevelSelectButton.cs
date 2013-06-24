using UnityEngine;
using System.Collections;

public class LevelSelectButton : MonoBehaviour {
	
	public int levelNumber = 0;
	
	tk2dTextMesh[] labels;
	
	// Use this for initialization
	void Awake () {
	}
	
	void Start () {
		labels = GetComponentsInChildren<tk2dTextMesh>();
		foreach (var label in labels) {
			label.text = "" + levelNumber;
			label.Commit();
		}
		GetComponent<tk2dUIUpDownDisableButton>().Disabled = levelNumber > 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
