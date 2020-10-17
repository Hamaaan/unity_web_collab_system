using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Input_to_Text : MonoBehaviour
{
    [SerializeField] Networking net;

    [SerializeField] Text textObj;
    string req = "NaN";

    [SerializeField] bool isDebug;

    [SerializeField] Input_from Input_From;

    // Start is called before the first frame update
    void Start()
    {
        textObj = textObj.GetComponent<Text>();

        net = net.GetComponent<Networking>();

        //Input_From = Input_From.GetComponent<Input_from>();
    }

    // Update is called once per frame
    void Update()
    {
        req = net.responce;

        if (!isDebug)
        {
            //req = Input_From.DebugText;

        }

        textObj.text = "request.downloadHandler.text : " + req;

    }
}
