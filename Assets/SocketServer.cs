using System;
using WebSocketSharp;
using WebSocketSharp.Server;
using UnityEngine;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Events;
using Crosstales.FB;
public class LaatJeLikken : WebSocketBehavior
{


    public static Queue<ClientData>  commands = new Queue<ClientData>();
    public static int userCount = 0;

    public static UnityEvent onCloseEvent = new UnityEvent();

    protected override void OnMessage(MessageEventArgs e)
    {


        Send(JsonConvert.SerializeObject(new ServerToClientMessage("bruh")));
        commands.Enqueue(new ClientData(Context.UserEndPoint, e.Data));

    }

    protected override void OnOpen()
    {
        base.OnOpen();
        userCount += 1;
        Send(JsonConvert.SerializeObject(new ServerToClientMessage("connected")));
    }

    protected override void OnClose(CloseEventArgs e)
    {
        base.OnClose(e);
        userCount -= 1;
        onCloseEvent.Invoke();
    }
}
public class SocketServer:MonoBehaviour
    {

    WebSocketServer wssv;
    [SerializeField]
    UnityEngine.UI.InputField input;

    [SerializeField]
    GameObject ipInputCanvas;







    private void Start()
    {
        
       
    }

    private void Update()
    {
        
    }

    public void OnIpButtonPressed()
    {
        System.Net.IPAddress address;
       if (System.Net.IPAddress.TryParse(input.text, out address)) {
            Debug.Log("Got here");
            wssv = new WebSocketServer(address, 24669, false);
            wssv.AddWebSocketService<LaatJeLikken>("/LaatJeLikken");
            WebSocketServiceHost h;
            wssv.KeepClean = false;
            wssv.Start();
            ipInputCanvas.SetActive(false);
        }
    }

    private void OnApplicationQuit()
    {
        wssv.Stop();
    }
}


public class ServerToClientMessage
{
   public string data;

   public ServerToClientMessage(string message)
    {
        data = message;
    }
}

public class ClientData
{
  public System.Net.IPEndPoint address { get; private set; }
   public string Data { get; private set; }
    public ClientData(System.Net.IPEndPoint adress, string data)
    {
        address = adress;
        this.Data = data;
    }

}
