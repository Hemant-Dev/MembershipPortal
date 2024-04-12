namespace MembershipPortal.API.ErrorHandling
{
    public class MyException : Exception
    {
        public static string DataWithIdNotPresent(long id, string tableName)
        {
            return ($"Data with id {id} is not present in {tableName} table");
        }

        public static string DataNotFound(string tableName)
        {
            return ($"{tableName} Table is empty");
        }
        public static string DataWithNameNotFound(string tableName)
        {
            return ($"Data with this name is not found");
        }

        public static string DataProcessingError(string errorMessage)
        {
            return ($"An error occurred while processing your request." );
        }

        public static string IdMismatch()
        {
            return ($"Id mismatch error. Id's are not same.");
        }

        public static string DataDeletedSuccessfully(string tableName)
        {
            return ($"{tableName} deleted successfully");
        }

        public static string DataUpdatedSuccessfully(string tableName)
        {
            return ($"{tableName} updated successfully");
        }
        
        public static string DataAddedSuccessfully(string tableName)
        {
            return ($"{tableName} Added successfully");
        }


    }
}
