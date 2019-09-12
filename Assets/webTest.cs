using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

   public class webTest:MonoBehaviour
    {
    private static readonly HttpClient client = new HttpClient();
    [SerializeField]
    string URL;
    private void Start()
    {


        UnityWebRequest www = UnityWebRequest.Head(URL);
        www.SendWebRequest();
        while (!www.isDone) { }

        Debug.Log(www.GetResponseHeader("Content-Type"));

        if (www.GetResponseHeader("Content-Type").StartsWith("Audio"))
        {
            www.Dispose();
            UnityWebRequest audioDataRequest = UnityWebRequest.Get(URL);
            audioDataRequest.SendWebRequest();
            while (!audioDataRequest.isDone)
            {
               
            }
            byte[] data = audioDataRequest.downloadHandler.data;
            Debug.Log(data);
        }
    }



}
