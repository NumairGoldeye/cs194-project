using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Leader board panel in the upper right corner
/// 
/// all this script does is make sure there are enough of the prefab
/// </summary>
public class LeaderBoardPanel : MonoBehaviour {

	int count = 0;

	public GameObject leaderBoardPlayerNamePrefab; // set in inspector


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		int currentCount = GameManager.Instance.players.Count ;

		if (count != currentCount) {
		
			//Clears the playerlist
			List<GameObject> children = new List<GameObject>();
			foreach (Transform child in transform) children.Add(child.gameObject);
			foreach ( GameObject child in children){
				child.transform.SetParent(null);
				GameObject.Destroy(child);
			}

			//repopulates it
			foreach( Player p in GameManager.Instance.players){
//				if (p != GameManager.Instance.myPlayer){
					GameObject child = Instantiate(leaderBoardPlayerNamePrefab, Vector3.zero, Quaternion.identity) as GameObject;
					child.transform.SetParent(transform);

					child.GetComponent<LeaderBoardPlayerName>().SetPlayer(p);	
//				}
			}

			count = currentCount;
		}


	}
}
