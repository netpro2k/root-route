using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
    // Static singleton property
    public static GameManager Instance { get; private set; }
	public enum GameState {Playing, Paused, Winning};
	
	public GameState State;
	
	private int nutrientsCollected = 0;
	
	void Awake () {
        Instance = this;
		State = GameState.Playing;
	}
	
	public void NutrientCollected(){
		nutrientsCollected++;
	}
	
	public void Win() {
		State = GameState.Winning;
		iTween.MoveTo(Camera.mainCamera.gameObject, new Vector3(0,0,0), 2);
	}
	
	// Update is called once per frame
	void Update () {
		if(!GameObject.FindGameObjectWithTag("RootTip")) {
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
