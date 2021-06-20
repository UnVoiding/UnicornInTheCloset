using System;
using UnityEngine;

namespace RomenoCompany
{
    [CreateAssetMenu(
        fileName = "FTUESettings", 
        menuName = "UIC/FTUESettings", 
        order = 59)]
    public class FTUESettings : ScriptableObject
    {
        public FTUETemplate[] ftueTemplates = null;

        public FTUETemplate GetFTUE(FTUEType type)
        {
            FTUETemplate template = Array.Find(ftueTemplates, fc => fc.fTUEType == type);
            if (template == null)
            {
                return Array.Find(ftueTemplates, fc => fc.fTUEType == FTUEType.DEFAULT);
            }

            return template;
        }
    }
}