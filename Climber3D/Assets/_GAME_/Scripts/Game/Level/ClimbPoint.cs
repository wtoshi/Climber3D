using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbPoint : MonoBehaviour
{
    [SerializeField] Transform rightHandTarget;
    [SerializeField] Transform leftHandTarget;

    public Transform RightHandTarget => rightHandTarget;
    public Transform LeftHandTarget => leftHandTarget;

}
