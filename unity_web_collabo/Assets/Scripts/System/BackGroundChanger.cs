using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundChanger : MonoBehaviour
{
    [SerializeField] Camera MainCamera;
    
    bool toggle = false;

    // Start is called before the first frame update
    void Start()
    {
        MainCamera.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if (!toggle)
        {
            MainCamera.backgroundColor = new Color(0, 1, 0, 1);
            toggle = true;
        }
        else
        {
            MainCamera.backgroundColor = new Color(0, 0, 0, 1);
            toggle = false;
        }
    }
}
