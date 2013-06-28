using UnityEngine;
using System.Collections;

public class SaveManager
{
    private static SaveManager instance;
 
 
    public static SaveManager Instance
    {
        get
        {
            if (instance == null)
            {
                new SaveManager ();
            }
 
            return instance;
        }
    }
 
	
	public int totalNutrients;
	public int[,] levelNutrients;
	
    public SaveManager () 
    {
        if (instance != null)
        {
            Debug.LogError ("Cannot have two instances of singleton. Self destruction in 3...");
            return;
        }
 
        instance = this;
		
		levelNutrients = new int[4,10];
		for (int world = 0; world < 4; world++) {
			for (int level = 0; level < 10; level++) {
				levelNutrients[world,level] = PlayerPrefs.GetInt("Level" + world + "-" + level, -1);
			}
		}
		
		// Always unlock first level
		if(levelNutrients[0,0] == -1) {
			levelNutrients[0,0] = 0;
		}
    }
	
	public void SaveNutrientsForLevel(int world, int level, int nutrients) {
		if(nutrients > levelNutrients[world,level]) {
			levelNutrients[world,level] = nutrients;
			PlayerPrefs.SetInt("Level"+world+"-"+level, nutrients);
		}

		// Unlock the next level if its not unlocked
		if(level < 9 && levelNutrients[world,level+1] == -1){
			levelNutrients[world, level+1] = 0;
			PlayerPrefs.SetInt("Level"+world+"-"+(level+1), 0);
		};
		
		PlayerPrefs.Save();
	}
	
}