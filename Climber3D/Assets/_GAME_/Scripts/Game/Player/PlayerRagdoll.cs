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
        SetMappingWeight(0);
    }

    public void SetForGame()
    {
        myRagdoll.mappingWeight = 1f;
        SetPinWeight(0);
    }

    public void SetPinWeight(float _weight)
    {
        myRagdoll.pinWeight = _weight;
    }

    public void SetMappingWeight(float _weight)
    {
        myRagdoll.mappingWeight = _weight;
    }

    public void SetMode(PuppetMaster.Mode _mode)
    {
        myRagdoll.mode = _mode;
    }

    void EnableRagdoll(bool _mode)
    {
        PuppetMaster.State state = _mode ? PuppetMaster.State.Alive : PuppetMaster.State.Dead;
        myRagdoll.state = state;
    }
}
