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
	private Tweener pauseInTween;
	private Tweener pauseOutTween;
	
	public GameObject levelCompleteMenu;
	private Tweener levelCompleteInTween;
	private Tweener levelCompleteOutTween;
	
	private int nutrientsCollected = 0;
	private tk2dSpriteAnimator flower;
	private tk2dCamera tkCam;
	
	void Awake () {
        Instance = this;
		State = GameState.Playing;
		SetupPauseTween();
		SetupLevelCompleteTween();
	}
	
	void Start () {
		flower = GameObject.Find("Seed/Flower").GetComponent<tk2dSpriteAnimator>();
		flower.gameObject.SetActive(false);
		
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
		Time.timeScale = 0;
		State = GameState.Paused;
		pauseButton.SetActive(false);
		pauseInTween.Rewind();
		pauseInTween.Play();
	}
	
	public void UnPause() {
		Time.timeScale = 1;
		State = GameState.Playing;
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
		
		levelCompleteOutTween = HOTween.To(levelCompleteMenu.transform, 0.5f, new TweenParms()
			.Prop ("localPosition", new Vector3(0,200,0))
			.Ease(EaseType.EaseOutQuad)
			.UpdateType(UpdateType.TimeScaleIndependentUpdate)
			.AutoKill(false)
			.Pause()
		);
	}
	
	
	public void Win() {
		State = GameState.Winning;
		Time.timeScale = 0;
		pauseButton.SetActive(false);
		
		SaveManager.Instance.SaveNutrientsForLevel(0,0,nutrientsCollected);
		
		Sequence winSequence = new Sequence(new SequenceParms().UpdateType(UpdateType.TimeScaleIndependentUpdate));
		winSequence.Append(HOTween.To(Camera.mainCamera.transform, 3, new TweenParms()
			.Prop("position", new Vector3(Camera.mainCamera.transform.position.x, flower.transform.position.y - tkCam.nativeResolutionHeight/2, Camera.mainCamera.transform.position.z))
			.Ease(EaseType.EaseOutQuad)
		));
		winSequence.AppendCallback(GrowFlower);
		winSequence.AppendInterval(1.5f);
		winSequence.Append (levelCompleteInTween);
		
		winSequence.Play();
	}
	
	public void NextLevel() {
		Time.timeScale = 1;
		Application.LoadLevel(Application.loadedLevel + 1);
	}
	
	public void GrowFlower() {
		flower.gameObject.SetActive(true);
		flower.GetComponent<tk2dSpriteAnimator>().Play("Grow " + nutrientsCollected);
	}
	
	public void NutrientCollected(){
		nutrientsCollected++;
	}
	
	// Update is called once per frame
	void Update () {
		if(!GameObject.FindGameObjectWithTag("RootTip")) {
			RestartLevel();
		}
	}
}
