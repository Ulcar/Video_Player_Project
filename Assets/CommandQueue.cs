using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;
using System.Net;
using System.Net.Http;
using UnityEngine.Networking;
public class CommandQueue:MonoBehaviour
    {

    public Queue<string> commentQueue = new Queue<string>();
    [SerializeField]
    video videoPlayer;

    [SerializeField]
    CommentManager comments;

    [SerializeField]
    MemeManager memes;

    float currentTime = 0;


    List<System.Net.IPEndPoint> votedUsers = new List<System.Net.IPEndPoint>();
    int votedUserCount = 0;

    [SerializeField]
    TMP_Text text;


    [SerializeField]
    List<TMP_FontAsset> fonts = new List<TMP_FontAsset>();
    HttpClient webClient;


    private void Start()
    {
        LaatJeLikken.onCloseEvent.AddListener(CheckVoteSkip);
        videoPlayer.onStart.AddListener(ClearVoteSkip);
    }

    private void Update()
    {
        currentTime += Time.deltaTime;


        currentTime = 0;
        CheckCommand();
        CheckCommand();
        CheckCommand();
        if (commentQueue.Count > 0)
        {
            comments.MakeComment(commentQueue.Dequeue());
        }

    }

    void CheckVoteSkip()
    {
        if ((votedUserCount >= Math.Ceiling((double)LaatJeLikken.userCount / 2)) || (LaatJeLikken.userCount == 1 && LaatJeLikken.userCount == votedUserCount) )
        {
            videoPlayer.Skip();
            votedUserCount = 0;
            votedUsers.Clear();
        }
    }

   public void ClearVoteSkip()
    {
        votedUserCount = 0;
        votedUsers.Clear();
    }


    ContentType CheckHeaderInfo(string URL)
    {

        UnityWebRequest www = UnityWebRequest.Head(URL);
        www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            return ContentType.Invaild;
        }
        while (!www.isDone) { }

        if (www.GetResponseHeader("Content-Type").StartsWith("image"))
        {

            if (www.GetResponseHeader("Content-Type").Contains("gif"))
            {
                return ContentType.Gif;
            }

            return ContentType.Image;
        }

        else if (www.GetResponseHeader("Content-Type").StartsWith("video"))
        {
            return ContentType.Video;
        }

        else if (www.GetResponseHeader("Content-Type").StartsWith("text"))
        {
           
            return ContentType.Text;
        }

        else
        {
            return ContentType.Invaild;
        }

    }


    void CheckCommand()
    {
        if (LaatJeLikken.commands.Count > 0)
        {
            ClientData command = LaatJeLikken.commands.Dequeue();
            Command parsedCommand = JsonConvert.DeserializeObject<Command>(command.Data);
            Debug.Log("Data sent: " + parsedCommand.data);
            Debug.Log(parsedCommand.command);

            if (parsedCommand.command == Command.CommandType.TextInput)
            {
                CheckTextInput(parsedCommand.data);
            }

            else if (parsedCommand.command == Command.CommandType.Pause)
            {
                videoPlayer.TogglePause();
            }

            else if (parsedCommand.command == Command.CommandType.VoteSkip)
            {
                if (!votedUsers.Contains(command.address))
                {
                    votedUsers.Add(command.address);
                    votedUserCount++;
                    text.text = votedUserCount.ToString() + " / " + LaatJeLikken.userCount + " voted skip";
                    text.gameObject.SetActive(true);
                    CheckVoteSkip();
                }
            }

        }

       

        }



    void CheckTextInput(string input)
    {

        string filterInput = input.ToLower();


        if (Uri.IsWellFormedUriString(input, UriKind.Absolute))
        {
            ContentType content = CheckHeaderInfo(input);
            Debug.Log(content);
            switch (content)
            {
                case ContentType.Image:

                    memes.PostImage(input);



                    break;
                case ContentType.Gif:
                    memes.PostGif(input);
                    break;
                case ContentType.Video:
                    videoPlayer.AddWebm(input);
                    break;
                case ContentType.Text:
                    // attempt Youtube-dl Shit
                    //TODO: Put Youtube-dl on a server, and get link data from there
                    YoutubeDLCaller.GetLinks(input);
                    break;

            }
        }



        else if (input.Contains("218"))
        {

            //do 218 memes
            Comment(input);
        }

     

        else if (filterInput.StartsWith("changefont"))
        {
            string data = filterInput.Substring(filterInput.IndexOf("changefont") + 11);
            if (data == "comic sans")
            {
                comments.ChangeFont(fonts[1]);
            }

            else if (data == "papyrus")
            {
                comments.ChangeFont(fonts[2]);
            }

            else if (data == "persona")
            {
                comments.ChangeFont(fonts[3]);
            }

            else
            {
                comments.ChangeFont(fonts[0]);
            }
        }

        else if (filterInput.Contains("joep's moeder") || filterInput.Contains("joeps moeder") || filterInput.Contains("sophie's schoonmoeder") || filterInput.Contains("sophies schoonmoeder"))
        {
            //play MGS joep's moeder
            Comment(input);
        }

        else
        {
            Comment(input);
        }
    }


    void Comment(string input)
    {
        if (!comments.MakeComment(input))
        {
            commentQueue.Enqueue(input);
        }
    }
}


public class Command
{
    public enum CommandType
    {
        TextInput,
        Pause,
        VoteSkip
    }
  public  CommandType command;
  public  string data;
}


public enum ContentType
{
    Image,
    Gif,
    Video,
    Text,
    Invaild
}