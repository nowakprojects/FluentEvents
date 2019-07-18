﻿using System;
using System.Collections.Generic;

namespace FluentEvents.Subscriptions
{
    /// <summary>
    ///     An exception that aggregates all exceptions thrown by the handlers of an event.
    /// </summary>
    public class SubscriptionPublishAggregateException : AggregateException
    {
        /// <summary>
        ///     Creates a new <see cref="SubscriptionPublishAggregateException"/>
        /// </summary>
        /// <param name="exceptions">The exceptions to aggregate.</param>
        public SubscriptionPublishAggregateException(IEnumerable<Exception> exceptions) 
            : base(exceptions)
        {
        }
    }
}