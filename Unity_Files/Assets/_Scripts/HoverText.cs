using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 
using System.Collections;


/// <summary>
/// Hover text is a script that you attach to objects to make sure that they have the right text that appears
/// whe you hover over them...
/// 
/// Set the text in the inspector
/// </summary>
public class HoverText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	static string defaultDisplayText = "Set Me";

	Text text;
	GameObject hoverTextObj; 
	GameObject prefab; // the hovertextobj prefab
	RectTransform hoverRectT; //rectransform of hovertext

	public string displayText = "";

	bool setup = false;
	bool positioned = false;
	bool pointerInside = false;

	float timeEntered = 0f;
	public float hoverRevealTime = 0.5f;


	// Use this for initialization
	void Start () {
		prefab =  Resources.LoadAssetAtPath<GameObject>("Assets/Materials/UI_images/HoverText.prefab");

		hoverTextObj = Instantiate(prefab) as GameObject;
		hoverTextObj.transform.SetParent(transform, false);

		hoverRectT = hoverTextObj.GetComponent<RectTransform>();

		// There should only be one child for the prefab
		text = hoverTextObj.transform.GetChild(0).GetComponent<Text>();


		if (displayText != ""){
			text.text = displayText;
		} else {
			text.text = defaultDisplayText;
		}

		HideHover();
	}



	// Update is called once per frame
	void Update () {
		TrySetupLayout();



		if (pointerInside && (Time.time - timeEntered) > hoverRevealTime){
			ShowHover();
		} else {
			HideHover();
		}
	}


	void TrySetupLayout(){
		if (!setup){
			if (text.rectTransform.rect.height != 0){
				setup = true;
			} else {
				LayoutRebuilder.MarkLayoutForRebuild (hoverTextObj.transform as RectTransform);
			}
		}
		
		if (!positioned && setup){
			hoverRectT.anchoredPosition = new Vector2(0, hoverRectT.rect.height + 20.0f );
			positioned = true;
		}
	}


	void HideHover(){
		hoverTextObj.SetActive(false);
	}

	void ShowHover(){
		hoverTextObj.SetActive(true);
	}




	public void OnPointerEnter(PointerEventData eventData){
		pointerInside = true;

		timeEntered = Time.time;

//		ShowHover();


		Debug.Log("Enter");



	}

	public void OnPointerExit(PointerEventData eventData){
		pointerInside = false;
//		Debug.Log("Exit");
//		HideHover();
	}


}
