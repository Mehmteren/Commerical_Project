﻿using MassTransit;
using Shared.Messages;
using System;
using System.Collections.Generic;

namespace Shared.Events.BasketEvents
{
    public class ProductAddedToBasketRequestEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public int UserId { get; set; }

        public int BasketId { get; set; }

        public List<BasketItemMessage> BasketItemMessages { get; set; }
    }
}
