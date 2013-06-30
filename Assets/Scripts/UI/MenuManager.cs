using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class MenuManager : MonoBehaviour {
	
	public GameObject worldSelectContainer;
	public GameObject[] worldSelectButtons;
	public tk2dTextMesh worldSelectTitle;
	private Sequence worldSelectTween;
	
	public GameObject levelSelectContainer;
	public LevelSelectButton[] levelSelectButtons;
	public tk2dTextMesh levelSelectTitle;
	private Sequence levelSelectTween;
	
	public AudioClip transitionSound;
	
	private int selectedWorld = -1;
	
	void Awake () {
		SetupWorldSelectTween ();
		SetupLevelSelectTween ();
	}
	
	// Use this for initialization
	void Start ()
	{	
		worldSelectContainer.transform.position = Vector3.zero;
		levelSelectContainer.transform.position = new Vector3(0,999,0);
		ShowWorldSelect ();
	}

	void SetupWorldSelectTween ()
	{
		worldSelectTween = new Sequence(new SequenceParms().AutoKill(false));
		for (int i = 0; i < worldSelectButtons.Length; i++) {
			var button = worldSelectButtons[i];
//			button.GetComponent<tk2dUIUpDownDisableButton>().Disabled = i > 0;
			button.GetComponent<tk2dUIItem>().OnClickUIItem += WorldSelected;
			worldSelectTween.Insert(1, HOTween.From(button.transform, 0.5f, new TweenParms()
				.Prop("position", new Vector3(button.transform.position.x, i % 2 == 0 ? 235 : -75, 0))		
				.Ease(EaseType.EaseOutBack)
			));
		}
		worldSelectTween.Insert(1.3f, HOTween.From(worldSelectTitle.transform, 0.5f, new TweenParms()
			.Prop("position", new Vector3(worldSelectTitle.transform.position.x, 180, 0))
			.Ease(EaseType.EaseOutQuad)
		));
		worldSelectTween.InsertCallback(1, PlayTransitionSound);
	}

	void SetupLevelSelectTween ()
	{
		levelSelectTween = new Sequence(new SequenceParms().AutoKill(false));
		for (int i = 0; i < levelSelectButtons.Length; i++) {
			var button = levelSelectButtons[i];
			button.GetComponent<tk2dUIItem>().OnClickUIItem += levelSelected;
			button.SetWorldAndLevel(0,i);
			levelSelectTween.Insert(0, HOTween.From(button.transform, 0.5f, new TweenParms()
				.Prop("position", new Vector3(280,button.transform.position.y, 0))
				.Ease(EaseType.EaseOutBack)
			));
		}
		
		levelSelectTween.Insert(0, HOTween.From(levelSelectTitle.transform, 0.5f, new TweenParms()
			.Prop("position", new Vector3(levelSelectTitle.transform.position.x, 180, 0))
			.Ease(EaseType.EaseOutQuad)
		));
	}
	
	public void PlayTransitionSound()
	{
		AudioSource.PlayClipAtPoint(transitionSound, Camera.main.transform.position);
	}
	
	public void ShowWorldSelect ()
	{	
		worldSelectTween.Play();
	}	
	
	public void HideWorldSelect ()
	{
		worldSelectTween.PlayBackwards();
	}
	
	public void ShowLevelSelect ()
	{
		levelSelectContainer.transform.position = Vector3.zero;
		
		levelSelectTween.Play();
	}

	void levelSelected (tk2dUIItem uiItem)
	{
		LevelSelectButton btn = uiItem.GetComponent<LevelSelectButton>();
		StartCoroutine(animateToLevel(btn.world+1, btn.level+1));
	}
	
	void WorldSelected (tk2dUIItem uiItem)
	{
//		LevelSelectButton btn = uiItem.GetComponent<LevelSelectButton>();
//		Debug.Log ("LEVEL" + btn.levelNumber);
		StartCoroutine(animateToLevelSelect(1));
	}
	
	IEnumerator animateToLevelSelect(int world) {
		HideWorldSelect();
		yield return new WaitForSeconds(0.5f);
		ShowLevelSelect();
	}
	
	IEnumerator animateToLevel(int world, int level) {
		levelSelectTween.PlayBackwards();
		PlayTransitionSound();
		yield return StartCoroutine(levelSelectTween.WaitForRewind());
		worldSelectTween.Kill();
		levelSelectTween.Kill();
		Application.LoadLevel("Level " + world + "-" + level);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
