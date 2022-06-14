using System.Collections.Generic;
using UnityEngine;

public class Consts : MonoBehaviour
{
    public struct Tags
    {
        public const string Player = "Player";
    }

    public struct Layers
    {
        public const string Player = "Player";
        public const string Touchable = "Touchable";
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

