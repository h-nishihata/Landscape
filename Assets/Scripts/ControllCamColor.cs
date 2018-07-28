using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllCamColor : MonoBehaviour {
    
    private float startTime;
    private float transitionTime = 1 / 10.0f;
    private float restorationTime = 1 / 2.0f;
    private bool isChangingColor;
    private bool isClimax;
    //private Camera baseCam;
    private VideoBloom bloom;
    private Color tintColor;
    private Color normalColor;
    public Kvant.Stream stream;


	void Start () {
        //baseCam = this.GetComponent<Camera>();
        bloom = this.GetComponent<VideoBloom>();
        normalColor = new Color(0.75f, 0.57f, 0.4f, 0.4f);
	}

    public void ControlTransition(int type){
        switch (type)
        {
            case 0:
                isChangingColor = false;
                break;
            case 1:
                isChangingColor = true;
                startTime = Time.timeSinceLevelLoad;
                isClimax = true;
                break;
            case 2:
                isChangingColor = true;
                startTime = Time.timeSinceLevelLoad;
                isClimax = false;
                break;            
        }
    }

	void Update () {
        if(isChangingColor) {
            var diff = Time.timeSinceLevelLoad - startTime;
            var rate = isClimax ? diff * transitionTime : diff * restorationTime;

            if (rate > 1.0f) return;

            if (isClimax) {                
                tintColor = Color.Lerp(normalColor, Color.white, rate);
                //baseCam.backgroundColor = Color.Lerp(Color.black, Color.white, rate);
                stream.noiseAmplitude = Mathf.Lerp(10f, 50f, rate);
            }else{
                tintColor = Color.Lerp(Color.white, normalColor, rate);
                stream.noiseAmplitude = Mathf.Lerp(50f, 10f, rate);
            }
            bloom.Tint = tintColor;
        }
	}
}
