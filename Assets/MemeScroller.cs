using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

   public class MemeScroller:MonoBehaviour
    {

        float commentTimer = 30;
        float commentSpeed = 0;
        float commentMinSpeed = 0.8f;
        float commentMaxSpeed = 2f;

        float currentTimer = 0;
    bool scrollEnabled = false;

        SpriteRenderer meme;


        // Start is called before the first frame update
        public void Init()
        {
            meme = GetComponent<SpriteRenderer>();
        }

    // Update is called once per frame
    void Update()
    {
        if (scrollEnabled)
        {
            transform.Translate(Vector3.left * Time.deltaTime * commentSpeed);
            currentTimer += Time.deltaTime;

            if (currentTimer > commentTimer)
            {
                currentTimer = 0;
                Destroy(gameObject);
            }
        }
    }

        public void EnableMeme(Vector3 position, Sprite meme)
        {
            transform.position = position;
            commentSpeed = UnityEngine.Random.Range(commentMinSpeed, commentMaxSpeed);
            this.meme.sprite = meme;
            gameObject.SetActive(true);
        scrollEnabled = true;
        }

    public void EnableMeme()
    {
        commentSpeed = UnityEngine.Random.Range(commentMinSpeed, commentMaxSpeed);
        gameObject.SetActive(true);
        scrollEnabled = true;
    }
    }
