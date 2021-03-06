using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class GameManager : MonoBehaviour {
	
    // Static singleton property
    public static GameManager Instance { get; private set; }
	public enum GameState {Playing, Paused, Winning, Losing};
	
	public GameState State;
	
	public GameObject pauseMenu;
	public GameObject pauseButton;
	private Tweener pauseInTween;
	private Tweener pauseOutTween;
	
	public GameObject levelCompleteMenu;
	private Tweener levelCompleteInTween;
	
	public GameObject levelFailMenu;
	private Tweener levelFailInTween;
	
	public AudioClip[] growSounds;
	
	private int nutrientsCollected = 0;
	private tk2dSpriteAnimator flower;
	private tk2dCamera tkCam;
	
	void Awake () {
        Instance = this;
		State = GameState.Playing;
		SetupPauseTween();
		SetupLevelCompleteTween();
		SetupLevelFailTween();
	}
	
	void Start () {
		flower = GameObject.Find("Seed/Flower").GetComponent<tk2dSpriteAnimator>();
		tkCam = Camera.mainCamera.GetComponent<tk2dCamera>();
	}
	
	
	#region pause menu handling
	private void SetupPauseTween() {
		pauseInTween = HOTween.To(pauseMenu.transform, 1, new TweenParms()
			.Prop ("localPosition", Vector3.zero)
			.Ease(EaseType.EaseOutBounce)
			.UpdateType(UpdateType.TimeScaleIndependentUpdate)
			.AutoKill(false)
			.Pause()
		);
		
		pauseOutTween = HOTween.To(pauseMenu.transform, 0.5f, new TweenParms()
			.Prop ("localPosition", new Vector3(0,200,0))
			.Ease(EaseType.EaseOutQuad)
			.UpdateType(UpdateType.TimeScaleIndependentUpdate)
			.AutoKill(false)
			.Pause()
		);
	}
	
	public void Pause() {
		State = GameState.Paused;
		Time.timeScale = 0;
		pauseButton.SetActive(false);
		
		pauseInTween.Rewind();
		pauseInTween.Play();
	}
	
	public void UnPause() {
		State = GameState.Playing;
		Time.timeScale = 1;
		pauseButton.SetActive(true);
		
		pauseOutTween.Rewind();
		pauseOutTween.Play();
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
	
	private void SetupLevelCompleteTween() {
		levelCompleteInTween = HOTween.To(levelCompleteMenu.transform, 1, new TweenParms()
			.Prop ("localPosition", Vector3.zero)
			.Ease(EaseType.EaseOutBounce)
			.UpdateType(UpdateType.TimeScaleIndependentUpdate)
			.AutoKill(false)
			.Pause()
		);
	}
	
	private void SetupLevelFailTween() {
		levelFailInTween = HOTween.To(levelFailMenu.transform, 1, new TweenParms()
			.Prop ("localPosition", Vector3.zero)
			.Ease(EaseType.EaseOutBounce)
			.UpdateType(UpdateType.TimeScaleIndependentUpdate)
			.AutoKill(false)
			.Pause()
		);
	}
	
	public void Win() {
		State = GameState.Winning;
		Time.timeScale = 0;
		pauseButton.SetActive(false);
		
		SaveManager.Instance.SaveNutrientsForCurrentLevel(nutrientsCollected);
		
		Sequence winSequence = new Sequence(new SequenceParms().UpdateType(UpdateType.TimeScaleIndependentUpdate));
		winSequence.Append(HOTween.To(Camera.mainCamera.transform, 3, new TweenParms()
			.Prop("position", new Vector3(Camera.mainCamera.transform.position.x, flower.transform.position.y - tkCam.nativeResolutionHeight/2, Camera.mainCamera.transform.position.z))
			.Ease(EaseType.EaseOutQuad)
		));
		winSequence.AppendCallback(GrowFlower);
		winSequence.AppendInterval(1.5f);
		winSequence.Append(levelCompleteInTween);
		
		winSequence.Play();
	}
	
	public void Lose() {
		State = GameState.Losing;
		Time.timeScale = 0;
		pauseButton.SetActive(false);
		
		levelFailInTween.Play();
	}
	
	public void NextLevel() {
		if(Application.loadedLevel == 10) {
			return;
		}
		
		Time.timeScale = 1;
		Application.LoadLevel(Application.loadedLevel + 1);
	}
	
	public void GrowFlower() {		
		flower.GetComponent<tk2dSpriteAnimator>().Play("Grow " + nutrientsCollected);
		Time.timeScale = 1;
		AudioSource.PlayClipAtPoint(growSounds[nutrientsCollected], flower.transform.position, 0.4f);
		Time.timeScale = 0;
	}
	
	public void NutrientCollected(){
		nutrientsCollected++;
	}
	
	// Update is called once per frame
	void Update () {
		if(!GameObject.FindGameObjectWithTag("RootTip")) {
			Lose();
		}
//		
//		if(Input.GetKeyDown(KeyCode.Space)) {
//			Application.CaptureScreenshot("screenshot.png", 5);
//		}
	}
}
