namespace BtcTurk.Order.Api.Exceptions;

/// <summary>
/// Business exception class
/// </summary>
public class BusinessException : Exception, IUnrecoverableException
{
    public string Code { get; set; }
    
    /// <summary>
    /// Default business exception const
    /// </summary>
    /// <param name="message"></param>
    /// <param name="code"></param>
    public BusinessException(string message, string code = "") : base(message)
    {
        this.Code = code;
    }

    /// <summary>
    /// Business exception Throw extension
    /// </summary>
    /// <param name="message"></param>
    /// <param name="code"></param>
    /// <exception cref="BusinessException"></exception>
    public static void Throw(string message, string code = "")
    {
        throw new BusinessException(message, code);
    }
}