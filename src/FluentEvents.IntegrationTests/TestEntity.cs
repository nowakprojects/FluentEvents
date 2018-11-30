﻿using System;
using System.Threading.Tasks;
using AsyncEvent;

namespace FluentEvents.IntegrationTests
{
    public class TestEntity
    {
        public string Id { get; set; }
        public event EventHandler<TestEventArgs> Test;
        public event AsyncEventHandler<TestEventArgs> AsyncTest;

        public void RaiseEvent(string value)
        {
            Test?.Invoke(this, new TestEventArgs {Value = value});
        }

        public async Task RaiseAsyncEvent(string value)
        {
            var asyncTest = AsyncTest;
            if (asyncTest != null)
                await asyncTest.InvokeAsync(this, new TestEventArgs {Value = value});
        }
    }
}