using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Networking : MonoBehaviour
{
    public string URL = "https://gesk=door_nums_debug";

    //https://geikosai2020-staging.herokuapp.com/staffonly?task=door_nums_debug
    //https://geikosai2020-staging.herokuapp.com/staffonly?task=door_nums&last_door=

    public string responce = "NaN";

    public string network_status = "NaN";

    [SerializeField] InputField field;

    [SerializeField] Text LogText;

	[SerializeField] bool Debugmode;

	[SerializeField]int currentID = 0;

	[SerializeField] int thisMashine;

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
        //URL = field.text;
    }

    public void ConnectionStart()
    {
        StartCoroutine(Connection());
    }

    private IEnumerator Connection()
    {
        while (true)
        {
			string currentURL = URL + currentID;

			UnityWebRequest request = UnityWebRequest.Get(currentURL);

            // リクエスト送信
			request.SendWebRequest();

			yield return new WaitForSeconds(2);

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



                    //デバッグモード
					if(Debugmode)
					{
						string receiveData = "";

                        int length = Random.Range(0, 8);

                            for (int i = 0; i < length; i++)
                            {
                                string tex_ID = Random.Range(0, 5).ToString();

                                string BaseColor_ID = Random.Range(0, 5).ToString();

                                string TexColor_ID = Random.Range(0, 5).ToString();


                                string randomData = "0.0." +
                                                        tex_ID + "." +
                                                            BaseColor_ID + "." +
                                                                TexColor_ID + "/";

                                receiveData += randomData;
                            }

						if(Probability(50))
						{
							receiveData = "";
						}

						responce = receiveData;
                    }

					LogText.text += "\n" + network_status + "current ID : " + currentID;

                    Debug.Log(currentID);

					if(responce != "")
					{
						currentID++;
					}


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


	public static bool Probability(float fPercent)
    {
        float fProbabilityRate = UnityEngine.Random.value * 100.0f;

        if (fPercent == 100.0f && fProbabilityRate == fPercent)
        {
            return true;
        }
        else if (fProbabilityRate < fPercent)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
