using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Dynamics;

public class PlayerRagdoll : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] PuppetMaster myRagdoll;
    [SerializeField] Transform pelvis;

    public Transform Pelvis => pelvis;

    public void SetPinWeight(float _weight)
    {
        myRagdoll.pinWeight = _weight;
    }
}
