using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Input_from : MonoBehaviour
{
    [SerializeField] InputField[] Field;

    public string DebugText;


    // Start is called before the first frame update
    void Start()
    {
        Field = new InputField[Field.Length];

        for (int i = 0; i < Field.Length; i++)
        {
            Field[i] = Field[i].GetComponent<InputField>();
        }


    }

    // Update is called once per frame
    void Update()
    {

        DebugText = Field[0].text;



    }
}
