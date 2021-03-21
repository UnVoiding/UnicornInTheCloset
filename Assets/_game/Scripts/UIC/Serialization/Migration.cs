using UnityEngine;

namespace RomenoCompany
{
    public class Migration
    {
        public static int saveVersion = 3;
        
        public static void Step(int fromVersion)
        {
            if (fromVersion == 0) From_0_to_1();
            if (fromVersion == 1) From_1_to_2();
            if (fromVersion == 2) From_2_to_3();
            if (fromVersion == 3) From_3_to_4();
            // etc
        }
        
        private static void From_0_to_1()
        {
            Debug.Log($"Migration: upgrading 0->1");
        }
        
        private static void From_1_to_2()
        {
            Debug.Log($"Migration: upgrading 1->2");
        }
        
        private static void From_2_to_3()
        {
            Debug.Log($"Migration: upgrading 2->3");
        }

        private static void From_3_to_4()
        {
            Debug.Log($"Migration: upgrading 3->4");
        }

    }

}
