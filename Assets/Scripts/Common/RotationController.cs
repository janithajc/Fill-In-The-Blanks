using UnityEngine;
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
}
