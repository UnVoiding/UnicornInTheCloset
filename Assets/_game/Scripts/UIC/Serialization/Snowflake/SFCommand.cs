// using System.Collections.Generic;
//
// namespace RomenoCompany
// {
//     public class SFCommand
//     {
//         public enum CommandType
//         {
//             SHOW_MESSAGE = 0,
//             SHOW_IMAGE = 10,
//             WAIT_ANSWERS = 20,
//         }
//
//         public CommandType type;
//         public List<SFCondition> conditions;
//         public List<SFStatement> effects;
//
//         public float waitTimeBeforeExec = 1.0f;
//         public float waitTimeAfterExec = 1.0f;
//
//         public SFCommand()
//         {
//             conditions = new List<SFCondition>();
//             effects = new List<SFStatement>();
//         }
//     }
// }