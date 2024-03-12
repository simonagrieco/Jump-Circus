using UnityEngine;
using System.Collections;

/*
 * 	This script is used to generate the background repeatedly. 
 */ 

public class GenerateBG : MonoBehaviour
{
	public float speed;

	[SerializeField]
	private Renderer bgRenderer;

    private void Update()
    {
        bgRenderer.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);
    }

    
}