using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Networking : MonoBehaviour
{
    string URL;

    //https://geikosai2020-staging.herokuapp.com/staffonly?task=door_nums_debug
    //https://geikosai2020-staging.herokuapp.com/staffonly?task=door_nums&last_door=

    public string responce = "NaN";

    public string network_status = "NaN";

    [SerializeField] InputField field;

    [SerializeField] Text LogText;

	[SerializeField] bool Debugmode;

	public int currentID = 0;

	[SerializeField] int requestInterval = 2;

    [SerializeField] int AllMachines_amount;

    [Range(0, 7)] [SerializeField] int thisMachine_number;

    List<string> DataBlocks = new List<string>();


    // Start is called before the first frame update
    void Start()
    {
		URL = "https://www.geikosai2020.jp/staffonly?task=door_nums&pc_id=";



        StartCoroutine(Connection());

        LogText = LogText.GetComponent<Text>();

        field = field.GetComponent<InputField>();


    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ConnectionStart()
    {
        StartCoroutine(Connection());
    }

    private IEnumerator Connection()
    {
        while (true)
        {
            string currentURL = URL + thisMachine_number + "&last_door=" + currentID;

            Debug.Log(currentURL);

			UnityWebRequest request = UnityWebRequest.Get(currentURL);

            // リクエスト送信
			request.SendWebRequest();

			yield return new WaitForSeconds(requestInterval);

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
						string Data = "";

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

                                Data += randomData;
                            }

						if(Probability(50))
						{
							Data = "";
						}

						responce = Data;
                    }

                    DataBlocks.Clear();
                    processData();

                    if(DataBlocks.Count > 0)
                    {
                        //DataBlockの末尾のidを代入
                        string[] lastData = SplitData(DataBlocks[DataBlocks.Count - 1], '.');
                        currentID = int.Parse(lastData[0]);
                        currentID++;
                        Debug.Log(currentID);
                    }


                    LogText.text += "\n" + network_status + "current ID : " + currentID;

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


	public void ID_updater(int DataBlocksCount)
	{
		currentID += DataBlocksCount;
	}

    void processData()
    {
        int Block_size = SplitData(responce, '/').Length;

        if (Block_size > 0)
        {
            string[] BlockData = new string[Block_size];

            for (int i = 0; i < Block_size; i++)
            {
                BlockData[i] = SplitData(responce, '/')[i];

                DataBlocks.Add(BlockData[i]);

            }

            DataBlocks.RemoveAt(DataBlocks.Count - 1);//ここの記述は、データをもらった際にかならず末尾に余計な/が入り、DataBlocksの数が実際のデータブロック数に対して狂うため、余りを予め削除する。

        }
    }

    string[] SplitData(string input, char sep)
    {
        string[] arr = input.Split(sep);

        string[] output = new string[arr.Length];

        for (int i = 0; i < arr.Length; i++)
        {
            output[i] = arr[i];
        }

        return output;

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
