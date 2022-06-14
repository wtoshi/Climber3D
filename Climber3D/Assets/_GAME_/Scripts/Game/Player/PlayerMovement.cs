using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.OnTouchEvent.AddListener(ClimbTo);   
    }
    private void OnDisable()
    {
        EventManager.OnTouchEvent.RemoveListener(ClimbTo);
    }

    void ClimbTo(TouchableData _data)
    {

        //TODO Climb to point
    }

}
