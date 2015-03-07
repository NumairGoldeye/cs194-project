﻿using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class TradeCounter : MonoBehaviour, IEnumerable {

	Dictionary<ResourceType, int> counts;

	void Start () {
		counts = new Dictionary<ResourceType, int>();
		foreach (ResourceType r in System.Enum.GetValues(typeof(ResourceType))) {
			if (r == ResourceType.None) continue; // Shouldn't have a count for None.
			counts[r] = 0;
		}
	}

	public void AddResource(ResourceType r, int amount) {
		counts[r] += amount;
	}
	
	/** Removes |amount| resources from the counter, bottoming at zero. */
	public void RemoveResource(ResourceType r, int amount) {
		int newCount = counts[r] - amount;
		if (newCount < 0) {
			newCount = 0;
		}
		counts[r] = newCount;
	}

	public int GetResourceAmount(ResourceType r) {
		return counts[r];
	}

	IEnumerator IEnumerable.GetEnumerator() {
		return (IEnumerator) counts.GetEnumerator();
	}

}
