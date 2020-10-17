using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplitText : MonoBehaviour
{
    [SerializeField] Text[] text;

    [SerializeField] InputField Field;

    int multiple = 0;

    bool trigger = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < text.Length; i++)
        {
        
            text[i] = text[i].GetComponent<Text>();
        }

        Field = Field.GetComponent<InputField>();
    }

    /*
        テスト用
12.4.22.7./13.5.23.8./14.6.24.9./15.3.25.10./

        
     */

    // Update is called once per frame
    void Update()
    {

        



        string[] result1 = SplitData(Field.text, '/');
        DebugLogArray(result1, result1.Length);

        for (int i = 0; i < result1.Length; i++)
        {
            string[] result2 = SplitData(result1[i], '.');
        }
        
    }

    void DebugLogArray(string[] strings, int length)
    {
        for (int i = 0; i < length; i++)
        {
            Debug.Log(strings[i]);
        }
    }

    //

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
}
