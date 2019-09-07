using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;
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

        if (input.Contains("youtube.com") || input.Contains("youtu.be"))
        {
            videoPlayer.AddYoutubeVideo(input);
        }
        else if (filterInput.EndsWith(".webm") || filterInput.EndsWith(".mp4"))
        {
            videoPlayer.AddWebm(input);
        }

        else if (input.Contains("218"))
        {

            //do 218 memes
            Comment(input);
        }

        else if (filterInput.EndsWith(".png") || filterInput.EndsWith(".jpg"))
        {
            memes.PostImage(input);
        }

        else if (input.EndsWith(".gif") || input.EndsWith(".gifv"))
        {
            memes.PostGif(input);
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
