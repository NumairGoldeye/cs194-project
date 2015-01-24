using UnityEngine;
using System.Collections;

public class TileClass : MonoBehaviour {
	public enum tileType {sheep, wood, brick, ore, wheat, desert};
	tileType type;

	private void getType () {
		GameObject go = GameObject.Find ("GameManager");
		GameManager gm = (GameManager)go.GetComponent (typeof(GameManager));
		type = (tileType)gm.assignTileType ();
		Debug.Log (type);
	}

	private void getMaterial () {

		switch (type) {
			case tileType.sheep:
				renderer.material.color = Color.green;
				break;
			case tileType.wheat:
				renderer.material.color = Color.yellow;
				break;
			case tileType.ore:
				renderer.material.color = Color.gray;
				break;
			case tileType.brick:
				renderer.material.color = Color.red;
				break;
			case tileType.wood:
				renderer.material.color = Color.black;
				break;
			case tileType.desert:
				renderer.material.color = Color.white;
				break;
		}
	}

	// Use this for initialization
	void Start () {
		getType ();	
		getMaterial ();	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
