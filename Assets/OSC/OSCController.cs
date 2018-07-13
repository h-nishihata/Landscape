using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityOSC;

public class OSCController : MonoBehaviour {

    private OSCHandler handler;
	private OSCReciever reciever;
	public int port = 6666;
	
	// Use this for initialization
	void Start () {
        handler = OSCHandler.Instance;
        handler.Init();
        handler.transform.SetParent(gameObject.transform);

		reciever = new OSCReciever();
		reciever.Open(port);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            handler.SendMessageToClient("Max", "/carrier", 73);
            handler.SendMessageToClient("Max", "/modulator", 54);
            handler.SendMessageToClient("Max", "/index", 10000);
        }

		if(reciever.hasWaitingMessages()){
			OSCMessage msg = reciever.getNextMessage();
			Debug.Log(string.Format("message received: {0} {1}", msg.Address, DataToString(msg.Data)));
		}
	}
	
    private string DataToString(List<object> data) {
		string buffer = "";
		
		for(int i = 0; i < data.Count; i++)
		{
			buffer += data[i].ToString() + " ";
		}
		
		buffer += "\n";
		
		return buffer;
	}
}
