using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour
{
    public UnityEvent OnBallGoalEnter = new UnityEvent();

   private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tag: "Ball"))
        {
            OnBallGoalEnter.Invoke();
        }
    }
}
