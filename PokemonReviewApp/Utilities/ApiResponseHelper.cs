namespace API_Dinamis.Utilities
{
    public class ApiResponseHelper
    {
        public static object StdApiResponse(string message, object data, int datacount)
        {
            var response = new
            {
                datacount,
                message,
                data
            };

            return response;
        }

        public static object ValidationResponse(bool result, string message)
        {
            var response = new
            {
                result,
                message
            };

            return response;
        }
    }
}
