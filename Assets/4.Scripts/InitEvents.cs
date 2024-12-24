using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InitEvents : MonoBehaviour
{
    public UnityEvent initEvents;
    public UnityEvent AnimationEvents;

    void Start()
    {
        initEvents.Invoke();
    }

    public void CallEventByAnimation()
    {

        AnimationEvents.Invoke();
    }
}
