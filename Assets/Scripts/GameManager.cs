using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class GameManager : MonoBehaviour {
	
    // Static singleton property
    public static GameManager Instance { get; private set; }
	public enum GameState {Playing, Paused, Winning};
	
	public GameState State;
	
	public GameObject pauseMenu;
	public GameObject pauseButton;
	private Sequence pauseTween;
	
	private int nutrientsCollected = 0;
	private tk2dSpriteAnimator flower;
	
	void Awake () {
        Instance = this;
		State = GameState.Playing;
		SetupPauseTween();
	}
	
	void Start () {
		flower = GameObject.Find("Seed/Flower").GetComponent<tk2dSpriteAnimator>();
		flower.gameObject.SetActive(false);
	}
	
	
	#region pause menu handling
	private void SetupPauseTween() {
		pauseTween = new Sequence(new SequenceParms().AutoKill(false).UpdateType(UpdateType.TimeScaleIndependentUpdate));
		
		pauseTween.Append(HOTween.To(pauseMenu.transform, 1, new TweenParms()
			.Prop ("localPosition", Vector3.zero)
			.Ease(EaseType.EaseOutBounce)
			.UpdateType(UpdateType.TimeScaleIndependentUpdate)
		));
		pauseTween.AppendCallback(pauseTween.Pause);
		pauseTween.Append(HOTween.To(pauseMenu.transform, 0.5f, new TweenParms()
			.Prop ("localPosition", new Vector3(0,200,0))
			.Ease(EaseType.EaseOutQuad)
			.UpdateType(UpdateType.TimeScaleIndependentUpdate)
		));
	}
	
	public void Pause() {
		Time.timeScale = 0;
		State = GameState.Paused;
		pauseButton.SetActive(false);
		pauseTween.Rewind();
		pauseTween.Play();
	}
	
	public void UnPause() {
		Time.timeScale = 1;
		State = GameState.Playing;
		pauseButton.SetActive(true);
		pauseTween.Play();
	}
	
	public void ExitLevel() {
		Time.timeScale = 1;
		Application.LoadLevel("Level Select");
	}
	
	public void RestartLevel() {
		Time.timeScale = 1;
		Application.LoadLevel(Application.loadedLevel);
	}
	#endregion
	
	public void Win() {
		State = GameState.Winning;
		Time.timeScale = 0;
		
		Sequence winSequence = new Sequence(new SequenceParms().UpdateType(UpdateType.TimeScaleIndependentUpdate));
		winSequence.Append(HOTween.To(Camera.mainCamera.transform, 1, new TweenParms()
			.Prop("position", new Vector3(Camera.mainCamera.transform.position.x, flower.transform.position.y, Camera.mainCamera.transform.position.z))
			.Ease(EaseType.EaseOutQuad)
		));
		winSequence.AppendCallback(GrowFlower);
		winSequence.Play();
	}
	
	public void GrowFlower() {
		flower.gameObject.SetActive(true);
		flower.GetComponent<tk2dSpriteAnimator>().Play("Grow " + nutrientsCollected);
	}
	
	public void NutrientCollected(){
		nutrientsCollected++;
		Win ();
	}
	
	// Update is called once per frame
	void Update () {
		if(!GameObject.FindGameObjectWithTag("RootTip")) {
			RestartLevel();
		}
	}
}
