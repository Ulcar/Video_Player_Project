using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommentTest : MonoBehaviour
{

    CommentManager manager;

    // Start is called before the first frame update
    private void OnEnable()
    {
        manager = GetComponent<CommentManager>();

        for (int i = 0; i < 20; i++)
        {
           Debug.Log(manager.MakeComment("hhhhhhh"));
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
