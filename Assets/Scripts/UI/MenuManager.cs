using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {
	
	public tk2dTextMesh titleLabel;
	public LevelSelectButton[] levelSelectButtons;
	public GameObject[] worldSelectButtons;
	
	public GameObject levelSelectContainer;
	public GameObject worldSelectContainer;
	
	private int selectedWorld = -1;
	
	// Use this for initialization
	void Start ()
	{	
		worldSelectContainer.transform.position = Vector3.zero;
		levelSelectContainer.transform.position = new Vector3(0,999,0);
		ShowWorldSelect ();
	}
	
	public void ShowWorldSelect ()
	{
		for (int i = 0; i < worldSelectButtons.Length; i++) {
			var button = worldSelectButtons[i];
			button.GetComponent<tk2dUIItem>().OnClickUIItem += WorldSelected;
			iTween.MoveFrom(button.gameObject, iTween.Hash(
				"position", new Vector3(button.transform.position.x, i % 2 == 0 ? 235 : -75, 0), 
				"time", 0.5, 
				"easetype", iTween.EaseType.easeOutBack,
				"delay", 1
			));
		}
	}	
	
	public void HideWorldSelect ()
	{
		for (int i = 0; i < worldSelectButtons.Length; i++) {
			var button = worldSelectButtons[i];
			button.GetComponent<tk2dUIItem>().OnClickUIItem += levelSelected;
			iTween.MoveTo(button.gameObject, iTween.Hash(
				"position", new Vector3(button.transform.position.x, i % 2 == 0 ? 235 : -75, 0), 
				"time", 0.5, 
				"easetype", iTween.EaseType.easeOutBack
			));
		}
	}
	
	public void ShowLevelSelect ()
	{
		levelSelectContainer.transform.position = Vector3.zero;
		for (int i = 0; i < levelSelectButtons.Length; i++) {
			var button = levelSelectButtons[i];
			button.GetComponent<tk2dUIItem>().OnClickUIItem += levelSelected;
			iTween.MoveFrom(button.gameObject, iTween.Hash(
				"position", new Vector3(280,button.transform.position.y, 0), 
				"time", 0.5, 
				"easetype", iTween.EaseType.easeOutBack
			));
		}
		
		iTween.MoveFrom(titleLabel.gameObject, iTween.Hash(
			"position", new Vector3(titleLabel.transform.position.x, 180, 0), 
			"time", 0.5, 
			"easetype", iTween.EaseType.easeOutBack
		));
	}

	void levelSelected (tk2dUIItem uiItem)
	{
		LevelSelectButton btn = uiItem.GetComponent<LevelSelectButton>();
		Debug.Log ("LEVEL" + btn.levelNumber);
		StartCoroutine(animateToLevel(1, btn.levelNumber));
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
		for (int i = 0; i < levelSelectButtons.Length; i++) {
			var button = levelSelectButtons[i];
			iTween.MoveTo(button.gameObject, iTween.Hash(
				"position", new Vector3(-40,button.transform.position.y, 0), 
				"time", 1, 
				"easetype", iTween.EaseType.easeOutBack
			));
		}
		
		iTween.MoveTo(titleLabel.gameObject, iTween.Hash(
			"position", new Vector3(titleLabel.transform.position.x, 180, 0), 
			"time", 1, 
			"easetype", iTween.EaseType.easeOutBack
		));
		
		yield return new WaitForSeconds(1);
		
		Application.LoadLevel("Level " + world + "-" + level);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
