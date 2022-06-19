using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] PlayerRagdoll playerRagdoll;
    [SerializeField] PlayerAnimations playerAnimations;

    [SerializeField] List<SkinnedMeshRenderer> skinnedMeshes;

    [SerializeField] Transform stickmanTransform;

    public PlayerMovement PlayerMovement => playerMovement;
    public PlayerInput PlayerInput => playerInput;
    public PlayerRagdoll PlayerRagdoll => playerRagdoll;
    public PlayerAnimations PlayerAnimations => playerAnimations;

    bool _hitGround;
    bool _onLevelEnd;

    private void Update()
    {
  
    }

    public void GetHit()
    {
        playerMovement.CanClimb = false;
        playerMovement.ReleaseHands();
    }


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

    public void OnHitGround(Vector3 _hitPos)
    {
        if (_hitGround)
            return;

        _hitGround = true;

        for (int i = 0; i < skinnedMeshes.Count; i++)
        {
            skinnedMeshes[i].enabled = false;
        }

        Debug.Log("Hit To Ground!");

        GameObject hitFX =  FXController.Instance.AddParticle(Consts.Particles.FX_GroundHit, _hitPos,-1,LevelController.Instance.SpawnsHolder);
        CameraManager.Instance.SetFailCameraTarget(hitFX.transform);

        this.Run(.5f, ()=> {

            GameObject grave = Instantiate(Resources.Load<GameObject>("Gravestone"), LevelController.Instance.SpawnsHolder);
            grave.transform.position = _hitPos;

            grave.transform.DOScale(2f, .3f).SetEase(Ease.OutBack).From(0f);

            CameraManager.Instance.SetFailCameraTarget(grave.transform);

            Destroy(gameObject);
        });

    }

    public void OnLevelEnd()
    {
        if (_onLevelEnd)
            return;

        _onLevelEnd = true;
        playerMovement.CanClimb = false;

        playerMovement.ReleaseHands();
        //ResetParentPosition();
        //transform.GetChild(0).localPosition = Vector3.zero;
        
        stickmanTransform.transform.position = playerRagdoll.Pelvis.position;
        playerRagdoll.SetMappingWeight(.1f);
        playerRagdoll.SetMode(RootMotion.Dynamics.PuppetMaster.Mode.Kinematic);
        //stickmanTransform.transform.localPosition = Vector3.zero;
        //playerRagdoll.SetPinWeight(1f);

        CameraManager.Instance.SetsuccessCameraTarget(stickmanTransform);

        LevelEnd levelEnd = LevelController.Instance.LevelEnd;

        playerAnimations.SetTrigger("JumpLevelEnd");

        var toRotate = new Vector3(0, 180f, 0);
        stickmanTransform.DORotate(toRotate, .3f).SetEase(Ease.Linear);
        stickmanTransform.DOJump(levelEnd.LandingPoint.position, 3f, 0, .7f).SetEase(Ease.Linear);

    }
}
