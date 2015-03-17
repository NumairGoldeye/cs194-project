using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TradeResourceTextCount : MonoBehaviour {
	
	
	public ResourceType type; // should be set in inspector
	public ResourceCounter counter; // should be set in inspector
	Text txt;
	
	
	// Use this for initialization
	void Start () {
		txt = gameObject.GetComponent<Text>();	
	}
	
	// Update is called once per frame
	void Update () {
		txt.text = counter.GetResourceAmount(type).ToString();
	}
}
