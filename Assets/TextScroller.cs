using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextScroller : MonoBehaviour
{

    float commentTimer = 30;
    float commentSpeed = 0;
    float commentMinSpeed = 0.8f;
    float commentMaxSpeed = 2f;

    float currentTimer = 0;

    TMP_Text text;
    
    
    // Start is called before the first frame update
    public void Init()
    {
        text = GetComponent<TMP_Text>();
        currentTimer += Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * commentSpeed);
        currentTimer += Time.deltaTime;

        if (currentTimer > commentTimer)
        {
            currentTimer = 0;
            gameObject.SetActive(false);
        }
    }

    public void EnableText(Vector3 position, string text)
    {
        transform.position = position;
        commentSpeed = Random.Range(commentMinSpeed, commentMaxSpeed);
        this.text.text = text;
        gameObject.SetActive(true);
    }

    public void ChangeFont(TMP_FontAsset font)
    {
        text.font = font;
        text.fontSize = 4;

    }
}
