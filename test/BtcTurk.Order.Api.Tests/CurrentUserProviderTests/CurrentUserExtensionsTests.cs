using System;
using BtcTurk.Order.Api.CurrentUserProvider;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BtcTurk.Order.Api.Tests.CurrentUserProviderTests;

public class CurrentUserExtensionsTests
{
    [Fact]
    public void AddCurrentUser_IfServiceCollectionIsNull_ThrowsException()
    {
        // Arrange
        ServiceCollection serviceCollection = null;
        
        // Act && Assert
        Assert.Throws<ArgumentNullException>(() => serviceCollection.AddCurrentUser());
    }
    
    [Fact]
    public void AddCurrentUser_IfServiceCollectionValid_AllServiceShouldBeRegistered()
    {
        // Arrange
        ServiceCollection serviceCollection = new ServiceCollection();
       
        // Act
        serviceCollection.AddCurrentUser();
        
        // Assert
        Assert.True(serviceCollection.IsRegistered<ICurrentUser>());
        Assert.True(serviceCollection.IsImplementedClassRegistered<HttpHeaderCurrentUserProvider>());
    }
}