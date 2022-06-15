using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using DG.Tweening;


public class PlayerIK : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] TwoBoneIKConstraint rightHandIK;
    [SerializeField] TwoBoneIKConstraint leftHandIK;

    public enum Hands
    {
        Right, Left
    }

    public void SetHandTarget(Hands _hand, Transform _target)
    {
        TwoBoneIKConstraint hand = _hand == Hands.Right ? rightHandIK : leftHandIK;

        if (_target == null)
        {
            DOTween.To(() => hand.weight, x => hand.weight = x, 0, 0.2f);

        }
        else
        {
            hand.transform.GetChild(0).position = _target.position;
            DOTween.To(() => hand.weight, x => hand.weight = x, 1, 0.2f);
        }
       
    }

}
