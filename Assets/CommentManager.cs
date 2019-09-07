using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommentManager : MonoBehaviour
{
    // Start is called before the first frame update

    List<Comment> comments = new List<Comment>();

    void Start()
    {


        comments.AddRange(GetComponentsInChildren<Comment>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool MakeComment(string comment)
    {
        foreach (Comment commentHandler in comments)
        {
            if (commentHandler.DoComment(comment))
            {
                return true;
            }
        }
        return false;
    }

    public void ChangeFont(TMPro.TMP_FontAsset font)
    {
        foreach (Comment comment in comments)
        {
            comment.ChangeFont(font);
        }
    }
}
