using UnityEngine;
using System.Collections;

public class TileLighting : MonoBehaviour {
	
	public bool update = false;
	
	private tk2dSprite sprite;
	private tk2dTileMap tilemap;
	
	// Use this for initialization
	void Start () {
		tilemap = GameObject.Find("TileMap").GetComponent<tk2dTileMap>();
		sprite = GetComponent<tk2dSprite>();
		sprite.color = SafeGetColorAtPosition();
	}
	
	private Color SafeGetColorAtPosition() {
		try {
			return tilemap.GetInterpolatedColorAtPosition(transform.position);
		} catch (System.IndexOutOfRangeException e) {
			Debug.LogWarning(e);
			return sprite.color;
		}
	}
	
	
	// Update is called once per frame
	void Update () {
		if(update){
			sprite.color = Color.Lerp(sprite.color, SafeGetColorAtPosition() , 0.1f);
		}
	}
}
