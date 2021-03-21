using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace RomenoCompany
{
    public class SkinBonesGetter : MonoBehaviour
    {
        public List<string> bones;
        [Button]
        public void GetBones()
        {
            bones = (from x in GetComponent<SkinnedMeshRenderer>().bones select x.name).ToList();
        }
    }
}
