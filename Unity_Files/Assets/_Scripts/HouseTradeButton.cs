using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HouseTradeButton : MonoBehaviour {

	Button btn;
	/// <summary>
	/// The order that resources occur in the menu.
	/// </summary>
	ResourceType[] menuOrder = {
				ResourceType.Sheep,
				ResourceType.Wheat,
				ResourceType.Brick,
				ResourceType.Ore,
				ResourceType.Wood
		};

	// Use this for initialization
	void Start () {
		btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(TradeWithHouse);
	}

	void TradeWithHouse() {
		Player p = TurnState.currentPlayer;
		ComboBox toGetComboBox = GameObject.Find ("ResourceToGet").GetComponent<ComboBox>();
		ResourceType resourceToGet = menuOrder[toGetComboBox.SelectedIndex];

		ComboBox toGiveComboBox =  GameObject.Find ("ResourceToGive").GetComponent<ComboBox>();
		ResourceType resourceToGive = menuOrder[toGiveComboBox.SelectedIndex];

		TradeManager.TradeWithHouse (resourceToGet, resourceToGive, p);
	}

}
