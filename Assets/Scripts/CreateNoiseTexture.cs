using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNoiseTexture : MonoBehaviour {

	private RenderTexture src;
	public RenderTexture dest;
	private Material mat;

    private float[] timeout = new float[2];
    private float[] elapsedTime = new float[2];

    private Texture2D tempTex;
    private Rect clippingBox;
    private Color[] col;
    public OSCController oscController;


	void Awake () {
		src = new RenderTexture(256, 256, 0);
		src.wrapMode = TextureWrapMode.Clamp;
		mat = GetComponent<Renderer> ().material;

        tempTex = new Texture2D(1/*src.width-100*/, 1);
        clippingBox = new Rect(0, 0, 1/*src.width-100*/, 1);
	}
		
	void Update () {
		Graphics.Blit (mat.mainTexture, src, mat, 0);
		Graphics.Blit (src, dest, mat, 0 );

        for (int i = 0; i < 2; i++) {
            elapsedTime[i] += Time.deltaTime;

            if (elapsedTime[i] >= timeout[i]) {
                //AnalyzePixels(i);
            }
        }
	}

    void AnalyzePixels(int msgType) {
        tempTex.ReadPixels(clippingBox, 0/*50*/, 0);
        tempTex.Apply();
        col = tempTex.GetPixels();
        /*
        for (int i = 0; i < col.Length; i++) {
            if((col[i].r <= 0.2f) && (col[i].g <= 0.2f) && (col[i].b <= 0.2f)){
                darkPixDetected = true;
                // set a departure point of the structures
            }
        }
        */
        var index = 0;//Random.Range(0, col.Length);
        switch (msgType)
        {
            case 0:
                oscController.SendMessages("/carrier", col[index].r);
                break;
            case 1:
                oscController.SendMessages("/modulator", col[index].g);
                break;
            //case 2:
                //oscController.SendMessages("/index", col[index].b);
                //break;
        }

        elapsedTime[msgType] = 0f;
        timeout[msgType] = Random.Range(0f, 30f);
    }
}