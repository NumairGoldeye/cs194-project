using UnityEngine;
using System.Collections;

public class SettlementClass : MonoBehaviour {

	private bool built; //is the settlement built?
	private bool visible;
	private bool upgrading;
	public GameObject settlements;
	private GameObject settlement;
	private GameObject city;
	private bool hasCity;
	public int vertexIndex;
	private int ownerId;

	// Use this for initialization
	void Start () {
		settlement = transform.FindChild("SettlementObject").gameObject;
		Debugger.Log ("Settlement", settlement.tag);
		city = transform.FindChild("CityObject").gameObject;
		built = false;
		upgrading = false;
		ownerId = -1;
		hideSettlement ();
		hideCity ();
		hasCity = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void hideCity() {
		Color temp = city.renderer.material.color;
		temp.a = 0;
		city.renderer.material.color = temp;
	}

	private void showCity() {
		city.renderer.material.color = TurnState.currentPlayer.playerColor;
	}
	
	public bool isCity() {
		return hasCity;
	}

	public int getPlayer() {
		return ownerId;
	}

	public bool isBuilt() {
		return built;
	}
	
	public void upgradeAbility() {
		if (isBuilt ())
			upgrading = true;
	}

	public void toggleSettlements() {
		if (!built) {
			if (!visible)
				showSettlement();
			else
				hideSettlement();
		}
	}
	void hideSettlement() {
		visible = false;
		Color temp = settlement.renderer.material.color;
		temp.a = 0;
		settlement.renderer.material.color = temp;		
	}

	void showSettlement() {
		visible = true;
		Color temp = settlement.renderer.material.color;
		temp.a = 0.8f;
		settlement.renderer.material.color = temp;
	}
	
	void OnMouseDown() {
		if (!built) {
			if (!visible) return;
			built = true;
			settlement.renderer.material.color = TurnState.currentPlayer.playerColor;
			ownerId = TurnState.currentPlayer.playerId;
			settlements.BroadcastMessage ("toggleSettlements");
			BuyManager.PurchaseForPlayer(BuyableType.settlement, TurnState.currentPlayer);
		} else {
			if (upgrading) {
				BuyManager.PurchaseForPlayer(BuyableType.city, TurnState.currentPlayer);
				hasCity = true;
				hideSettlement();
				showCity();
			}
		}
	}
}
