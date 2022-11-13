namespace eShopSolution.Utilities.Contants
{
    public class SystemContants
    {
        public const string MainConnectionString = "eShopSolutionDb";

        public const int STATUS_DEFAULT = 1;

        public enum RoleId
        {
            PUBLIC = 3,
            MANAGER = 2,
        };

        public enum FormId
        {
            PRODUCT = 1,
            CATEGORY = 2,
            DISTRIBUTOR = 3,
            NEWS = 4,
            CUSTOMER = 5,
            USER = 6,
            ROLE = 7,
            ORDER = 8,
        }

        public enum ActionId
        {
            CREATE = 1,
            UPDATE = 2,
            REMOVE = 3,
        }

        public enum SortOrderNumber
        {
            ZERO = 0,
            FIRST = 1,
            SECOND = 2,
            THIRD = 3,
            FOURTH = 4,
            FIFTH = 5,
            SIXTH = 6,
            SEVENTH = 7,
        }
    }
}
