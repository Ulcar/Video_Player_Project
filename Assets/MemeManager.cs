using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class MemeManager : MonoBehaviour
{
    [SerializeField]
    GameObject MemePrefab;

    [SerializeField]
    GameObject gifMemePrefab;

    [SerializeField]
    List<Transform> StartPositions;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



   public void PostImage(string url)
    {
        StartCoroutine(GetText(url));
    }



    public void PostGif(string url)
    {
        GameObject tmp = Instantiate(gifMemePrefab);
        tmp.GetComponent<RectTransform>().anchoredPosition = StartPositions[Random.Range(0, StartPositions.Count - 1)].position;
        UniGifImage m_uniGifImage = tmp.GetComponent<UniGifImage>();
        StartCoroutine(m_uniGifImage.SetGifFromUrlCoroutine(url));
    }


    public void SpawnMeme(Sprite meme)
    {
     GameObject tmp =    Instantiate(MemePrefab);
        tmp.GetComponent<MemeScroller>().Init();
        tmp.GetComponent<MemeScroller>().EnableMeme(StartPositions[Random.Range(0, StartPositions.Count - 1)].position, meme);
    }


    IEnumerator GetText(string url)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                // Get downloaded asset bundle
                var texture = DownloadHandlerTexture.GetContent(uwr);
              Sprite sprite =  Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 200);
                SpawnMeme(sprite);
            }
        }
    }
}
