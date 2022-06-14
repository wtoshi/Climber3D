using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.ParticleSystem;
using Random = UnityEngine.Random;

public static class Extension
{

    #region Task Extensions
    public delegate void Task();

    /// <summary>
    /// Runs the given task one time after a delay.
    /// </summary>
    /// <param name="delay">Delay before running the task.</param>
    /// <param name="task">Task to run.</param>
    public static void Run(this MonoBehaviour behaviour, float delay, Task task)
    {
        behaviour.StartCoroutine(RunTask(delay, task));
    }

    private static IEnumerator RunTask(float delay, Task task)
    {
        yield return new WaitForSeconds(delay);
        task.Invoke();
    }
    #endregion

    #region Transform
    /// <summary>
    /// Destroys children of a transform with reverse for loop.
    /// </summary>
    /// <param name="keep">How many children to keep from start</param>
    public static void DestroyChildren(this Transform transform, int keep = 0)
    {
        for (int i = transform.childCount - 1; i >= keep; i--)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
    }
    #endregion

    #region List Extension
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        for (var i = 0; i < count - 1; ++i)
        {
            var r = Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    /// <summary>
    /// Copies a List<> excluding excludes.
    /// </summary>
    /// <param name="excludes">Exclude these objects from List<></param>
    /// <returns>Copied List<></returns>
    public static List<T> CopyExcluding<T>(this List<T> list, params T[] excludes)
    {
        List<T> copy = new List<T>(list);
        foreach (T exclude in excludes)
        {
            copy.Remove(exclude);
        }
        return copy;
    }
    #endregion

    #region Particle System
    /// <summary>
    /// Sets start color of a particle system.
    /// </summary>
    /// <param name="color">Color value.</param>
    public static void SetStartColor(this MainModule mainModule, Color color)
    {
        mainModule.startColor = color;
    }
    #endregion

    #region Random
    /// <summary>
    /// Generates a random index within start(inclusive) and end(inclusive), excluding given indexes.
    /// </summary>
    /// <param name="start">Start of indexes.</param>
    /// <param name="end">End of indexes (should be bigger than start).</param>
    /// <param name="exclude">Exclude indexes</param>
    /// <returns></returns>
    public static int RandomIndex(int start, int end, params int[] exclude)
    {
        List<int> indexes = new List<int>();
        for (int i = 0; i <= end - start; i++)
        {
            indexes.Add(i + start);
        }
        foreach (int ex in exclude)
        {
            if (ex >= start && ex <= end)
            {
                indexes.Remove(ex);
            }
        }
        return indexes[Random.Range(0, indexes.Count)];
    }

    /// <summary>
    /// Returns a random element from the list.
    /// </summary>
    /// <param name="removeElement">If true removes the selected random element form the list. True by default.<s/param>
    /// <typeparam name="T">List element type.</typeparam>
    /// <returns></returns>
    public static T RandomElement<T>(this List<T> list, bool removeElement = true)
    {
        if (list.Count <= 0) return default(T);
        T randomElement = list[Random.Range(0, list.Count)];
        if (removeElement)
        {
            list.Remove(randomElement);
        }
        return randomElement;
    }
    #endregion


    public static float GetScaledValue(float _fromLimitMin, float _fromLimitMax, float _toLimitMin, float _toLimitMax, float _oldValue)
    {
        float newValue;

        newValue = Mathf.Lerp(_toLimitMin, _toLimitMax, Mathf.InverseLerp(_fromLimitMin, _fromLimitMax, _oldValue));

        return newValue;
    }


}

