﻿using System;
using FluentEvents.Azure.ServiceBus.Receiving;
using FluentEvents.Infrastructure;
using NUnit.Framework;

namespace FluentEvents.Azure.ServiceBus.UnitTests.Receiving
{
    [TestFixture]
    public class AzureTopicEventReceiverConfigTests
    {
        private AzureTopicEventReceiverConfig m_AzureTopicEventReceiverConfig;

        [SetUp]
        public void SetUp()
        {
            m_AzureTopicEventReceiverConfig = new AzureTopicEventReceiverConfig
            {
                ReceiveConnectionString = Constants.ValidConnectionString,
                ManagementConnectionString = Constants.ValidConnectionString,
                SubscriptionNameGenerator = () => "",
                TopicPath = "TopicPath"
            };
        }

        [Test]
        public void SubscriptionNameGenerator_WhenNotSet_ShouldReturnGuidByDefault()
        {
            m_AzureTopicEventReceiverConfig = new AzureTopicEventReceiverConfig();

            var subscriptionName = m_AzureTopicEventReceiverConfig.SubscriptionNameGenerator();

            Assert.That(subscriptionName, Is.Not.Null);
            Assert.That(Guid.TryParse(subscriptionName, out _), Is.True);
        }

        [Test]
        public void SubscriptionNameGenerator_WhenSetToNull_ShouldThrow()
        {
            Assert.That(() =>
            {
                m_AzureTopicEventReceiverConfig = new AzureTopicEventReceiverConfig
                {
                    SubscriptionNameGenerator = null
                };
            }, Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ManagementConnectionString_WhenConnectionStringIsInvalid_ShouldThrow()
        {
            Assert.That(() =>
            {
                m_AzureTopicEventReceiverConfig.ManagementConnectionString = Constants.InvalidConnectionString;
            }, Throws.TypeOf<InvalidConnectionStringException>());
        }

        [Test]
        public void ManagementConnectionString_WhenConnectionStringIsValid_ShouldSet()
        {
            m_AzureTopicEventReceiverConfig.ManagementConnectionString = Constants.ValidConnectionString;

            Assert.That(
                m_AzureTopicEventReceiverConfig,
                Has
                    .Property(nameof(AzureTopicEventReceiverConfig.ManagementConnectionString))
                    .EqualTo(Constants.ValidConnectionString)
            );
        }

        [Test]
        public void ReceiveConnectionString_WhenConnectionStringIsInvalid_ShouldThrow()
        {
            Assert.That(() =>
            {
                m_AzureTopicEventReceiverConfig.ReceiveConnectionString = Constants.InvalidConnectionString;
            }, Throws.TypeOf<InvalidConnectionStringException>());
        }

        [Test]
        public void ReceiveConnectionString_WhenConnectionStringIsValid_ShouldSet()
        {
            m_AzureTopicEventReceiverConfig.ReceiveConnectionString = Constants.ValidConnectionString;

            Assert.That(
                m_AzureTopicEventReceiverConfig,
                Has
                    .Property(nameof(AzureTopicEventReceiverConfig.ReceiveConnectionString))
                    .EqualTo(Constants.ValidConnectionString)
            );
        }

        [Test]
        public void Validate_WhenReceiveConnectionStringIsNull_ShouldThrow()
        {
            m_AzureTopicEventReceiverConfig = new AzureTopicEventReceiverConfig
            {
                ManagementConnectionString = Constants.ValidConnectionString,
                SubscriptionNameGenerator = m_AzureTopicEventReceiverConfig.SubscriptionNameGenerator,
                TopicPath = m_AzureTopicEventReceiverConfig.TopicPath
            };

            Assert.That(() =>
            {
                ((IValidableConfig) m_AzureTopicEventReceiverConfig).Validate();
            }, Throws.TypeOf<ReceiveConnectionStringIsNullException>());
        }

        [Test]
        public void Validate_WhenManagementConnectionStringIsNull_ShouldThrow()
        {
            m_AzureTopicEventReceiverConfig = new AzureTopicEventReceiverConfig
            {
                ReceiveConnectionString = Constants.ValidConnectionString,
                SubscriptionNameGenerator = m_AzureTopicEventReceiverConfig.SubscriptionNameGenerator,
                TopicPath = m_AzureTopicEventReceiverConfig.TopicPath
            };

            Assert.That(() =>
            {
                ((IValidableConfig)m_AzureTopicEventReceiverConfig).Validate();
            }, Throws.TypeOf<ManagementConnectionStringIsNullException>());
        }

        [Test]
        public void Validate_WhenTopicPathIsNull_ShouldThrow()
        {
            m_AzureTopicEventReceiverConfig = new AzureTopicEventReceiverConfig
            {
                ManagementConnectionString = Constants.ValidConnectionString,
                ReceiveConnectionString = Constants.ValidConnectionString,
                SubscriptionNameGenerator = m_AzureTopicEventReceiverConfig.SubscriptionNameGenerator
            };

            Assert.That(() =>
            {
                ((IValidableConfig)m_AzureTopicEventReceiverConfig).Validate();
            }, Throws.TypeOf<TopicPathIsNullException>());
        }
    }
}
