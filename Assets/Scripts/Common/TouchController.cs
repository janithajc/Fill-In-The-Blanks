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
	private Vector3 screenPoint;
	private Vector3 initialPosition;
	private Vector3 offset;
	private Rigidbody rbody;
	private Color gold;

	void Start (){
		Input.simulateMouseWithTouches = true;
		rend = GetComponent<Renderer> ();
		rbody = GetComponent<Rigidbody> ();
		gold = new Color (1, 0.7f, 0.5f);
	}

	void OnMouseDown() {
		isDragging = true;
		isGonnaDrag = true;
		touchTime = Time.time;
		//screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position); // I removed this line to prevent centring 
		initialPosition = transform.position;
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		Debug.Log ("Touched "+name);
		if(rbody != null)
			rbody.velocity = Vector3.zero;
	}

	void OnMouseDrag() { 
		if (isGonnaDrag && Time.time > touchTime + 1F) {
			rend.material.SetColor("_Color", gold);
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
			transform.position = curPosition;
			isDragging = false;
			if(rbody != null)
				rbody.velocity = curPosition - initialPosition;
		}
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
		transform.position = new Vector3 (transform.position.x, transform.position.y, -2);
		if (Input.GetMouseButton(0) && isDragging) {
			theSpeed = new Vector3(-Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0.0F);
			//Debug.Log(-Input.GetAxis("Mouse X") + " : " + Input.GetAxis("Mouse Y"));
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