namespace server.Helpers
{
    public static class ApiResponseHelper
    {
        public static object Sucess(object? data = null, string? message = null)
        {
            return new
            {
                IsSuccess = true,
                Data = data,
                Message = message ?? "Operation successful"
            };
        }

        public static object Fail(string message)
        {
            return new
            {
                IsSuccess = false,
                Message = message
            };
        }

    }
}