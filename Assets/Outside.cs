using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Outside : MonoBehaviour
{
    public UnityEvent OnBallEnter = new UnityEvent();

    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.CompareTag(tag: "Ball"))
        {
            OnBallEnter.Invoke();
        }
    }
}
