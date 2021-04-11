namespace RomenoCompany
{
    public class SFCondition
    {
        public enum BoolOperation
        {
            EQUALS = 0,
            NOT_EQUALS = 1,
            GREATER = 2,
            LESS = 3,
            GREATER_EQUALS = 4,
            LESS_EQUALS = 5,
            NONE = 1000
        }
        
        public string variableName;
        public BoolOperation operation;
        public int value;
    }
}