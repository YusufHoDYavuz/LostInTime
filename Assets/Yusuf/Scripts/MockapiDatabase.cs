using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class MockapiDatabase : MonoBehaviour
{
    private string URL = "https://64b1704a062767bc48263498.mockapi.io/lostintime";

    [SerializeField] private TextMeshPro userName;
    [SerializeField] private int userID;
    
    private void Start()
    {
        StartCoroutine(GetDatas());
    }

    private IEnumerator GetDatas()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(URL))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
                Debug.LogError(request.error);
            else
            {
                string json = request.downloadHandler.text;
                SimpleJSON.JSONNode stats = SimpleJSON.JSON.Parse(json);

                userName.text = "Karakter ismi: " + stats[userID]["userName"];
            }
        }
    }
}
