using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TradeAddResource : MonoBehaviour {

	public ResourceType type; // should be set in inspector
	public TradeCounter counter; // should be set in inspector

	Button btn;

	void Start () {
		btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(IncrementResource);
	}

	private void IncrementResource() {
		counter.AddResource(type, 1);
	}

}
