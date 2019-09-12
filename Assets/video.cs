using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using YoutubeExplode;
using YoutubeExplode.Models.MediaStreams;
using System.Threading.Tasks;
using FFmpeg.NET;
using System.Diagnostics;
using UnityEngine.Networking;
using UnityEngine.Events;
using Crosstales.FB;
using System;
public class video : MonoBehaviour
{
    VideoPlayer player;
    YoutubeClient client = new YoutubeClient();
    YoutubeExplode.Models.Video vid;
    VideoStreamInfo streamInfo;
    MediaStream h;
    CommandQueue queue;
    AudioSource audioSource;
    bool isPlayingAudio;
    VideoData currentVideo;
   public UnityEvent onStart = new UnityEvent();

    Queue<VideoData> data = new Queue<VideoData>();
    AudioClip retrievedClip;
    bool audioReady = false;
    bool isQueued;
    bool paused;
    string audioSavePath;
    string ffmpegPath;
    float audioPauseTime;




    // Start is called before the first frame update
    async void Start()
    {
        player = GetComponent<VideoPlayer>();
        audioSource = GetComponent<AudioSource>();
        audioSavePath = FileBrowser.OpenSingleFolder("open audio saving folder");
        ffmpegPath = FileBrowser.OpenSingleFile("exe");


        // await AddYoutubeVideo("https://www.youtube.com/watch?v=ECGnXTJLzE4&list=PL8wLRHtZnTEA2ZvsAth3BNn4I0eDrbO5w&index=1");
        //    await AddYoutubeVideo("https://www.youtube.com/watch?v=f0jJ2u2rk3k");
        //    await AddYoutubeVideo("https://www.youtube.com/watch?v=0PW5ZrZ3JCw");
        //    await AddYoutubeVideo("https://www.youtube.com/watch?v=QC8iQqtG0hg");
        //    await AddYoutubeVideo("https://www.youtube.com/watch?v=FWVTTbj08_w");
        //    await AddYoutubeVideo("https://www.youtube.com/watch?v=ucZl6vQ_8Uo");




    }

    public async Task<bool> AddYoutubeVideo(string videoURL)
    {


        string videoId = "";

        if (!GetVideoId(videoURL, out videoId))
        {
            return false;
        }

        if (videoURL.Contains("list="))
        {
            // get entire playlist
         string playlistID =   videoURL.Substring(videoURL.IndexOf("list=") + 5);
            int videoIndex = 1;
            if (playlistID.Contains("&index"))
            {
                videoIndex = int.Parse(playlistID.Substring(playlistID.IndexOf("&index") + 7));
                playlistID =     playlistID.Substring(0, playlistID.IndexOf("&index"));
                
            }

          YoutubeExplode.Models.Playlist playlist =   await client.GetPlaylistAsync(playlistID);
            if (playlist != null)
            {
                for (int i = videoIndex - 1; i < playlist.Videos.Count; i++)
                {
                  data.Enqueue(  await YoutubeVideoStuff(playlist.Videos[i].Id));
                }
            }
        }
        VideoData newVideo = await YoutubeVideoStuff(videoId);
        if (newVideo != null)
        {
            data.Enqueue(newVideo);
        }
        return newVideo != null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.isPlaying && !isQueued && !paused)
        {

            if (data.Count > 0)
            {
                onStart.Invoke();
                currentVideo = data.Dequeue();
                player.url = currentVideo.url;
                if (currentVideo.hasAudio)
                {
                    StartCoroutine(GetAudioClip(currentVideo.audiourl));
                }
                player.Prepare();
                isQueued = true;
            }
        }

        if (player.isPrepared && isQueued &&(!currentVideo.hasAudio || audioReady))
        {
                player.Play();
            if (currentVideo.hasAudio)
            {
                audioSource.clip = retrievedClip;
                audioSource.Play();
                UnityEngine.Debug.Log(player.frameRate);
            }
                isQueued = false;
        }
    }

    public void TogglePause()
    {
        if (!paused)
        {
            paused = true;
            audioPauseTime = audioSource.time;
            audioSource.Pause();
            
            player.Pause();
            
        }

        else
        {
            audioSource.Play();
            audioSource.time = audioPauseTime;
            player.Play();
            paused = false;
        }
    }

    public void Skip()
    {
        player.Stop();
        audioSource.Stop();
        paused = false;
    }



    async Task<VideoData> YoutubeVideoStuff(string videoId)
    {
        try
        {
            var streamInfoSet = await client.GetVideoMediaStreamInfosAsync(videoId);
            var streamInfo = streamInfoSet.Video.WithHighestVideoQuality();
            var audioInfo = streamInfoSet.Audio.WithHighestBitrate();
            player.source = VideoSource.Url;



            await client.DownloadMediaStreamAsync(audioInfo, audioSavePath + "/" + videoId + "." + audioInfo.Container);
            UnityEngine.Debug.Log("ffmpeg - i  " + audioSavePath + " / " + videoId + audioInfo.Container + Application.persistentDataPath + " / " + videoId + ".wav ");


            Engine ffmpeg = new Engine(ffmpegPath);
            MediaFile inputFile = new MediaFile(audioSavePath + " /" + videoId + "." + audioInfo.Container);
            MediaFile outputFile = new MediaFile(audioSavePath + " /" + videoId + ".wav");
            await ffmpeg.ConvertAsync(inputFile, outputFile);

            //     WWW kutAudio = new WWW("file:///" + Application.persistentDataPath + "/" + videoId + ".wav");


            // WWW kutAudio = new WWW("file:///C:/PATH/test.wav");
            UnityEngine.Debug.Log("file:///" + audioSavePath + " / " + videoId + ".wav");
            //   while (!kutAudio.isDone) { }
            // AudioClip kanker =  kutAudio.GetAudioClip(false, false, AudioType.WAV);
            //  AudioClip kanker = UnityWebRequestMultimedia.GetAudioClip("file:///" + Application.persistentDataPath + "/" + videoId + ".wav", AudioType.WAV);
            VideoData returnValue = new VideoData();
            returnValue.url = streamInfo.Url;
            //   returnValue.audio = kanker;
            returnValue.hasAudio = true;
            returnValue.audiourl = "file:///" + audioSavePath + "/" + videoId + ".wav";
            inputFile.FileInfo.Delete();
            // outputFile.FileInfo.Delete();
            return returnValue;


        }

        catch (Exception ex)
        {
            UnityEngine.Debug.Log(ex);
            return null;
        }


    }

    public void AddWebm(string url)
    {
        VideoData data = new VideoData();
        data.url = url;
        data.hasAudio = false;
        this.data.Enqueue(data);

    }

    public bool GetVideoId(string videoUrl, out string videoID)
    {
        if (!YoutubeClient.TryParseVideoId(videoUrl, out videoID))
        {
            return false;
        }

        return true;
    }

    IEnumerator GetAudioClip(string url)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV))
        {
            yield return www.Send();



               retrievedClip = DownloadHandlerAudioClip.GetContent(www);
                audioReady = true;
        }
    }


}

public class VideoData
{
   public string url;
    public bool hasAudio;
   public AudioClip audio;
    public string audiourl;
}
