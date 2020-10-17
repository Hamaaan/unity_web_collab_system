using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimator : MonoBehaviour
{
    Animator Animator;

    // Start is called before the first frame update
    void Start()
    {
        Animator = this.gameObject.GetComponent<Animator>();

        Animator.SetBool("turn", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
