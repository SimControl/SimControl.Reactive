// Copyright (c) SimControl e.U. - Wilhelm Medetz. All rights reserved. MIT License - see LICENSE.md

// TODO CR

namespace SimControl.Reactive;

/// <summary>Generic event arguments.</summary>
/// <typeparam name="T">Generic type parameter.</typeparam>
public class GenericEventArgs<T> : EventArgs
{
    /// <summary>Constructor.</summary>
    /// <param name="value">The value.</param>
    public GenericEventArgs(T value) => Value = value;

    /// <summary>T casting operator.</summary>
    /// <param name="args">The arguments.</param>
    public static implicit operator T(GenericEventArgs<T> args) =>
        // Contract.Requires(args != null);

        args.Value;

    /// <summary>Gets the value.</summary>
    /// <value>The value.</value>
    public T Value { get; }
}
