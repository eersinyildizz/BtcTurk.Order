using System;
using BtcTurk.Order.Api.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BtcTurk.Order.Api.Tests.Handlers;

public class HandlerExtensionsTests
{
    [Fact]
    public void AddHandlers_IfServiceCollectionIsNull_ThrowsException()
    {
        // Arrange
        ServiceCollection serviceCollection = null;
        
        // Act && Assert
        Assert.Throws<ArgumentNullException>(() => serviceCollection.AddHandlers());
    }
    [Fact]
    public void  AddHandler_IfServiceCollectionValid_AllServiceShouldBeRegistered()
    {
        // Arrange
        ServiceCollection serviceCollection = new ServiceCollection();
       
        // Act
        serviceCollection.AddHandlers();
        
        // Assert
        Assert.True(serviceCollection.IsRegistered<IEmailNotificationHandler>());
        Assert.True(serviceCollection.IsImplementedClassRegistered<EmailNotificationHandler>());
        
        Assert.True(serviceCollection.IsRegistered<ISmsNotificationHandler>());
        Assert.True(serviceCollection.IsImplementedClassRegistered<SmsNotificationHandler>());
        
        Assert.True(serviceCollection.IsRegistered<IPushNotificationHandler>());
        Assert.True(serviceCollection.IsImplementedClassRegistered<PushNotificationHandler>());
    }
    
}