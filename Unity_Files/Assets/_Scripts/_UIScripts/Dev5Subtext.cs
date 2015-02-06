using UnityEngine;
using UnityEngine.UI;
using System.Collections;


// Makes sure the little text at the top of the 5 Resource panel is righ
public class Dev5Subtext : MonoBehaviour {
	
	// Should set
	Text txt;
	
	// Use this for initialization
	void Start () {
		txt = gameObject.GetComponent<Text>();	
	}
	
	// Update is called once per frame
	void Update () {
		DevCardType currentDev = TurnState.DevTypeForSubstate();
		txt.text = DevCard.DescForCardType(currentDev);
	}
}