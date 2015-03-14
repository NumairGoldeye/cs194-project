using UnityEngine;
using UnityEngine.UI;
using System.Collections;


/// <summary>
/// The name of a game in a gameList
/// 
/// should be attached to a button
/// </summary>
public class GameListName : MonoBehaviour {

	public GameList list;
	public HostData data;


	Text txt;
	Button btn;

	private ColorBlock selectedBlock;
	private ColorBlock unselectedBlock;
	static Color selectedColor = new Color(0.5f, 0.5f, 0.5f);
	static Color unselectedColor = new Color(0,0,0);


	// Use this for initialization
	void Start () {
		btn = gameObject.GetComponent<Button>();
		txt = transform.GetChild(0).gameObject.GetComponent<Text>();

		btn.onClick.AddListener(SelectSelf);

		selectedBlock = btn.colors;
		selectedBlock.normalColor = GameListName.selectedColor;
		unselectedBlock = btn.colors;
		unselectedBlock.normalColor = GameListName.unselectedColor;
	}
	
	// Update is called once per frame
	void Update () {
		if (data != null){
			txt.text = data.gameName;
		}
	}

	// Should be called right after initializing the prefab;
	public void Setup(HostData hd, GameList gl){
		list = gl;
		data = hd;
	}

	// Tells the gameList that this one is selected
	void SelectSelf(){
		Debugger.Log ("GameList", "clicked");
		transform.parent.gameObject.BroadcastMessage("UnSelectSelf");

		btn.colors = selectedBlock;

		if (list)
			list.ChooseGame(data);

	}

	void UnSelectSelf(){
		btn.colors = unselectedBlock;
	}

}
