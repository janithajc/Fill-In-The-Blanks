using UnityEngine;
using System.Collections;

public class TouchController : MonoBehaviour {
	
	private float rotationSpeed = 10.0F;
	private float lerpSpeed = 1.0F;
	
	private Vector3 theSpeed;
	private Vector3 avgSpeed;
	private bool isDragging = false;
	private bool isGonnaDrag = false;
	private Vector3 targetSpeedX;
	private float touchTime;
	private Renderer rend;

	void Start (){
		Input.simulateMouseWithTouches = true;
		rend = GetComponent<Renderer>();
	}

	private Vector3 screenPoint; private Vector3 offset;
	
	void OnMouseDrag() { 
		if (isGonnaDrag && Time.time > touchTime + 2F) {
			rend.material.SetColor("_Color",Color.green);
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
			transform.position = curPosition;
			isDragging = false;
		}
	}
	
	void OnMouseDown() {
		isDragging = true;
		isGonnaDrag = true;
		touchTime = Time.time;
		//screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position); // I removed this line to prevent centring 
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		Debug.Log ("Touched "+name);
	}

	void OnMouseExit() {
		isGonnaDrag = false;
	}

	void OnMouseUp() {
		isGonnaDrag = false;
		rend.material.SetColor("_Color",Color.white);
	}
	
	void Update() {
		//Rotation Detect
		if (Input.GetMouseButton(0) && isDragging) {
			theSpeed = new Vector3(-Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0.0F);
			avgSpeed = Vector3.Lerp(avgSpeed, theSpeed, Time.deltaTime * 5);
		} else {
			if (isDragging) {
				theSpeed = avgSpeed;
				isDragging = false;
			}
			float i = Time.deltaTime * lerpSpeed;
			theSpeed = Vector3.Lerp(theSpeed, Vector3.zero, i);
		}
		
		transform.Rotate(Camera.main.transform.up * theSpeed.x * rotationSpeed, Space.World);
		transform.Rotate(Camera.main.transform.right * theSpeed.y * rotationSpeed, Space.World);
	}
}

/*using UnityEngine;
using System.Collections;

public class RotationController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	bool isGrabbed = false;
	Vector3 grabbedPoint;
	
	void Update () 
	{ 
		RaycastHit hit; 
		
		if (Input.GetMouseButton(0)) 
		{         
			if(!isGrabbed)
			{
				isGrabbed = true;
				
				//Find the hit point
				Physics.Raycast (Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
				
				//Convert touched point to a local point on the earth
				grabbedPoint = getTouchedPointLocal();
			}
			else
			{
				Vector3 targetPoint = getTouchedPoint(); 
				transform.rotation = Quaternion.FromToRotation (grabbedPoint,targetPoint);
			}
		}
		else
			isGrabbed = false;
	}
	
	Vector3 getTouchedPoint()
	{
		RaycastHit hit;
		
		//Find the hit point
		Physics.Raycast (Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
		
		//Convert touched point to a local point on the earth
		return  hit.point-transform.position; 
	}
	
	Vector3 getTouchedPointLocal()
	{
		return transform.InverseTransformPoint(getTouchedPoint());
	}
}*/
