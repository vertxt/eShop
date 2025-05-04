namespace eShop.Web.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; }

        public ApiException(string message, Exception? innerException = null, int statusCode = 500)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }

    public class NotFoundException : ApiException
    {
        public NotFoundException(string message)
            : base(message, statusCode: 404)
        {
        }
    }

    public class UnauthorizedException : ApiException
    {
        public UnauthorizedException(string message)
            : base(message, statusCode: 401)
        {
        }
    }

    public class ForbiddenException : ApiException
    {
        public ForbiddenException(string message)
            : base(message, statusCode: 403)
        {
        }
    }

    public class BadRequestException : ApiException
    {
        public BadRequestException(string message)
            : base(message, statusCode: 400)
        {
        }
    }
}