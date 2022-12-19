using System;
using BtcTurk.Order.Api.CurrentUserProvider;
using BtcTurk.Order.Api.Utils;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace BtcTurk.Order.Api.Tests.CurrentUserProviderTests;

public class HttpHeaderCurrentUserProviderTests : TestsFor<HttpHeaderCurrentUserProvider>
{
    [Fact]
    public void Id_IfHttpContextHeaderHasUserId_Returns()
    {
        // Arrange
        var mockHttpContextAccessor = GetMockFor<IHttpContextAccessor>();
        var context = new DefaultHttpContext();
        var fakeUserId = "123";
        context.Request.Headers[Constants.Header.UserId] = fakeUserId;
        mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);
        
        // Act
        var result = Instance.Id;
        
        // Assert
        Assert.Equal(result, fakeUserId);
    }
    
    [Fact]
    public void Id_IfHttpContextHeaderHasNotUserId_ReturnsRandomGuid()
    {
        // Arrange
        var mockHttpContextAccessor = GetMockFor<IHttpContextAccessor>();
        var context = new DefaultHttpContext();
        mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);
        
        // Act
        var result = Instance.Id;
        
        // Assert
        Assert.True(Guid.TryParse(result, out var randomGuid));
    }
}