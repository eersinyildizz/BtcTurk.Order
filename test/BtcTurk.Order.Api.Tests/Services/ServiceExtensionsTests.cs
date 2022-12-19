using System;
using BtcTurk.Order.Api.Services;
using BtcTurk.Order.Api.Services.Notifications.Email;
using BtcTurk.Order.Api.Services.Notifications.Push;
using BtcTurk.Order.Api.Services.Notifications.Sms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BtcTurk.Order.Api.Tests.Services;

public class ServiceExtensionsTests
{
    [Fact]
    public void AddServices_IfServiceCollectionIsNull_ThrowsException()
    {
        // Arrange
        ServiceCollection serviceCollection = null;
        
        // Act && Assert
        Assert.Throws<ArgumentNullException>(() => serviceCollection.AddServices());
    }
    
    [Fact]
    public void AddServices_IfServiceCollectionValid_AllServiceShouldBeRegistered()
    {
        // Arrange
        ServiceCollection serviceCollection = new ServiceCollection();
       
        // Act
        serviceCollection.AddServices();
        
        // Assert
        Assert.True(serviceCollection.IsRegistered<IEmailService>());
        Assert.True(serviceCollection.IsRegistered<ISmsService>());
        Assert.True(serviceCollection.IsRegistered<IPushService>());
    }
}