using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Animator myAnim;

    public void SetTrigger(string _triggerName)
    {
        myAnim.SetTrigger(_triggerName);
    }
}
