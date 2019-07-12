﻿using System;
using FluentEvents.Config;
using FluentEvents.IntegrationTests.Common;
using FluentEvents.Pipelines.Publication;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace FluentEvents.IntegrationTests
{
    [TestFixture]
    public class PipelineWithInheritanceTest
    {
        private IServiceProvider _appServiceProvider;

        [SetUp]
        public void SetUp()
        {
            var services = new ServiceCollection();
            services.AddSingleton<SubscribingService>();
            services.AddEventsContext<TestEventsContext>(options => { });
            _appServiceProvider = services.BuildServiceProvider();
        }

        [Test]
        public void PipelinesWithInheritedEvents_ShouldPublishInheritingEvents()
        {
            var subscribingService = _appServiceProvider.GetRequiredService<SubscribingService>();
            var testEventsContext = _appServiceProvider.GetRequiredService<TestEventsContext>();
            var eventsScope = _appServiceProvider.GetRequiredService<EventsScope>();

            TestUtils.AttachAndRaiseEvent(testEventsContext, eventsScope);
            
            Assert.That(subscribingService, Has.Property(nameof(SubscribingService.BaseTestEvents)).Empty);
            Assert.That(subscribingService, Has.Property(nameof(SubscribingService.TestEvents)).With.One.Items);
            Assert.That(subscribingService, Has.Property(nameof(SubscribingService.TestEvents)).With.One.Items.TypeOf<TestEvent>());
        }

        private class TestEventsContext : EventsContext
        {
            protected override void OnBuildingSubscriptions(SubscriptionsBuilder subscriptionsBuilder)
            {
                subscriptionsBuilder
                    .ServiceHandler<SubscribingService, TestEvent>()
                    .HasGlobalSubscription();
            }

            protected override void OnBuildingPipelines(PipelinesBuilder pipelinesBuilder)
            {
                pipelinesBuilder
                    .Event<TestEventBase>()
                    .IsPiped()
                    .ThenIsPublishedToGlobalSubscriptions();
            }
        }
    }
}
