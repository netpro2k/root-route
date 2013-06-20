using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
    // Static singleton property
    public static GameManager Instance { get; private set; }
	public enum GameState {Playing, Paused, Winning};
	
	public GameState State;
	
	private int nutrientsCollected = 0;
	private tk2dSpriteAnimator flower;
	
	void Awake () {
        Instance = this;
		State = GameState.Playing;
	}
	
	void Start () {
		flower = GameObject.Find("Seed/Flower").GetComponent<tk2dSpriteAnimator>();
		flower.gameObject.SetActive(false);
	}
	
	public void NutrientCollected(){
		nutrientsCollected++;
	}
	
	public void Win() {
		State = GameState.Winning;
		
		iTween.MoveTo(Camera.mainCamera.gameObject, new Vector3(Camera.mainCamera.transform.position.x, flower.transform.position.y, Camera.mainCamera.transform.position.z), 1);
		GrowFlower();
	}
	
	public void GrowFlower() {
		flower.gameObject.SetActive(true);
		flower.GetComponent<tk2dSpriteAnimator>().Play("Grow " + nutrientsCollected);
	}
	
	// Update is called once per frame
	void Update () {
		if(!GameObject.FindGameObjectWithTag("RootTip")) {
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
