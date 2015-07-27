using UnityEngine;
using System.Collections;

public class TouchController : MonoBehaviour {

	public float perspectiveZoomSpeed = 0.1f;        // The rate of change of the field of view in perspective mode.
	
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
		Debug.Log (Time.time + " : Touched " + name + ", " + Input.mousePosition.x + ":" + Input.mousePosition.y);
		isDragging = true;
		isGonnaDrag = true;
		touchTime = Time.time;
		//screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position); // I removed this line to prevent centring 
		initialPosition = transform.position;
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		if(rbody != null)
			rbody.velocity = Vector3.zero;
	}

	void OnMouseDrag() { 
		if (isGonnaDrag && Time.time > touchTime + 1F) {
			Debug.Log (Time.time + " : Dragging " + name + ", " + Input.mousePosition.x + ":" + Input.mousePosition.y);
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
		transform.position = new Vector3 (transform.position.x, transform.position.y, -2);
		//pinch zoom start
		if (Input.touchCount == 2 && isDragging)
		{
			// Store both touches.
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);
			
			// Find the position in the previous frame of each touch.
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
			
			// Find the magnitude of the vector (the distance) between the touches in each frame.
			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
			
			// Find the difference in the distances between each frame.
			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

			// Otherwise change the field of view based on the change in distance between the touches.
			transform.localScale += new Vector3(deltaMagnitudeDiff * perspectiveZoomSpeed, deltaMagnitudeDiff * perspectiveZoomSpeed, deltaMagnitudeDiff * perspectiveZoomSpeed);
				
			// Clamp the field of view to make sure it's between 0 and 180.
			//transform.localScale.magnitude = Mathf.Clamp(transform.localScale.magnitude, 0.1f, 179.9f);
		}
		//pinch zoom end
		if (Input.GetMouseButton(0) && isDragging) {
			Debug.Log (Time.time + " : Rotating " + name + ", " + Input.mousePosition.x + ":" + Input.mousePosition.y);
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