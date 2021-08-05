// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information. 

// TODO CR

namespace SimControl.Reactive
{
    /// <summary>Effect delegate</summary>
    public delegate void Effect();

    /// <summary>Effect delegate</summary>
    /// <typeparam name="T1">Type of the in t 1.</typeparam>
    /// <param name="arg1">The first argument.</param>
    public delegate void Effect<in T1>(T1 arg1);

    /// <summary>Effect delegate</summary>
    /// <typeparam name="T1">Type of the in t 1.</typeparam>
    /// <typeparam name="T2">Type of the in t 2.</typeparam>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    public delegate void Effect<in T1, in T2>(T1 arg1, T2 arg2);

    /// <summary>Effect delegate</summary>
    /// <typeparam name="T1">Type of the in t 1.</typeparam>
    /// <typeparam name="T2">Type of the in t 2.</typeparam>
    /// <typeparam name="T3">Type of the in t 3.</typeparam>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="arg3">The third argument.</param>
    public delegate void Effect<in T1, in T2, in T3>(T1 arg1, T2 arg2, T3 arg3);

    /// <summary>Effect delegate</summary>
    /// <typeparam name="T1">Type of the in t 1.</typeparam>
    /// <typeparam name="T2">Type of the in t 2.</typeparam>
    /// <typeparam name="T3">Type of the in t 3.</typeparam>
    /// <typeparam name="T4">Type of the in t 4.</typeparam>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="arg3">The third argument.</param>
    /// <param name="arg4">The fourth argument.</param>
    public delegate void Effect<in T1, in T2, in T3, in T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

    /// <summary>Effect delegate</summary>
    /// <typeparam name="T1">Type of the in t 1.</typeparam>
    /// <typeparam name="T2">Type of the in t 2.</typeparam>
    /// <typeparam name="T3">Type of the in t 3.</typeparam>
    /// <typeparam name="T4">Type of the in t 4.</typeparam>
    /// <typeparam name="T5">Type of the in t 5.</typeparam>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="arg3">The third argument.</param>
    /// <param name="arg4">The fourth argument.</param>
    /// <param name="arg5">The fifth argument.</param>
    public delegate void Effect<in T1, in T2, in T3, in T4, in T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

    /// <summary>Effect delegate</summary>
    /// <typeparam name="T1">Type of the in t 1.</typeparam>
    /// <typeparam name="T2">Type of the in t 2.</typeparam>
    /// <typeparam name="T3">Type of the in t 3.</typeparam>
    /// <typeparam name="T4">Type of the in t 4.</typeparam>
    /// <typeparam name="T5">Type of the in t 5.</typeparam>
    /// <typeparam name="T6">Type of the in t 6.</typeparam>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="arg3">The third argument.</param>
    /// <param name="arg4">The fourth argument.</param>
    /// <param name="arg5">The fifth argument.</param>
    /// <param name="arg6">The argument 6.</param>
    public delegate void Effect<in T1, in T2, in T3, in T4, in T5, in T6>(
        T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);

    /// <summary>Effect delegate</summary>
    /// <typeparam name="T1">Type of the in t 1.</typeparam>
    /// <typeparam name="T2">Type of the in t 2.</typeparam>
    /// <typeparam name="T3">Type of the in t 3.</typeparam>
    /// <typeparam name="T4">Type of the in t 4.</typeparam>
    /// <typeparam name="T5">Type of the in t 5.</typeparam>
    /// <typeparam name="T6">Type of the in t 6.</typeparam>
    /// <typeparam name="T7">Type of the in t 7.</typeparam>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="arg3">The third argument.</param>
    /// <param name="arg4">The fourth argument.</param>
    /// <param name="arg5">The fifth argument.</param>
    /// <param name="arg6">The argument 6.</param>
    /// <param name="arg7">The argument 7.</param>
    public delegate void Effect<in T1, in T2, in T3, in T4, in T5, in T6, in T7>(
        T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);

    /// <summary>Effect delegate</summary>
    /// <typeparam name="T1">Type of the in t 1.</typeparam>
    /// <typeparam name="T2">Type of the in t 2.</typeparam>
    /// <typeparam name="T3">Type of the in t 3.</typeparam>
    /// <typeparam name="T4">Type of the in t 4.</typeparam>
    /// <typeparam name="T5">Type of the in t 5.</typeparam>
    /// <typeparam name="T6">Type of the in t 6.</typeparam>
    /// <typeparam name="T7">Type of the in t 7.</typeparam>
    /// <typeparam name="T8">Type of the in t 8.</typeparam>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="arg3">The third argument.</param>
    /// <param name="arg4">The fourth argument.</param>
    /// <param name="arg5">The fifth argument.</param>
    /// <param name="arg6">The argument 6.</param>
    /// <param name="arg7">The argument 7.</param>
    /// <param name="arg8">The argument 8.</param>
    public delegate void Effect<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8>(
        T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);

    /// <summary>Effect delegate</summary>
    /// <typeparam name="T1">Type of the in t 1.</typeparam>
    /// <typeparam name="T2">Type of the in t 2.</typeparam>
    /// <typeparam name="T3">Type of the in t 3.</typeparam>
    /// <typeparam name="T4">Type of the in t 4.</typeparam>
    /// <typeparam name="T5">Type of the in t 5.</typeparam>
    /// <typeparam name="T6">Type of the in t 6.</typeparam>
    /// <typeparam name="T7">Type of the in t 7.</typeparam>
    /// <typeparam name="T8">Type of the in t 8.</typeparam>
    /// <typeparam name="T9">Type of the in t 9.</typeparam>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="arg3">The third argument.</param>
    /// <param name="arg4">The fourth argument.</param>
    /// <param name="arg5">The fifth argument.</param>
    /// <param name="arg6">The argument 6.</param>
    /// <param name="arg7">The argument 7.</param>
    /// <param name="arg8">The argument 8.</param>
    /// <param name="arg9">The argument 9.</param>
    public delegate void Effect<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9>(
        T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);

    /// <summary>Effect delegate</summary>
    /// <typeparam name="T1">Type of the in t 1.</typeparam>
    /// <typeparam name="T2">Type of the in t 2.</typeparam>
    /// <typeparam name="T3">Type of the in t 3.</typeparam>
    /// <typeparam name="T4">Type of the in t 4.</typeparam>
    /// <typeparam name="T5">Type of the in t 5.</typeparam>
    /// <typeparam name="T6">Type of the in t 6.</typeparam>
    /// <typeparam name="T7">Type of the in t 7.</typeparam>
    /// <typeparam name="T8">Type of the in t 8.</typeparam>
    /// <typeparam name="T9">Type of the in t 9.</typeparam>
    /// <typeparam name="T10">Type of the in t 10.</typeparam>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="arg3">The third argument.</param>
    /// <param name="arg4">The fourth argument.</param>
    /// <param name="arg5">The fifth argument.</param>
    /// <param name="arg6">The argument 6.</param>
    /// <param name="arg7">The argument 7.</param>
    /// <param name="arg8">The argument 8.</param>
    /// <param name="arg9">The argument 9.</param>
    /// <param name="arg10">The argument 10.</param>
    public delegate void Effect<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10>(
        T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);
}
