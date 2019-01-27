﻿using System;
using System.Threading.Tasks;
using FluentEvents.Pipelines;

namespace FluentEvents.Queues
{
    public class EventsQueuesService : IEventsQueuesService
    {
        private readonly EventsQueuesContext m_EventsQueuesContext;
        private readonly IEventsQueueNamesService m_EventsQueueNamesService;

        public EventsQueuesService(EventsQueuesContext eventsQueuesContext, IEventsQueueNamesService eventsQueueNamesService)
        {
            m_EventsQueuesContext = eventsQueuesContext;
            m_EventsQueueNamesService = eventsQueueNamesService;
        }

        public async Task ProcessQueuedEventsAsync(EventsScope eventsScope, string queueName)
        {
            if (eventsScope == null) throw new ArgumentNullException(nameof(eventsScope));

            if (queueName != null)
            {
                if (!m_EventsQueueNamesService.IsQueueNameExisting(queueName))
                    throw new EventsQueueNotFoundException();

                var eventsQueue = eventsScope.EventQueues.GetOrAddEventsQueue(m_EventsQueuesContext, queueName);
                await ProcessQueue(eventsScope, eventsQueue);
            }
            else
            {
                foreach (var eventsQueue in eventsScope.EventQueues)
                    await ProcessQueue(eventsScope, eventsQueue);
            }
        }

        private async Task ProcessQueue(EventsScope eventsScope, IEventsQueue eventsQueue)
        {
            if (eventsScope == null) throw new ArgumentNullException(nameof(eventsScope));
            if (eventsQueue == null) throw new ArgumentNullException(nameof(eventsQueue));

            foreach (var queuedPipelineEvent in eventsQueue.DequeueAll())
                await queuedPipelineEvent.Pipeline.ProcessEventAsync(queuedPipelineEvent.PipelineEvent, eventsScope);
        }

        public void DiscardQueuedEvents(EventsScope eventsScope, string queueName)
        {
            if (eventsScope == null) throw new ArgumentNullException(nameof(eventsScope));

            if (queueName != null)
            {
                if (!m_EventsQueueNamesService.IsQueueNameExisting(queueName))
                    throw new EventsQueueNotFoundException();

                var eventsQueue = eventsScope.EventQueues.GetOrAddEventsQueue(m_EventsQueuesContext, queueName);
                eventsQueue.DiscardQueuedEvents();
            }
            else
            {
                foreach (var eventsQueue in eventsScope.EventQueues)
                    eventsQueue.DiscardQueuedEvents();
            }
        }

        public void EnqueueEvent(EventsScope eventsScope, PipelineEvent pipelineEvent, IPipeline pipeline)
        {
            if (eventsScope == null) throw new ArgumentNullException(nameof(eventsScope));
            if (pipelineEvent == null) throw new ArgumentNullException(nameof(pipelineEvent));
            if (pipeline == null) throw new ArgumentNullException(nameof(pipeline));

            if (!m_EventsQueueNamesService.IsQueueNameExisting(pipeline.QueueName))
                throw new EventsQueueNotFoundException();

            var queue = eventsScope.EventQueues.GetOrAddEventsQueue(m_EventsQueuesContext, pipeline.QueueName);

            queue.Enqueue(new QueuedPipelineEvent
            {
                Pipeline = pipeline,
                PipelineEvent = pipelineEvent
            });
        }
    }
}