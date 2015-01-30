using UnityEngine;
using UnityEngine.UI;
using System.Collections;


/*
Used just for development to add resources to people

*/

public class _DevAddResource : MonoBehaviour {

	public bool useDev;
	public DevCardType cardType; // set in inspector, only set this or resType, not both
	public ResourceType resType; // set in inspector
	public int val = 1;
	public Player player;

	Button btn;


	// Use this for initialization
	void Start () {
		btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(ChangeResource);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void ChangeResource(){
		player = TurnState.currentPlayer;
		if (useDev){
			if (val > 0) {
				player.AddDevCard(cardType);
			} else {
				player.RemoveDevCard(cardType);
			}
		} else {
			// if resourceType
			player.AddResource(resType, val);
		}
	}


}
