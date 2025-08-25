using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace CrossProject.Extensions
{
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
    }
}
