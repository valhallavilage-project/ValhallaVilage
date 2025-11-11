using System;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public static class Extensions
{
    public static void RemoveAllChildren(this Transform transform)
    {
        int childCount = transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
            Object.Destroy(transform.GetChild(i).gameObject);
    }

    public static void SetUniqueCallback(this Button button, Action callback)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => callback?.Invoke());
    }

    public static Vector3 WithX(this Vector3 vector3, float xValue)
    {
        var result = vector3;
        result.x = xValue;

        return result;
    }

    public static Vector3 WithY(this Vector3 vector3, float yValue)
    {
        var result = vector3;
        result.y = yValue;

        return result;
    }

    public static Vector3 WithZ(this Vector3 vector3, float zValue)
    {
        var result = vector3;
        result.z = zValue;

        return result;
    }

    public static string ToTimeFormat(this int totalSeconds)
    {
        return FormatTime(totalSeconds);
    }

    public static string ToTimeFormat(this float totalSeconds)
    {
        return FormatTime((int)totalSeconds);
    }

    private static string FormatTime(int totalSeconds)
    {
        var time = TimeSpan.FromSeconds(totalSeconds);
        var result = "";

        if (totalSeconds >= 86400)
            result += $"{time.Days:00} D ";

        if (totalSeconds >= 3600)
            result += $"{time.Hours:00} H ";

        if (totalSeconds >= 60)
            result += $"{time.Minutes:00} M ";

        result += $"{time.Seconds:00} S";

        return result.Trim();
    }
}
