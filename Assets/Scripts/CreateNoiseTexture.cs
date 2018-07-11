using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNoiseTexture : MonoBehaviour {

	private RenderTexture src;
	public RenderTexture dest;
	private Material mat;


	void Awake () {
		
		src = new RenderTexture( 256, 256, 0 );
		src.wrapMode = TextureWrapMode.Clamp;

		mat = GetComponent<Renderer> ().material;

	}
		
	void Update () {
		
		Graphics.Blit (mat.mainTexture, src, mat, 0 );
		Graphics.Blit (src, dest, mat, 0 );

	}

}
