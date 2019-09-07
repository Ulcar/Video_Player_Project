using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFade : MonoBehaviour
{

    float fadeTimer = 3;
    float currentTime = 0;
    float startFadeTime = 1;
    TMPro.TMP_Text text;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        currentTime = 0;
        text = GetComponent<TMPro.TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > startFadeTime)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, (1 - ((currentTime - startFadeTime) / fadeTimer)));
        }
        if (currentTime > fadeTimer)
        {
            gameObject.SetActive(false);
        }
    }


}
