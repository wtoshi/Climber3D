using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] PlayerRagdoll playerRagdoll;
    [SerializeField] PlayerAnimations playerAnimations;
    //[SerializeField] PlayerIK playerIK;



    public PlayerMovement PlayerMovement => playerMovement;
    public PlayerInput PlayerInput => playerInput;
    public PlayerRagdoll PlayerRagdoll => playerRagdoll;
    public PlayerAnimations PlayerAnimations => playerAnimations;
    //public PlayerIK PlayerIK => playerIK;


    public void ResetParentPosition()
    {
        List<Vector3> childLocals = new List<Vector3>();

        for (int i = 0; i < transform.childCount; i++)
        {
            childLocals.Add(transform.GetChild(i).localPosition);
        }

        var newPos = playerRagdoll.Pelvis.position;
        newPos.z = transform.position.z;

        var childPos = newPos - transform.position;
        transform.position = newPos;

        transform.SetPositionAndRotation(newPos, transform.rotation);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localPosition -= childPos;
        }
    }
}
