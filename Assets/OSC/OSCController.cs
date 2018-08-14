using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityOSC;

public class OSCController : MonoBehaviour {

    private OSCHandler handler;
    private OSCReceiver receiver;
    public string address = "172.16.202.207";
	public int sendPort = 7400;
    public int receivePort = 6666;
    public ControllCamColor camColor;
	

	void Start () {
        handler = OSCHandler.Instance;
        handler.Init("Max", address, sendPort);
        handler.transform.SetParent(gameObject.transform);
		receiver = new OSCReceiver();
		receiver.Open(receivePort);
	}	
	
	void Update () {
		if(receiver.hasWaitingMessages()) {            
			OSCMessage msg = receiver.getNextMessage();
            if (msg.Address == "/transition") {
                ReceiveMessages(msg.Data);
                //}else{
                //Debug.LogError("cannot receive the message.");
            }
        }
    }
	
    public void SendMessages(string address, float value) {
        handler.SendMessageToClient("Max", address, value);
    }

    private void ReceiveMessages(List<object> data) {
        Debug.Log("received msgs");
		for(int i = 0; i < data.Count; i++) {
            int num;
            bool b = int.TryParse(data[i].ToString(), out num);

            if(b){
                camColor.ControlTransition(num);
            }
		}
	}
}