using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class BackendHandler : MonoBehaviour
{
    public TMPro.TMP_Text highScoresTextArea;
    bool updateHighScoreTextArea = false;
    const string urlBackendHighScores = "https://niisku.lab.fi/~konoy/RoomForDistraction/api/highscores.php";

    private HighScoreList hs;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("BackendHandler started");
        StartCoroutine(GetRequestForHighScores(urlBackendHighScores));
    }

    // Update is called once per frame
    void Update()
    {
        //logTextArea.text = log;
        if (updateHighScoreTextArea && highScoresTextArea != null)
        {
            highScoresTextArea.text = CreateHighScoreList();
            updateHighScoreTextArea = false;
        }
    }
    string CreateHighScoreList()
    {
        string hsList = "";
        if (hs != null)
        {
            int len = (hs.scores.Length < 5) ? hs.scores.Length : 5; for (int i = 0; i < len; i++)
            {
                /* hsList += hs.scores[i].playername + ": \t" + hs.scores[i].score + " \t" + hs.scores[i].playtime+"\n"; */
                float seconds = hs.scores[i].mstime / 1000f;
                hsList += string.Format("[ {0} ] | {1,-15} | {2,5} | {3,-15:F2}s\n", (i + 1),
                    hs.scores[i].playerName,
                    hs.scores[i].score,
                    seconds);
            }
        }
        return hsList;
    }

    // Get high scores from backend
    IEnumerator GetRequestForHighScores(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // set downloadHandler for json
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");
            // Request and wait for reply
            yield return webRequest.SendWebRequest();
            
            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                // get raw data and convert it to string
                string resultStr = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data);
                // create HighScore item from json string
                hs = JsonUtility.FromJson<HighScoreList>(resultStr);
                updateHighScoreTextArea = true;
                Debug.Log("Received(UTF8): " + resultStr);
                Debug.Log("Received(HS): " + JsonUtility.ToJson(hs));
                Debug.Log("Received(HS) name: " + hs.scores[0].playerName);
            }
        }
    }

    // method to get high scores
    public HighScoreList GetHighScores()
    {
        return hs;
    }

    public void PostGameResults(HighScore scores)
    {
        Debug.Log("highScore: " + JsonUtility.ToJson(scores));
        StartCoroutine(PostRequestForHighScores(urlBackendHighScores, scores));
    }

    // post high score to backend
    IEnumerator PostRequestForHighScores(string uri, HighScore scores)

    {
        using (UnityWebRequest webRequest = UnityWebRequest.PostWwwForm(uri, JsonUtility.ToJson(scores)))
        {
            Debug.Log("Posting high score to backend: " + JsonUtility.ToJson(scores));
            Debug.Log("POST request sent to " + uri);
            // set downloadHandler for json
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");
            // Request and wait for reply
            yield return webRequest.SendWebRequest();
            // get raw data and convert it to string
            string resultStr = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data);
            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Error in post request: " + webRequest.error);
            }
            else
            {
                Debug.Log("Received(UTF8): " + resultStr);

                // Notify GameManager to sync local cache
                GameManager.Instance.highScoreManager.SyncWithBackend();
            }
        }
    }

}
