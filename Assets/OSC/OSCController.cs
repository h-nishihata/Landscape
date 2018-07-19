using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityOSC;

public class OSCController : MonoBehaviour {

    private OSCHandler handler;
	private OSCReciever reciever;
    public string address = "172.16.202.207";
	public int sendPort = 7400;
    public int receivePort = 6666;
	
	// Use this for initialization
	void Start () {
        handler = OSCHandler.Instance;
        handler.Init("Max", address, sendPort);
        handler.transform.SetParent(gameObject.transform);

		reciever = new OSCReciever();
		reciever.Open(receivePort);
	}
	
	// Update is called once per frame
	void Update () {
		if(reciever.hasWaitingMessages()) {
			OSCMessage msg = reciever.getNextMessage();
			Debug.Log(string.Format("message received: {0} {1}", msg.Address, DataToString(msg.Data)));
		}
	}
	
    public void SendMessages(string address, float value) {
        handler.SendMessageToClient("Max", address, value);
    }

    private string DataToString(List<object> data) {
		string buffer = "";
		
		for(int i = 0; i < data.Count; i++) {
			buffer += data[i].ToString() + " ";
		}
		
		buffer += "\n";
		
		return buffer;
	}
}