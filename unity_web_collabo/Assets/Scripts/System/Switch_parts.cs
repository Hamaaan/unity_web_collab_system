using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Switch_parts : MonoBehaviour
{

    [SerializeField] InputField[] Fields;

    [SerializeField] Networking networking;

    string receiveData;

    GameObject[] gameObjects = new GameObject[4];

    [SerializeField] int Stock_max = 8;

    int Stock = 0;

    float time = 0;

    [SerializeField] float default_x = -2f;

    [SerializeField] float x_offset = 2f;

    [SerializeField] float offsetTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Fields.Length; i++)
        {
            Fields[i] = Fields[i].GetComponent<InputField>();
        }

        networking = networking.GetComponent<Networking>();
    }

    // Update is called once per frame
    void Update()
    {
        keyControl();

        //offsetTimeごとに繰り返し

        time += Time.deltaTime;

        if (time > offsetTime)
        {
            StockCounter();

            SwitchParts("DoorBase", "1");

            SwitchParts("DoorNob", "1");

            time = 0;
        }

    }

    void GetNetworkingData()
    {
        receiveData = networking.responce;

        int Block_size = SplitData(receiveData, '/').Length;

        if (Block_size > 0)
        {
            string[] BlockData = new string[Block_size];

            for (int i = 0; i < Block_size; i++)
            {
                BlockData[i] = SplitData(receiveData, '/')[i];
            }

            DebugLogArray(BlockData);
        }

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

    void SwitchParts(string parts, string ID)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(parts);

        string path = parts + "s/" + ID;

        GameObject obj = Instantiate(Resources.Load(path) as GameObject);

        obj.name = obj.name + Stock.ToString();

        obj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        obj.transform.position = new Vector3(default_x + Stock * x_offset, 0, 0);

        if (objects.Length >= Stock_max * 2)
        {
            Destroy(GameObject.FindGameObjectWithTag(parts));
        }

    }

    void StockCounter()
    {
        if (Stock < Stock_max)
        {

        }
        else
        {
            Stock = 0;
        }

        Stock++;
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


    //デバッグ用の記述

    void keyControl()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StockCounter();

            for (int i = 0; i < Fields.Length; i++)
            {
                SwitchParts(Fields[i].name, Fields[i].text);
            }

        }
    }

    void DebugLogArray(string[] strings)
    {
        for (int i = 0; i < strings.Length; i++)
        {
            Debug.Log(strings[i]);
        }
    }
}
