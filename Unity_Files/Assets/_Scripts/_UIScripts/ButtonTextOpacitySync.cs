using UnityEngine;
using UnityEngine.UI;
using System.Collections;


/// <summary>
/// Sync's the text ofa  button to have the opacity of its button parent
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
//		float opacity = btn.
	}
}
