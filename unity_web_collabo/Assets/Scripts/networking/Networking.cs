using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Networking : MonoBehaviour
{
    static public string URL = "https://gesk=door_nums_debug";

    //https://geikosai2020-staging.herokuapp.com/staffonly?task=door_nums_debug
    //https://geikosai2020-staging.herokuapp.com/staffonly?task=door_nums&last_door=

    public string responce = "NaN";

    public string network_status = "NaN";

    [SerializeField] InputField field;

    [SerializeField] Text LogText;

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(Connection());

        LogText = LogText.GetComponent<Text>();

        field = field.GetComponent<InputField>();

    }

    // Update is called once per frame
    void Update()
    {
        URL = field.text;
    }

    public void ConnectionStart()
    {
        StartCoroutine(Connection());
    }

    private IEnumerator Connection()
    {
        while (true)
        {
            UnityWebRequest request = UnityWebRequest.Get(URL);

            // リクエスト送信
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log("エラー:" + request.error);
            }
            else
            {
                if (request.responseCode == 200)
                {
                    network_status = "Success.";

                    Debug.Log(request.downloadHandler.text);

                    responce = request.downloadHandler.text;

                    LogText.text += "\n"+network_status;
                }
                else
                {

                    network_status = "Failed... : " + request.responseCode;

                    LogText.text += "\n"+network_status;

                    responce = "E.x.a.m.p./l.e.E.x.a./m.p.l.e.E./x.a.m.p.l./";
                }
            }
        }

        
    }


}
