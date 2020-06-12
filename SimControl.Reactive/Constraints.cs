// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

namespace SimControl.Reactive
{
    /// <summary>Encapsulates a constraint method that returns a bool value.</summary>
    /// <returns>The constraint evaluation.</returns>
    public delegate bool Constraint();

    /// <summary>Encapsulates a constraint method that has one parameter and returns a bool value.</summary>
    /// <typeparam name="T1">The type of the 1.</typeparam>
    /// <param name="arg1">The arg1.</param>
    /// <returns>The constraint evaluation.</returns>
    public delegate bool Constraint<in T1>(T1 arg1);

    /// <summary>Encapsulates a constraint method that has ten parameters and returns a bool value.</summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <returns>The constraint evaluation.</returns>
    public delegate bool Constraint<in T1, in T2>(T1 arg1, T2 arg2);

    /// <summary>Encapsulates a constraint method that has ten parameters and returns a bool value.</summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="arg3">The third argument.</param>
    /// <returns>The constraint evaluation.</returns>
    public delegate bool Constraint<in T1, in T2, in T3>(T1 arg1, T2 arg2, T3 arg3);

    /// <summary>Encapsulates a constraint method that has ten parameters and returns a bool value.</summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="arg3">The third argument.</param>
    /// <param name="arg4">The fourth argument.</param>
    /// <returns>The constraint evaluation.</returns>
    public delegate bool Constraint<in T1, in T2, in T3, in T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

    /// <summary>Encapsulates a constraint method that has ten parameters and returns a bool value.</summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fith parameter.</typeparam>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="arg3">The third argument.</param>
    /// <param name="arg4">The fourth argument.</param>
    /// <param name="arg5">The fith argument.</param>
    /// <returns>The constraint evaluation.</returns>
    public delegate bool Constraint<in T1, in T2, in T3, in T4, in T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

    /// <summary>Encapsulates a constraint method that has ten parameters and returns a bool value.</summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fith parameter.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter.</typeparam>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="arg3">The third argument.</param>
    /// <param name="arg4">The fourth argument.</param>
    /// <param name="arg5">The fith argument.</param>
    /// <param name="arg6">The sixth argument.</param>
    /// <returns>The constraint evaluation.</returns>
    public delegate bool Constraint<in T1, in T2, in T3, in T4, in T5, in T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);

    /// <summary>Encapsulates a constraint method that has ten parameters and returns a bool value.</summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fith parameter.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter.</typeparam>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="arg3">The third argument.</param>
    /// <param name="arg4">The fourth argument.</param>
    /// <param name="arg5">The fith argument.</param>
    /// <param name="arg6">The sixth argument.</param>
    /// <param name="arg7">The seventh argument.</param>
    /// <returns>The constraint evaluation.</returns>
    public delegate bool Constraint<in T1, in T2, in T3, in T4, in T5, in T6, in T7>(
        T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);

    /// <summary>Encapsulates a constraint method that has ten parameters and returns a bool value.</summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fith parameter.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter.</typeparam>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="arg3">The third argument.</param>
    /// <param name="arg4">The fourth argument.</param>
    /// <param name="arg5">The fith argument.</param>
    /// <param name="arg6">The sixth argument.</param>
    /// <param name="arg7">The seventh argument.</param>
    /// <param name="arg8">The eighth argument.</param>
    /// <returns>The constraint evaluation.</returns>
    public delegate bool Constraint<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8>(
        T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);

    /// <summary>Encapsulates a constraint method that has ten parameters and returns a bool value.</summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fith parameter.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter.</typeparam>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="arg3">The third argument.</param>
    /// <param name="arg4">The fourth argument.</param>
    /// <param name="arg5">The fith argument.</param>
    /// <param name="arg6">The sixth argument.</param>
    /// <param name="arg7">The seventh argument.</param>
    /// <param name="arg8">The eighth argument.</param>
    /// <param name="arg9">The ninth argument.</param>
    /// <returns>The constraint evaluation.</returns>
    public delegate bool Constraint<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9>(
        T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);

    /// <summary>Encapsulates a constraint method that has ten parameters and returns a bool value.</summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fith parameter.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter.</typeparam>
    /// <typeparam name="T10">The type of the thenth parameter.</typeparam>
    /// <param name="arg1">The first argument.</param>
    /// <param name="arg2">The second argument.</param>
    /// <param name="arg3">The third argument.</param>
    /// <param name="arg4">The fourth argument.</param>
    /// <param name="arg5">The fith argument.</param>
    /// <param name="arg6">The sixth argument.</param>
    /// <param name="arg7">The seventh argument.</param>
    /// <param name="arg8">The eighth argument.</param>
    /// <param name="arg9">The ninth argument.</param>
    /// <param name="arg10">The tenth argument.</param>
    /// <returns>The constraint evaluation.</returns>
    public delegate bool Constraint<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10>(
        T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);
}
