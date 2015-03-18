using UnityEngine;
using UnityEngine.UI;
using System.Collections;


/// <summary>
/// Syncs the UI image with its UI parent Button
/// </summary>
public class UIImageOpacitySync : MonoBehaviour {
	Button btn;
	Image img;
	
	// Use this for initialization
	void Start () {
		img = GetComponent<Image>();
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
		Color tempColor = img.color;
		tempColor.a = opacity;
		
		img.color = tempColor;

	}

}
