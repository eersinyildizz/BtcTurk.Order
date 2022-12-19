using BtcTurk.Order.Api.Utils;

namespace BtcTurk.Order.Api.CurrentUserProvider;

public class HttpHeaderCurrentUserProvider : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpHeaderCurrentUserProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string Id => _httpContextAccessor.HttpContext != null
                        && _httpContextAccessor.HttpContext.Request.Headers.TryGetValue(Constants.Header.UserId, out var userid)
        ? userid.ToString()
        : Guid.NewGuid().ToString();
}