using UnityEngine;
using System.Collections;

public class DirtParticleSwitcher : MonoBehaviour {
	
	public Material[] materials;
	private tk2dTileMap tilemap;
	private int curDirtType = 0;
	
	void Start () {
		tilemap = GameObject.Find("TileMap").GetComponent<tk2dTileMap>();
	}
	
	private void SetDirtType(int i) {
		if(i != curDirtType) {
			particleSystem.renderer.material = materials[i];
			curDirtType = i;
		}
	}
	
	// This is a pretty derp way of doing it. Instead query the tile data
	void Update () {
		if(transform.position.y < 243) {
			SetDirtType(3);
		} else if(transform.position.y < 499) {
			SetDirtType(2);
		} else if(transform.position.y < 757) {
			SetDirtType(1);
		} else {
			SetDirtType(0);
		}
	}
}
