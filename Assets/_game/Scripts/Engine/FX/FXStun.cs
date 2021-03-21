using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RomenoCompany
{
    public class FXStun : FXItem
    {
        private void Update()
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
