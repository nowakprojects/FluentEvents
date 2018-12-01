﻿using System;
using System.Threading.Tasks;
using FluentEvents.Pipelines;
using FluentEvents.Pipelines.Filters;
using Moq;
using NUnit.Framework;

namespace FluentEvents.UnitTests.Pipelines.Filters
{
    [TestFixture]
    public class FilterPipelineModuleTests : PipelineModuleTestBase
    {
        private FilterPipelineModule m_FilterPipelineModule;
        private FilterPipelineModuleConfig m_FilterPipelineModuleConfig;

        [SetUp]
        public void SetUp()
        {
            m_FilterPipelineModule = new FilterPipelineModule();
            m_FilterPipelineModuleConfig = new FilterPipelineModuleConfig((sender, args) => ((TestSender)sender).IsValid);
        }

        [Test]
        public async Task InvokeAsync_WithoutMatch_ShouldNotInvokeNextModule()
        {
            var testSender = new TestSender { IsValid = false };
            var testEventArgs = new TestEventArgs();

            var pipelineModuleContext = SetUpPipelineModuleContext(
                testSender,
                testEventArgs,
                m_FilterPipelineModuleConfig
            );

            var isInvoked = false;

            Task InvokeNextModule(PipelineContext context)
            {
                isInvoked = true;
                return Task.CompletedTask;
            }

            await m_FilterPipelineModule.InvokeAsync(pipelineModuleContext, InvokeNextModule);

            Assert.That(isInvoked, Is.False);
        }

        [Test]
        public async Task InvokeAsync_WithMatch_ShouldInvokeNextModule()
        {
            var testSender = new TestSender { IsValid = true };
            var testEventArgs = new TestEventArgs();

            var pipelineModuleContext = SetUpPipelineModuleContext(
                testSender,
                testEventArgs, 
                m_FilterPipelineModuleConfig
            );

            PipelineContext nextModuleContext = null;

            Task InvokeNextModule(PipelineContext context)
            {
                nextModuleContext = context;
                return Task.CompletedTask;
            }

            await m_FilterPipelineModule.InvokeAsync(pipelineModuleContext, InvokeNextModule);

            Assert.That(nextModuleContext, Is.Not.Null);
            Assert.That(nextModuleContext, Is.EqualTo(pipelineModuleContext));
        }
        
        private class TestSender
        {
            public bool IsValid { get; set; }
        }

        private class TestEventArgs
        {

        }
    }
}
