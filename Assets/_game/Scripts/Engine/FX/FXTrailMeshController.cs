using UnityEngine;

namespace RomenoCompany
{
    public class FXTrailMeshController : MonoBehaviour
    {
        [SerializeField] private float speedMult = 1f;
        [SerializeField] private Transform[] bones;
        void Update()
        {
            for(int i = 1; i < bones.Length; i++)
            {
                bones[i].rotation = Quaternion.Lerp(bones[i].rotation, bones[i - 1].rotation, (float) (bones.Length - i + 1) / bones.Length * speedMult * TimeManager.Instance.TimeScale);
                bones[i].position = Vector3.Lerp(bones[i].position, bones[i - 1].position, (float) (bones.Length - i + 1) / bones.Length * speedMult * TimeManager.Instance.TimeScale);
            }
        }
    }    
}

