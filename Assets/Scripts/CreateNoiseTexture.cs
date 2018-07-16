using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNoiseTexture : MonoBehaviour {

	private RenderTexture src;
	public RenderTexture dest;
	private Material mat;
    //private Texture2D tempTex;
    //private Rect clippingBox;
    //private Color[] col;

	void Awake () {
		src = new RenderTexture(256, 256, 0);
		src.wrapMode = TextureWrapMode.Clamp;
		mat = GetComponent<Renderer> ().material;
        //tempTex = new Texture2D(src.width-100, 1);
        //clippingBox = new Rect(0, 0, src.width-100, 1);
	}
		
	void Update () {
		Graphics.Blit (mat.mainTexture, src, mat, 0);
		Graphics.Blit (src, dest, mat, 0 );
        /*
        if (Input.GetKeyDown(KeyCode.T)) {
            tempTex.ReadPixels(clippingBox, 50, 0);
            tempTex.Apply();
            col = tempTex.GetPixels();

            for (int i = 0; i < col.Length; i++) {
                if((col[i].r <= 0.2f) && (col[i].g <= 0.2f) && (col[i].b <= 0.2f)){
                    
                }
            }
        }
        */
	}
}