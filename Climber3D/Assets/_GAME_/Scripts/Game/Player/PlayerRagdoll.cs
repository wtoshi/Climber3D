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

    private void Start()
    {
        FirstState();
    }

    void FirstState()
    {
        myRagdoll.mappingWeight = 0f;
    }

    public void SetForGame()
    {
        myRagdoll.mappingWeight = 1f;
        SetPinWeight(0);
    }

    void SetPinWeight(float _weight)
    {
        myRagdoll.pinWeight = _weight;
    }

    void EnableRagdoll(bool _mode)
    {
        PuppetMaster.State state = _mode ? PuppetMaster.State.Alive : PuppetMaster.State.Dead;
        myRagdoll.state = state;
    }
}
