﻿using System;
using AdvancedDddCqrs.Messages;
using Newtonsoft.Json;

namespace AdvancedDddCqrs
{
    public static class TTLFilteringHandler
    {
        public static TTLFilteringHandler<T> Wrap<T>(IHandler<T> handler)
        {
            return new TTLFilteringHandler<T>(handler);
        }
    }

    public class TTLFilteringHandler<T> : IHandler<T>
    {
        private readonly IHandler<T> _handler;
        private readonly int _durationSeconds;

        public TTLFilteringHandler(IHandler<T> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            _handler = handler;
        }

        public bool Handle(T message)
        {
            var hasTTL = message as IHaveTTL;
            if (hasTTL == null || hasTTL.HasExpired() == false)
            {
                return _handler.Handle(message);
            }

            return true;
        }

        public override string ToString()
        {
            return string.Format("TTLFilteringHandler({0})", _handler);
        }
    }
}