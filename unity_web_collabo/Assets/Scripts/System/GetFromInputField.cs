using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetFromInputField : MonoBehaviour
{


    [SerializeField] InputField Field1;
    [SerializeField] Text Text1;

    public string FieldName;

    // Start is called before the first frame update
    void Start()
    {
        Field1 = Field1.GetComponent<InputField>();
        Text1 = Text1.GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ValueChanged()
    {
        Text1.text = Field1.text;

        //InputValueToProcess();

        

    }

    

    public int GetInputValue()
    {
        int value = int.Parse(Field1.text);

        FieldName = Field1.name;

        Debug.Log(Field1.name);

        return value;

    }
}
