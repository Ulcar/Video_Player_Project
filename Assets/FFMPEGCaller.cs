using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using UnityEngine;
using System.IO;
   public class FFMPEGCaller
    {
    Queue<byte[]> dataQueue;
    public static IEnumerator GetAudio(byte[] data, int size)
    {
        Process ffmpeg = new Process();
        ffmpeg.StartInfo.FileName = Application.dataPath + "FFMPEG.exe";
        ffmpeg.StartInfo.UseShellExecute = false;
        ffmpeg.StartInfo.CreateNoWindow = true;
        ffmpeg.StartInfo.RedirectStandardInput = true;
        ffmpeg.StartInfo.RedirectStandardOutput = true;
        ffmpeg.StartInfo.Arguments = "-nostats -loglevel 0 -i - -f wav -";
        ffmpeg.Start();

        var ffmpegIn = ffmpeg.StandardInput.BaseStream;
        var ffmpegOut = ffmpeg.StandardOutput.BaseStream;
        int dataSent = 0;
     
        while (!ffmpeg.HasExited)
        {
            yield return null;
        }
       

    }

    IEnumerator SendAudioPipe(Stream stream, byte[] data , int size)
    {

        int dataSent = 0;
        while (dataSent < size)
        {
            if (size - dataSent > 1024)
            {
                stream.Write(data, 0, 1024);
                dataSent += 1024;
            }
            else
            {
                stream.Write(data, 0, size - dataSent);
                dataSent += size - dataSent;
            }

            yield return null;
        }

    }
}
