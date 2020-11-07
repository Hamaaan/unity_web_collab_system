using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Switch_parts_fromNetworking : MonoBehaviour
{
    [SerializeField] bool DebugByInputField;

    [SerializeField] InputField[] Fields;

    [SerializeField] Text Log;

    List<string> LogLine = new List<string>();

    //webとの連携

    [SerializeField] Networking networking;

    string receiveData;


    [SerializeField] float ReceiveInterval = 15;

    float Interval = 0;

    
    List<string> DataBlocks = new List<string>();

	public int receivedDataBlocksCount = 0;
    
    //アニメーションを生成

    [SerializeField] int Stock_max = 64;

    int Stock = 0;

    [SerializeField] int Column_max = 8;

    int column = 0;

    int line = 0;

    float time = 0;

    [SerializeField, Range(0f, 3f)] float default_size = 1f;


    [SerializeField] float default_x = -2f;

    [SerializeField] float default_y = 2f;

    [SerializeField] float x_offset = 2f;

    [SerializeField] float y_offset = -2f;

    [SerializeField] float offsetTime = 1f;

    bool toggle_initialize = false;

	private int pre_currentID = 0;

	[SerializeField] int AllMachines_amount;

    [Range(0, 7)] [SerializeField] int thisMachine_number;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Fields.Length; i++)
        {
            Fields[i] = Fields[i].GetComponent<InputField>();
        }

        networking = networking.GetComponent<Networking>();

        Log = Log.GetComponent<Text>();
        
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (toggle_initialize)
        {
            FunctionInitialize();   //初期化ボタン用
        }

        keyControl();

        

        bool flag_nothing = false;

        if (DataBlocks.Count > 0)
        {
            //offsetTimeごとに繰り返し

            time += Time.deltaTime;

			if (time > offsetTime)
            {

                StockCounter();

                //DataBlocks[0]を呼び出して、後にRemoveAt[0]
                string[] data_id = SplitData(DataBlocks[0], '.');

                //DoorBaseの呼び出し
                SwitchParts("DoorBase", data_id[1], data_id[4], data_id[5], data_id[6]);

                //DoorHandleの呼び出し
                SwitchParts("DoorNob", data_id[2], "0", "0", "0");

                //windowの呼び出し
                SwitchParts("window", data_id[3], "0", "0", "0");


                LogLine.Add(DataBlocks[0]);

                

                if (LogLine.Count > 16)
                {
                    LogLine.RemoveAt(0);
                }

                string LogText = "ネットワークステータス : " + networking.network_status;

                LogText += "\nデバッグモード : ";

                if (DebugByInputField)
                {
                    LogText += "ON\n";
                }
                else
                {
                    LogText += "OFF\n";
                }

                LogText += "IDs : id.形.取っ手.窓.模様.ベース色.模様色";

                for (int i = 0; i < LogLine.Count; i++)
                {

                    LogText += "/\n" + LogLine[i];

                    if(i == (LogLine.Count - 1))
                    {
                        LogText += " <---the latest";
                    }
                }
                Log.text = LogText;
                
                DataBlocks.RemoveAt(0);            

                time = 0;

				Debug.Log("DataBlocks : " + DataBlocks.Count);
            }
            
            

        }
        else
        {
            flag_nothing = true;
        }

        //ReceiveIntervalごとに繰り返し
        //DataBlocksが0の時に以下の処理が行われる

        Interval += Time.deltaTime;

		if (Interval > ReceiveInterval 
		    &&
		    flag_nothing)
        {
            
            GetNetworkingData();
			Debug.Log("GetNetwork");
            

            Interval = 0;
        }



    }

    void GetNetworkingData()
    {
		//DataBlocks.Clear();
			
			receiveData = networking.responce;

            //デバッグモード
            if (DebugByInputField)
            {
                int length = Random.Range(0, 8);

                for (int i = 0; i < length; i++)
                {
                    string Door_ID = Random.Range(0, 5).ToString();
                    
                    string Handle_ID = Random.Range(0, 4).ToString();

                    string Window_ID = Random.Range(0, 3).ToString();

                    string tex_ID = Random.Range(0, 9).ToString();

                    string BaseColor_ID = Random.Range(0, 10).ToString();

                    string TexColor_ID = Random.Range(0, 10).ToString();


                    string randomData = "0." +
                                         Door_ID + "." +
                                           Handle_ID + "." +
                                             Window_ID + "." +
                                                tex_ID + "." +
                                                    BaseColor_ID + "." +
                                                        TexColor_ID + "/";

                    receiveData += randomData;
                }

            }
            //デバッグモードここまで

            //InputField分を追加

            for (int i = 0; i < Fields.Length; i++)
            {
                receiveData += Fields[i].text;
            }

            //DataBlocksにreceiveDataを変換入力

            int Block_size = SplitData(receiveData, '/').Length;
            
		    receivedDataBlocksCount = Block_size;
            
            if (Block_size > 0)
            {
                string[] BlockData = new string[Block_size];

                for (int i = 0; i < Block_size; i++)
                {
                    BlockData[i] = SplitData(receiveData, '/')[i];

                    DataBlocks.Add(BlockData[i]);
                    
                }

			DataBlocks.RemoveAt(DataBlocks.Count - 1);//ここの記述は、データをもらった際にかならず末尾に余計な/が入り、DataBlocksの数が実際のデータブロック数に対して狂うため、余りを予め削除する。

			networking.ID_updater(DataBlocks.Count);
            

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

    /*
     * 
     * parts→パーツの名前。DoorBaseかDoorNobが入る。
     * 
     * ID→各パーツごとに振り分けた番号。
     * 
     * Stock→画面に表示されているドアの数(のはず)。このクラスで宣言されているStockを利用。
     * 
     */

    void SwitchParts(string parts, string ID, string Tex_ID, string color1_ID, string color2_ID)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(parts);

        string path = parts + "s/" + ID;

        GameObject obj = Instantiate(Resources.Load(path) as GameObject);

        obj.name = obj.name + Stock.ToString();

        obj.transform.localScale = obj.transform.localScale* 0.25f * default_size;

        obj.transform.position = new Vector3(default_x + column * x_offset, 
                                                default_y + line * y_offset, 0);

        switch_Texture_color stc = obj.GetComponent<switch_Texture_color>();

        stc.Texture_ID = int.Parse(Tex_ID);

        stc.BaseColor_ID = int.Parse(color1_ID);

        stc.TextureColor_ID = int.Parse(color2_ID);

        if (objects.Length >= Stock_max)
        {
            Destroy(GameObject.FindGameObjectWithTag(parts));
        }

    }

    void StockCounter() 
    {
        if (column >= Column_max)
        {
            column = 0;
            line++;
        }

        if (Stock >= Stock_max)
        {
            Stock = 0;
            line = 0;

        }

        column++;
        
        Stock++;
    }

    

    

    public void OnClick_Initialize()
    {
        toggle_initialize = true;
    }

    void FunctionInitialize()
    {
        Interval = 0;

        Stock = 0;

        column = 0;

        line = 0;

        time = 0;

        GameObject[] Targets1 = GameObject.FindGameObjectsWithTag("DoorBase");
        GameObject[] Targets2 = GameObject.FindGameObjectsWithTag("DoorNob");
        GameObject[] Targets3 = GameObject.FindGameObjectsWithTag("window");


        for (int i = 0; i < Targets1.Length; i++)
        {
            Destroy(Targets1[i]);
        }
        for (int i = 0; i < Targets2.Length; i++)
        {
            Destroy(Targets2[i]);
        }
        for (int i = 0; i < Targets3.Length; i++)
        {
            Destroy(Targets3[i]);
        }


        toggle_initialize = false;
    }

    //デバッグ用の記述

    void keyControl()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StockCounter();

            for (int i = 0; i < Fields.Length; i++)
            {

            }

        }
    }

    public void OnClick_DebugMode()
    {
        if(DebugByInputField)
        {
            DebugByInputField = false;
        }
        else
        {
            DebugByInputField = true;
        }
    }

    void DebugLogArray(string[] strings)
    {
        for (int i = 0; i < strings.Length; i++)
        {
            Debug.Log(strings[i]);
        }
    }

    void DebugLogList(List<string> strings)
    {
        for (int i = 0; i < strings.Count; i++)
        {
            Debug.Log(strings[i]);
        }
    }
}
