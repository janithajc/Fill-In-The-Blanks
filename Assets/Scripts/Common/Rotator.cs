﻿using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {
	public float speedx, speedy, speedz;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (Vector3.up, speedx * Time.deltaTime);
		transform.Rotate (Vector3.right, speedy * Time.deltaTime);
		transform.Rotate (Vector3.forward, speedz * Time.deltaTime);
	}
}
