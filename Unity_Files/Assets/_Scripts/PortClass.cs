using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PortClass : MonoBehaviour {
	public ResourceType type;
	public int ratio;
	
	public PortClass(ResourceType type, int ratio) {
		this.type = type;
		this.ratio = ratio;
	}
}
