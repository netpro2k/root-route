using UnityEngine;
using System.Collections;

public class RootTip : MonoBehaviour {
	
	public float speed;
	public GameObject segmentPrefab;
	public tk2dSpriteAnimator cooldownAnim;
	
	public AudioClip crashSound;
	
	private tk2dTileMap tilemap;
	[HideInInspector] public tk2dSprite sprite;
	
	void Start () {
		sprite = GetComponent<tk2dSprite>();
		tilemap = GameObject.Find("TileMap").GetComponent<tk2dTileMap>();
	}
	
	void Awake () {
		rigidbody.velocity = Vector3.down * speed;
		UpdateSegmentSpawnRate(16.0f/speed);
	}
	
	void Update () {
	
	}
	
	void OnMouseDown() {
		// TODO do this in a less derp way
		if(cooldownAnim.CurrentFrame >= cooldownAnim.CurrentClip.frames.Length-1) {
			Split ();
		}
	}
	
    void OnCollisionEnter(Collision collision) {
        foreach (ContactPoint contact in collision.contacts) {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
			
			int tileId = tilemap.GetTileIdAtPosition(contact.point,0);
			tk2dRuntime.TileMap.TileInfo tile = tilemap.GetTileInfoForTileId(tileId);
			Debug.Log (tile.stringVal);
			if(tile.stringVal == "brick" || tile.stringVal == "") {
				Curl();
				break;
			} else if (tile.stringVal == "water") {
				Application.LoadLevel(Application.loadedLevel);
			}
        }
    }
		
	GameObject SpawnSegment(){
		return SpawnSegment("Grow" + Random.Range(1,3));
	}
	
	GameObject SpawnSegment(string type){
		GameObject segment = (GameObject) Instantiate(segmentPrefab, transform.position, transform.rotation);
		segment.transform.Translate(0,0,2);
		segment.GetComponent<tk2dSpriteAnimator>().Play(type);
		return segment;
	}
	
	public RootTip Split() {
		// Update this tip's orientation
		if(transform.eulerAngles.z == 0) {
			GameObject junction = SpawnSegment("Y Junction");
			junction.transform.Translate(0,0,-1);
			
			rigidbody.velocity = new Vector3(-1 * speed, -1 * speed, 0);	
			transform.eulerAngles = new Vector3(0,0,-45);
			UpdateSegmentSpawnRate(11f/speed);
		} else {
			tk2dSprite junction = SpawnSegment("T Junction").GetComponent<tk2dSprite>();
			junction.transform.Translate(0,0,-1);
			junction.FlipX = this.sprite.FlipX;
		}
		
		cooldownAnim.PlayFrom(cooldownAnim.CurrentClip, 0);
		cooldownAnim.transform.rotation = Quaternion.identity;
		
		// Spawn the other tip		
		RootTip newTip = ((GameObject) Instantiate(this.gameObject)).GetComponent<RootTip>();	
		newTip.transform.eulerAngles = new Vector3(0,0,-transform.eulerAngles.z);
		newTip.sprite.FlipX = newTip.transform.eulerAngles.z > 180;
		Vector3 vel = newTip.rigidbody.velocity;
		vel.x = rigidbody.velocity.x * -1;
		newTip.rigidbody.velocity = vel;
		newTip.UpdateSegmentSpawnRate(11f/speed);
		newTip.cooldownAnim.transform.rotation = Quaternion.identity;
		
		return this;
	}
	
	public void UpdateSegmentSpawnRate(float newRate) {
		Debug.Log (newRate);
		CancelInvoke("SpawnSegment");
		InvokeRepeating("SpawnSegment", newRate, newRate);
	}
	
	public void Curl() {
		Destroy(gameObject);
		SpawnSegment("Curl");
		AudioSource.PlayClipAtPoint(crashSound,transform.position);
	}
}
