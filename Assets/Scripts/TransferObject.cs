using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferObject : MonoBehaviour {

    public GameObject startPosition;
    public GameObject endPosition;
    float startTime;
    float translateTime = 70.0f;
    Material mat;
    Color tintColor;
    Color targetColor;

	// Use this for initialization
	void Start () {
        transform.position = startPosition.transform.position;
        startTime = Time.timeSinceLevelLoad;
        mat = GetComponent<Renderer>().material;
        targetColor = new Color(0.65f, 0.45f, 0.28f, 0.47f);
	}
	
	// Update is called once per frame
	void Update () {
        var diff = Time.timeSinceLevelLoad - startTime;
        if (diff > translateTime) {
            transform.position = endPosition.transform.position;
            enabled = false;
        }
        var rate = diff / translateTime;

        tintColor = Color.Lerp(Color.black, targetColor, rate);
        mat.SetColor("_TintColor", tintColor);
        transform.position = Vector3.Lerp(startPosition.transform.position, endPosition.transform.position, rate);
	}
}
