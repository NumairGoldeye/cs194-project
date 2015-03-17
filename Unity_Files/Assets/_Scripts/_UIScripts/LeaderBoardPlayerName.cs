using UnityEngine;
using UnityEngine.UI;
using System.Collections;


/// <summary>
/// Leader board player name.
/// </summary>
public class LeaderBoardPlayerName : MonoBehaviour {

	Player player; // set by LeaderBoardPanel;
	bool set = false;

	// Set in inspector
	public Text playerName;
	public Text knightCount;
	public Text cardCount;

	public Image largestArmyIcon;
	public Image longestRoadIcon;




	// Use this for initialization
	void Start () {
		// Why does the UI autoscale it? so annoying, sigh
		transform.localScale = new Vector3(1f,1f,1f);

	}
	
	// Update is called once per frame
	void Update () {
//		Debugger.Log ("LeaderBoard", "WHEE");

		if (set){
//
//
			Debugger.Log ("LeaderBoard", player.playerName);

			playerName.text = player.playerName;
			knightCount.text = player.numUsedKnights.ToString();
			cardCount.text = player.GetTotalHandSize().ToString();

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


		}
	}

	public void SetPlayer( Player p){
		set = true; // because (player != null) just won't cut it
		Debugger.Log ("LeaderBoard", p.playerName);
		player = p;
	}
}
