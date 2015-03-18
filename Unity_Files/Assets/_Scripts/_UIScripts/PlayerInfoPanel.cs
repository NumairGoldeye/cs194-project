using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;


/// <summary>
/// Player info panel UI on your own UI
/// 
/// Very similar to LeaderBoardPlayerName
/// </summary>
public class PlayerInfoPanel : MonoBehaviour {

	Player player;

	public Text playerName;
	public Text knightCount;
	public Text victoryCount;
	
	public Image largestArmyIcon;
	public Image longestRoadIcon;

	bool setup = false;

	// Use this for initialization
	void Start () {
		player = GameManager.Instance.myPlayer;
	}
	
	// Update is called once per frame
	void Update () {
			
//		if (GameManager.Instance.myPlayer != null){
			player = GameManager.Instance.myPlayer;
			setup = true;
			
//		}
			//
		//
		try {
//			Debugger.Log ("LeaderBoard", player.playerName);
			
			playerName.text = player.playerName;
			knightCount.text = player.numUsedKnights.ToString();
			victoryCount.text = player.victoryPoints.ToString();
			
			if (player.hasLargestArmy){
				largestArmyIcon.gameObject.SetActive(true);
			} else  {
				largestArmyIcon.gameObject.SetActive(false);
			}
			
			if (player.hasLongestRoad){
				longestRoadIcon.gameObject.SetActive(true);
			} else {
				longestRoadIcon.gameObject.SetActive(false);
				//				longestRoadIcon.color = new Color(1f,1f,1f);
			}
		} catch ( NullReferenceException exception) {
				//Do other stuff.
		}
			
			
	}
}
