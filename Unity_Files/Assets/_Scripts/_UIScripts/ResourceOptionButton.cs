using UnityEngine;
using UnityEngine.UI;
using System.Collections;


/*

	For the resource options chosen for monopoly and year of plenty

	If a button is clicked -
		TurnState.chosenResource becomes that resource
		all other buttons are unhighlighted.

 */

public class ResourceOptionButton : MonoBehaviour {

	// Set in inspector
	public ResourceType resType;


	// Don't need inspector
	private Button btn;
	private ColorBlock selectedBlock;
	private ColorBlock unselectedBlock;
	public GameObject allButtonParent; 
	// Text txt; // set this later

	static Color selectedColor = new Color(1.0f,1.0f,1.0f);
	static Color unselectedColor = new Color(.8f,.8f,.8f);


	// button should have a highlighted state - currently set in inspector
	
	// Use this for initialization
	void Start () {
		btn = gameObject.GetComponent<Button>();
		selectedBlock = btn.colors;
		selectedBlock.normalColor = ResourceOptionButton.selectedColor;
		unselectedBlock = btn.colors;
		unselectedBlock.normalColor = ResourceOptionButton.unselectedColor;

		btn.onClick.AddListener(SelectResource);
		allButtonParent = transform.parent.gameObject;

	}
	
	// Update is called once per frame
	void Update () {
		UpdateColor();
	}

	// The allButtonParent broadcasts this to its children...
	void UpdateColor(){
		if (resType == TurnState.chosenResource){
//			cb.normalColor = ResourceOptionButton.selectedColor;
			btn.colors = selectedBlock;
		} else {

			btn.colors = unselectedBlock;
		}
	}


	// 
	void SelectResource(){
		TurnState.chosenResource = resType;
		allButtonParent.BroadcastMessage("UpdateColor");
	}

}
