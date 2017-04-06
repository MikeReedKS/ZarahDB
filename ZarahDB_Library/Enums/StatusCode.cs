// ReSharper disable InconsistentNaming

namespace ZarahDB_Library.Enums
{
    /// <summary>
    ///     Enum StatusCode
    ///     See http://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html
    /// </summary>
    public enum StatusCode
    {
        //Informational 1xx
        /// <summary>
        ///     The continue
        /// </summary>
        Continue = 100,

        /// <summary>
        ///     The switching protocols
        /// </summary>
        Switching_Protocols = 101,
        //Successful 2xx
        /// <summary>
        ///     The ok
        /// </summary>
        OK = 200,

        /// <summary>
        ///     The created
        /// </summary>
        Created = 201,

        /// <summary>
        ///     The accepted
        /// </summary>
        Accepted = 202,

        /// <summary>
        ///     The non authoritative information
        /// </summary>
        Non_Authoritative_Information = 203,

        /// <summary>
        ///     The no content
        /// </summary>
        No_Content = 204,

        /// <summary>
        ///     The reset content
        /// </summary>
        Reset_Content = 205,

        /// <summary>
        ///     The partial content
        /// </summary>
        Partial_Content = 206,
        //Redirection 3xx
        /// <summary>
        ///     The multiple choices
        /// </summary>
        Multiple_Choices = 300,

        /// <summary>
        ///     The moved permanently
        /// </summary>
        Moved_Permanently = 301,

        /// <summary>
        ///     The found
        /// </summary>
        Found = 302,

        /// <summary>
        ///     The see other
        /// </summary>
        See_Other = 303,

        /// <summary>
        ///     The not modified
        /// </summary>
        Not_Modified = 304,

        /// <summary>
        ///     The use proxy
        /// </summary>
        Use_Proxy = 305,

        /// <summary>
        ///     The temporary redirect
        /// </summary>
        Temporary_Redirect = 307,
        //Client Error 4xx
        /// <summary>
        ///     The bad request
        /// </summary>
        Bad_Request = 400,

        /// <summary>
        ///     The unauthorized
        /// </summary>
        Unauthorized = 401,

        /// <summary>
        ///     The payment required
        /// </summary>
        Payment_Required = 402,

        /// <summary>
        ///     The forbidden
        /// </summary>
        Forbidden = 403,

        /// <summary>
        ///     The not found
        /// </summary>
        Not_Found = 404,

        /// <summary>
        ///     The method not allowed
        /// </summary>
        Method_Not_Allowed = 405,

        /// <summary>
        ///     The not acceptable
        /// </summary>
        Not_Acceptable = 406,

        /// <summary>
        ///     The proxy authentication required
        /// </summary>
        Proxy_Authentication_Required = 407,

        /// <summary>
        ///     The request timeout
        /// </summary>
        Request_Timeout = 408,

        /// <summary>
        ///     The conflict
        /// </summary>
        Conflict = 409,

        /// <summary>
        ///     The gone
        /// </summary>
        Gone = 410,

        /// <summary>
        ///     The length required
        /// </summary>
        Length_Required = 411,

        /// <summary>
        ///     The precondition failed
        /// </summary>
        Precondition_Failed = 412,

        /// <summary>
        ///     The request entity too large
        /// </summary>
        Request_Entity_Too_Large = 413,

        /// <summary>
        ///     The request URI too long
        /// </summary>
        Request_URI_Too_Long = 414,

        /// <summary>
        ///     The unsupported media type
        /// </summary>
        Unsupported_Media_Type = 415,

        /// <summary>
        ///     The requested range not satisfiable
        /// </summary>
        Requested_Range_Not_Satisfiable = 416,

        /// <summary>
        ///     The expectation failed
        /// </summary>
        Expectation_Failed = 417,
        //Server Error 5xx
        /// <summary>
        ///     The internal server error
        /// </summary>
        Internal_Server_Error = 500,

        /// <summary>
        ///     The not implemented
        /// </summary>
        Not_Implemented = 501,

        /// <summary>
        ///     The bad gateway
        /// </summary>
        Bad_Gateway = 502,

        /// <summary>
        ///     The service unavailable
        /// </summary>
        Service_Unavailable = 503,

        /// <summary>
        ///     The gateway timeout
        /// </summary>
        Gateway_Timeout = 504,

        /// <summary>
        ///     The HTTP version not supported
        /// </summary>
        HTTP_Version_Not_Supported = 505,

        /// <summary>
        ///     The exception
        /// </summary>
        Exception = 520,
        //Transaction 7xx
        /// <summary>
        ///     The rolled back
        /// </summary>
        Rolled_Back = 770,
        //Locks 8xx
        /// <summary>
        ///     The outside lock
        /// </summary>
        Outside_Lock = 850
    }
}