using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using UnityEngine;

   public class webTest:MonoBehaviour
    {
    private static readonly HttpClient client = new HttpClient();
    [SerializeField]
    string URL;
    private async void Start()
    {
        HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Head, URL);
     HttpResponseMessage response =   await client.SendAsync(msg);
        Debug.Log(response.Content.Headers.ContentType);
    }
}
