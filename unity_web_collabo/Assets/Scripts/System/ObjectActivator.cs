using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActivator : MonoBehaviour
{
    [SerializeField] GameObject TargetObj;

    bool toggle = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if (!toggle)
        {
            TargetObj.SetActive(true);
            toggle = true;
        }
        else
        {
            TargetObj.SetActive(false);
            toggle = false;
        }
    }
}
