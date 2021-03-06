﻿using System;

namespace MinecraftServerSharp.Utility
{
    public interface IMemory : IReadOnlyMemory
    {
        new Span<byte> Span { get; }
    }

    public interface IMemory<T> : IReadOnlyMemory<T>, IMemory
    {
        new Span<T> Span { get; }
    }
}
