using UnityEngine;
using System.Collections;

public class GetTileSize : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log(GetComponent<MeshFilter>().mesh.bounds.size.x);
		Debug.Log(GetComponent<MeshFilter>().mesh.bounds.size.y);
	}
	
	// Update is called once per frame
	void Update () {
		//System.Console.WriteLine (GetComponent<MeshFilter>().mesh.bounds);
	}
}
