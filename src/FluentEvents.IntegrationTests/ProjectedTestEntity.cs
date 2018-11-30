﻿using System;
using AsyncEvent;

namespace FluentEvents.IntegrationTests
{
    public class ProjectedTestEntity
    {
        public string Id { get; set; }
        public event EventHandler<ProjectedEventArgs> Test;
        public event AsyncEventHandler<ProjectedEventArgs> AsyncTest;
    }
}
