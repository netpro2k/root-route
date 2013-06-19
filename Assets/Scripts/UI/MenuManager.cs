using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {
	
	public tk2dTextMesh titleLabel;
	public LevelSelectButton[] buttons;
	
	// Use this for initialization
	void Start () {
		for (int i = 0; i < buttons.Length; i++) {
			var button = buttons[i];
			button.GetComponent<tk2dUIItem>().OnClickUIItem += levelSelected;
			iTween.MoveFrom(button.gameObject, iTween.Hash(
				"position", new Vector3(280,button.transform.position.y, 0), 
				"time", 1, 
				"easetype", iTween.EaseType.easeInOutElastic,
				"delay", 1
			));
		}
		
		iTween.MoveFrom(titleLabel.gameObject, iTween.Hash(
			"position", new Vector3(titleLabel.transform.position.x, 180, 0), 
			"time", 1, 
			"easetype", iTween.EaseType.easeInOutElastic,
			"delay", 1
		));
	}

	void levelSelected (tk2dUIItem uiItem)
	{
		LevelSelectButton btn = uiItem.GetComponent<LevelSelectButton>();
		Debug.Log ("LEVEL" + btn.levelNumber);
		StartCoroutine(animateToLevel(1, btn.levelNumber));
	}
	
	IEnumerator animateToLevel(int world, int level) {
		for (int i = 0; i < buttons.Length; i++) {
			var button = buttons[i];
			iTween.MoveTo(button.gameObject, iTween.Hash(
				"position", new Vector3(-40,button.transform.position.y, 0), 
				"time", 1, 
				"easetype", iTween.EaseType.easeInOutElastic
			));
		}
		
		iTween.MoveTo(titleLabel.gameObject, iTween.Hash(
			"position", new Vector3(titleLabel.transform.position.x, 180, 0), 
			"time", 1, 
			"easetype", iTween.EaseType.easeInOutElastic
		));
		
		yield return new WaitForSeconds(1);
		
		Application.LoadLevel("Level " + world + "-" + level);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
