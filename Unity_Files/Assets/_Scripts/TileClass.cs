using UnityEngine;
using System.Collections;

public class TileClass : MonoBehaviour {

	public ResourceType type;
	public int diceValue;

	private void getInfo () {
		GameObject go = GameObject.Find ("GameManager");
		GameManager gm = (GameManager)go.GetComponent (typeof(GameManager));
		GameManager.TileInfo info = gm.assignTileInfo ();
		type = info.type;
		diceValue = info.diceNumber;
		// Debug.Log ("Type:" + type + " DiceNumber:" + diceValue);
	}

	private void getMaterial () {

		switch (type) {
			case ResourceType.sheep:
				renderer.material.color = Color.green;
				break;
			case ResourceType.wheat:
				renderer.material.color = Color.yellow;
				break;
			case ResourceType.ore:
				renderer.material.color = Color.gray;
				break;
			case ResourceType.brick:
				renderer.material.color = Color.red;
				break;
			case ResourceType.wood:
				renderer.material.color = Color.black;
				break;
			case ResourceType.desert:
				renderer.material.color = Color.white;
				break;
		}
	}

	// Use this for initialization
	void Start () {
		getInfo ();	
		getMaterial ();	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
