using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Rigidbody myRB;
    [SerializeField] Rigidbody rightHandRB;
    [SerializeField] Rigidbody leftHandRB;
    [SerializeField] ConfigurableJoint rightHandAnchor;
    [SerializeField] ConfigurableJoint leftHandAnchor;
    [SerializeField] Rigidbody pelvisRB;

    [FoldoutGroup("Movement Settings")]
    [SerializeField] float baseClimbSpeed;

    public bool CanClimb { get => _canClimb; set { _canClimb = value; } }
    public Transform pelvis => pelvisRB.transform;

    bool _canClimb;
    ClimbPoint _currentClimbPoint;
    float _currentClimbSpeed;
    bool _isClimbing;

    private void Start()
    {
        _currentClimbSpeed = baseClimbSpeed;
    }

    private void OnEnable()
    {
        EventManager.OnTouchEvent.AddListener(ClimbTo);
        EventManager.LevelStartEvent.AddListener(OnLevelStarted);
    }
    private void OnDisable()
    {
        EventManager.OnTouchEvent.RemoveListener(ClimbTo);
        EventManager.LevelStartEvent.RemoveListener(OnLevelStarted);
    }

    void OnLevelStarted()
    {
        //myRB.useGravity = true;
        FirstJump();
    }

    void FirstJump()
    {
        _isClimbing = true;
        myRB.isKinematic = false;

        // Jump Animation
        playerController.PlayerAnimations.SetTrigger(Consts.Animations.Jump);

        // Get First Climb Point
        ClimbPoint firstPoint = LevelController.Instance.LevelFacade.FirstJumpPoint;
        _currentClimbPoint = firstPoint;

        
        this.Run(.8f, ()=> {

            //myRB.velocity = Vector3.up * _currentClimbSpeed;
            myRB.AddForce(Vector3.up * 5f, ForceMode.Impulse);

            this.Run(.2f, ()=> {

                playerController.PlayerRagdoll.SetForGame();
                MoveHand(firstPoint, true);
                MoveHand(firstPoint, false);

                myRB.velocity = Vector3.zero;
                myRB.angularVelocity = Vector3.zero;
                myRB.isKinematic = true;

                _isClimbing = false;
                _canClimb = true;
                playerController.ResetParentPosition();
            });
        });
        
    }

    void ClimbTo(TouchableData _data)
    {
        if (_isClimbing || _currentClimbPoint == _data.climbPoint || !_canClimb)
            return;

        ClimbPoint toPoint = _data.climbPoint;

        bool toRight = toPoint.transform.position.x >= transform.position.x;

        // Check distance
        Vector3 distance = Vector3.zero;
        if (toRight)
            distance = toPoint.RightHandTarget.position - rightHandRB.transform.position;
        else
            distance = toPoint.LeftHandTarget.position - leftHandRB.transform.position;

        distance.z = 0f;

        float dist = Mathf.Abs(distance.magnitude);
        Debug.Log("distance: "+ dist);
        if (dist >= 3f)
            return;

        _isClimbing = true;

        // Move hands
        MoveHand(toPoint, toRight, ()=> {
            MoveHand(toPoint,!toRight, ()=> {
                playerController.ResetParentPosition();
                _isClimbing = false;
                _currentClimbPoint = toPoint;
            });
        });

    }

    void MoveHand(ClimbPoint _point, bool _rightHand, System.Action _onComplete = null)
    {
        ConfigurableJoint handAnchor = _rightHand ? rightHandAnchor : leftHandAnchor;
        Rigidbody handRB = _rightHand ? rightHandRB : leftHandRB;
        Rigidbody handAnchorRB = handAnchor.GetComponent<Rigidbody>();
        Transform toMove = _rightHand ? _point.RightHandTarget : _point.LeftHandTarget;

        if (handAnchor.connectedBody == null)
        {
            handAnchor.transform.position = handRB.transform.position;
            handAnchor.connectedBody = handRB;
        }

        handAnchorRB.DOMove(toMove.position, baseClimbSpeed).SetEase(Ease.Linear).SetSpeedBased(true).OnComplete(()=> {
            _onComplete?.Invoke();
        });
    }





}
