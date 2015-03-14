using UnityEngine;
using UnityEngine.UI;
using System.Collections;


/// <summary>
/// Sync's the text ofa  button to have the opacity of its button parent
/// depending on its interactability
/// 
/// assumes that the button is using the Color Tint to change its visuals
/// </summary>
public class ButtonTextOpacitySync : MonoBehaviour {

	Button btn;
	Text txt;

	// Use this for initialization
	void Start () {
		txt = GetComponent<Text>();
		btn = transform.parent.gameObject.GetComponent<Button>();
	}
	
	// Update is called once per frame
	void Update () {
		float opacity;

		if (btn.interactable){
			opacity = btn.colors.normalColor.a;
		} else {
			opacity = btn.colors.disabledColor.a;
		}
		Color tempColor = txt.color;
		tempColor.a = opacity;

		txt.color = tempColor;
	}
}
