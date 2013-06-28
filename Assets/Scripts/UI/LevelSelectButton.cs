using UnityEngine;
using System.Collections;

public class LevelSelectButton : MonoBehaviour {
	
	public tk2dTextMesh[] labels;
	public tk2dSprite[] nutrients;
	public int world;
	public int level;
	
	public void SetWorldAndLevel(int world, int level) {
		this.world = world;
		this.level = level;
		foreach (var label in labels) {
			label.text = "" + (level + 1);
			label.Commit();
		}
		
		int unlockedNutrients = SaveManager.Instance.levelNutrients[world,level];
		GetComponent<tk2dUIUpDownDisableButton>().Disabled = unlockedNutrients == -1;
		for (int i = 0; i < nutrients.Length; i++) {
			nutrients[i].SetSprite(unlockedNutrients >= i + 1 ? "nutrient/0" : "nutrient/1");
		}
	}
}
