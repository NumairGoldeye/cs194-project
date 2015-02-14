using UnityEngine;
using System.Collections;

public class FirstPersonControl : MonoBehaviour {


	public float xRotatePower = 2.0f;
	public float yRotatePower = 2.0f;

	public float xTranslatePower = 2.0f;
	public float zTranslatePower = 2.0f;

	// Some code http://answers.unity3d.com/questions/29741/mouse-look-script.html
	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
	
	public float minimumX = -360F;
	public float maximumX = 360F;
	
	public float minimumY = -60F;
	public float maximumY = 60F;

	bool allowCamera = false;
	
	float rotationX = 0F;
	float rotationY = 0F;
	
	Quaternion originalRotation;
	

	
	void Start ()
	{
		// Make the rigid body not change rotation
		if (rigidbody)
			rigidbody.freezeRotation = true;
		originalRotation = transform.localRotation;
	}
	
	public static float ClampAngle (float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp (angle, min, max);
	}
	
	// Update is called once per frame
	void Update () {

		if (ChatLog.InChat()){
			return;
		}


		if (Input.GetKeyDown("space")){
			allowCamera = !allowCamera;
		}

		if (!allowCamera) return;


		float xAxisValue = Input.GetAxis("Horizontal") * xTranslatePower;
		float zAxisValue = Input.GetAxis("Vertical") * zTranslatePower;


//     	float xRotateVal = Input.GetAxis("RotateHorizontal") * xRotatePower;
//     	float yRotateVal = - Input.GetAxis("RotateVertical") * yRotatePower;

     	// if(Camera.current != null) {
//		transform.Translate(new Vector3(xAxisValue, 0.0f, zAxisValue));
		transform.position += new Vector3(xAxisValue, 0.0f, zAxisValue);

		transform.Translate(Vector3.forward * Input.GetAxis("Mouse ScrollWheel"));

//        	transform.Rotate(new Vector3(yRotateVal, 0.0f, 0.0f ));

//        	transform.RotateAround(transform.position, Vector3.up, xRotateVal);
     	// }

		if (axes == RotationAxes.MouseXAndY){
			// Read the mouse input axis
			rotationX += Input.GetAxis("Mouse X") * sensitivityX;
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			
			rotationX = ClampAngle (rotationX, minimumX, maximumX);
			rotationY = ClampAngle (rotationY, minimumY, maximumY);
			
//			Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);

			Quaternion yQuaternion = Quaternion.AngleAxis (rotationY, -Vector3.right);
			
			transform.localRotation = originalRotation * yQuaternion;
			transform.RotateAround(transform.position, Vector3.up, rotationX);

		} else if (axes == RotationAxes.MouseX) {
			rotationX += Input.GetAxis("Mouse X") * sensitivityX;
			rotationX = ClampAngle (rotationX, minimumX, maximumX);
			
//			Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
//			transform.localRotation = originalRotation * xQuaternion;

			transform.RotateAround(transform.position, Vector3.up, rotationX);
		} else {
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = ClampAngle (rotationY, minimumY, maximumY);
			
			Quaternion yQuaternion = Quaternion.AngleAxis (-rotationY, Vector3.right);
			transform.localRotation = originalRotation * yQuaternion;
		}
	}

}
