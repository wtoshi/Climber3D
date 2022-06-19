using System.Collections.Generic;
using UnityEngine;

public class Consts : MonoBehaviour
{
    public struct Tags
    {
        public const string Player = "Player";
        public const string PlayerRagdoll = "Puppet";
        public const string Ground = "Ground";
        public const string LevelEnd = "LevelEnd";
    }

    public struct Layers
    {
        public const string Player = "Player";
        public const string PlayerRagdoll = "Puppet";
        public const string Touchable = "Touchable";
    }

    public struct Animations
    {
        public const string Idle = "Idle";
        public const string Jump = "Jump";
        public const string Hanging = "Hanging";
    }

    public struct Particles
    {
        public const string RockHit = "FX_RockHit";
        public const string FX_GroundHit = "FX_GroundHit";
    }

    public struct FileNames
    {
        public const string LEVELDATA = "level.dat";
    }

    public struct AnalyticsEventNames
    {
        public const string LEVEL_START = "Start";
        public const string LEVEL_SUCCESS = "Success";
        public const string LEVEL_FAIL = "Fail";
    }

    public struct PrefKeys
    {
        public const string HAPTIC = "Haptic";
    }



}

