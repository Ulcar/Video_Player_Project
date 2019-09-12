using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
  public  class YoutubeDLCaller
    {


   public static Queue<List<string>> links;
    public static IEnumerable GetLinks(string URL)
    {
        Process youtubeDL = new Process();
        youtubeDL.StartInfo.FileName = Application.dataPath + "Youtube-dl.exe";
        youtubeDL.StartInfo.Arguments = "-g " + URL;
        youtubeDL.Start();
        List<string> linkList = new List<string>();
        while (!youtubeDL.HasExited)
        {
            while (!youtubeDL.StandardOutput.EndOfStream)
            {
                linkList.Add(youtubeDL.StandardOutput.ReadLine());
            }
            yield return null;
        }
        links.Enqueue(linkList);
        foreach (string link in linkList)
        {
            UnityEngine.Debug.Log(link);
        }
       
    }

    }
