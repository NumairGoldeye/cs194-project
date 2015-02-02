using UnityEngine;
using System.Collections.Generic;


public class TileClass : MonoBehaviour {

	public ResourceType type;
	public int diceValue;
	public int tileNumber;
	public bool hasRobber;
	private List<int> vertices;

	private void displayDiceNumber (){
		if (diceValue > 0) {
			GetComponentInChildren <TextMesh>().text = diceValue.ToString ();
		}
		else {//Don't display a number if it is desert
			GetComponentInChildren <TextMesh>().text = "";
		}
	}

	public void assignType(int diceNumber, ResourceType assignedType){
		type = assignedType;
		diceValue = diceNumber;
		displayDiceNumber ();
		getMaterial ();
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

	public void init() {
		vertices = new List<int>();
	}
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void addVertex(int vertex) {
//		Debugger.Log ("Vertex", v.settlement.isBuilt ().ToString());
		vertices.Add (vertex);
	}

	public List<int> getVertices() {
		return vertices;
	}
}
