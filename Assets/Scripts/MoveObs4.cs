using UnityEngine;
using System.Collections;

/*
 * 	This script is used to move the Obstacle towards left and then to destroy the Obstacle once it reaches a certain point.
 */ 

public class MoveObs4 : MonoBehaviour
{
	//	we move the obstacle left along the negative x axis with speed of 4.
	// Use this for initialization
	void Start ()
	{	
		
		GetComponent<Rigidbody2D>().velocity = new Vector2 (-3.0f,0.0f); //-4
	}

	//	we check the obstacle's position against a point such that we are sure that this obstacle is outside the camera and then destroy the obstacle
	// Update is called once per frame
	void Update ()
	{	
		if( transform.position.x < -30.0f )
		{
			Destroy(gameObject);
		}
	}
}