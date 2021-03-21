using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace RomenoCompany
{
    public class ArrayUtil
    {
        public static List<T> Shuffle<T>(List<T> arr)
        {
            for (int i = 0; i < arr.Count; i++) {
                var temp = arr[i];
                int randomIndex = Random.Range(i, arr.Count);
                arr[i] = arr[randomIndex];
                arr[randomIndex] = temp;
            }
            return arr;
        }
    
        public static T[] Shuffle<T>(T[] arr)
        {
            for (int i = 0; i < arr.Length; i++) {
                var temp = arr[i];
                int randomIndex = Random.Range(i, arr.Length);
                arr[i] = arr[randomIndex];
                arr[randomIndex] = temp;
            }
            return arr;
        }
    }
}
