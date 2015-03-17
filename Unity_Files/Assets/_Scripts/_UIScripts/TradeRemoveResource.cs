using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TradeRemoveResource : MonoBehaviour {

	public ResourceType type; // should be set in inspector
	public ResourceCounter counter; // should be set in inspector

	Button btn;

	void Start () {
		btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(DecrementResource);
	}

	private void DecrementResource() {
		counter.RemoveResource(type, 1);
	}
}
