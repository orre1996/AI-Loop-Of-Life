using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodGone : Config {

	// Use this for initialization
	void Start () {
		Invoke("destroy", 1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void destroy()
	{
		Destroy(gameObject);
	}
}
