using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comment : MonoBehaviour
{

    List<TextScroller> commentTexts = new List<TextScroller>();

    float currentTime = 1;
    [SerializeField]
    float coolDownTime = 1;
    
    
    // Start is called before the first frame update
    void Start()
    {
      commentTexts.AddRange(  GetComponentsInChildren<TextScroller>(true));
        foreach (TextScroller scroller in commentTexts)
        {
            scroller.Init();
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
    }


    public bool DoComment(string text, bool ignoreCooldown = false)
    {
        if (currentTime >= coolDownTime || ignoreCooldown)
        {
            currentTime = 0;
            TextScroller scroller = GetFirstInactiveTextScroller();
            if (scroller != null)
            {
                scroller.EnableText(transform.position, text);
                return true;
            }
        }
        return false;
    }


    TextScroller GetFirstInactiveTextScroller()
    {
        foreach (TextScroller scroller in commentTexts)
        {
            if (!scroller.gameObject.activeSelf)
            {
                return scroller;
            }
        }
        return null;
    }

    public void ChangeFont(TMPro.TMP_FontAsset font)
    {
        foreach (TextScroller scroller in commentTexts)
        {
            scroller.ChangeFont(font);
        }
    }
}


