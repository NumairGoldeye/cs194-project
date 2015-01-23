using UnityEngine;
using System.Collections;

public class FirstPersonControl : MonoBehaviour {


	public float xRotatePower = 2.0f;
	public float yRotatePower = 2.0f;

	public float xTranslatePower = 2.0f;
	public float zTranslatePower = 2.0f;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		float xAxisValue = Input.GetAxis("Horizontal") * xTranslatePower;
     	float zAxisValue = Input.GetAxis("Vertical") * zTranslatePower;

     	float xRotateVal = Input.GetAxis("RotateHorizontal") * xRotatePower;
     	float yRotateVal = - Input.GetAxis("RotateVertical") * yRotatePower;

     	// if(Camera.current != null) {
        	transform.Translate(new Vector3(xAxisValue, 0.0f, zAxisValue));
        	transform.Rotate(new Vector3(yRotateVal, 0.0f, 0.0f ));

        	transform.RotateAround(transform.position, Vector3.up, xRotateVal);
     	// }

	}

}
