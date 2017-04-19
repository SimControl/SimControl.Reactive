// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using SimControl.Log;

namespace SimControl.TestUtils
{
    /// <summary>Test extensions for asserting timeouts.</summary>
    public static class AssertTimeout
    {
        /// <summary>
        /// Wrapper that calls <see cref="ClientBase{TChannel}.Abort()"/> or <see cref="ClientBase{TChannel}.Close()"/>
        /// while asserting the test timeout.
        /// </summary>
        /// <param name="client">The client to act on.</param>
        public static void AbortOrCloseAssertTimeout( this ICommunicationObject client ) =>
            client.AbortOrCloseAssertTimeout( TestFrame.DefaultTestTimeout );

        /// <summary>
        /// Wrapper that calls <see cref="ClientBase{TChannel}.Abort()"/> or <see cref="ClientBase{TChannel}.Close()"/>
        /// while asserting the test timeout.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="client">The client to act on.</param>
        /// <param name="timeout">The timeout.</param>
        [Log( LogLevel = LogAttributeLevel.Off )]
        public static void AbortOrCloseAssertTimeout( this ICommunicationObject client, int timeout )
        {
            if( client != null )
                if( client.State == CommunicationState.Opened )
                    try
                    {
                        client.Close( TimeSpan.FromMilliseconds( timeout ) );
                    }
                    catch( Exception )
                    {
                        client.Abort();
                        throw;
                    }
                else if( client.State != CommunicationState.Closed )
                    client.Abort();
        }

        /// <summary><see cref="Task"/> wrapper that asserts the test timeout.</summary>
        /// <param name="task">The task to act on.</param>
        public static Task AssertTimeoutAsync( this Task task )
        {
            Contract.Requires( task != null );

            return task.AssertTimeoutAsync( TestFrame.DefaultTestTimeout );
        }

        /// <summary><see cref="Task"/> wrapper that asserts the test timeout.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task">The task to act on.</param>
        /// <returns></returns>
        public static Task<T> AssertTimeoutAsync<T>( this Task<T> task )
        {
            Contract.Requires( task != null );

            return task.AssertTimeoutAsync( TestFrame.DefaultTestTimeout );
        }

        /// <summary><see cref="Task"/> wrapper that asserts the test timeout.</summary>
        /// <exception cref="TimeoutException">Thrown when a Timeout error condition occurs.</exception>
        /// <param name="task">The task to act on.</param>
        /// <param name="timeout">The timeout.</param>
        [Log( LogLevel = LogAttributeLevel.Off )]
        public static async Task AssertTimeoutAsync( this Task task, int timeout )
        {
            Contract.Requires( task != null );

            if( task == await TaskEx.WhenAny( task, TaskEx.Delay( timeout ) ).ConfigureAwait( false ) )
                await task.ConfigureAwait( false );
            else
                throw TimeoutException( timeout );
        }

        /// <summary><see cref="Task"/> wrapper that asserts the test timeout.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task">The task to act on.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        /// <exception cref="System.TimeoutException">
        /// Test timeout " + timeout.ToString( CultureInfo.InvariantCulture ) + " expired
        /// </exception>
        /// <exception cref="TimeoutException">Thrown when a Timeout error condition occurs.</exception>
        [Log( LogLevel = LogAttributeLevel.Off )]
        public static async Task<T> AssertTimeoutAsync<T>( this Task<T> task, int timeout )
        {
            Contract.Requires( task != null );

            if( task == await TaskEx.WhenAny( task, TaskEx.Delay( timeout ) ).ConfigureAwait( false ) )
                return await task.ConfigureAwait( false );

            throw TimeoutException( timeout );
        }

        /// <summary><see cref="Thread.Join()"/> wrapper that asserts the test timeout.</summary>
        /// <param name="thread">The thread to act on.</param>
        public static void JoinAssertTimeout( this Thread thread )
        {
            Contract.Requires( thread != null );

            thread.JoinAssertTimeout( TestFrame.DefaultTestTimeout );
        }

        /// <summary><see cref="Thread.Join()"/> wrapper that asserts the test timeout.</summary>
        /// <exception cref="TimeoutException">Thrown when a Timeout error condition occurs.</exception>
        /// <param name="thread">The thread to act on.</param>
        /// <param name="timeout">The timeout.</param>
        [Log( LogLevel = LogAttributeLevel.Off )]
        public static void JoinAssertTimeout( this Thread thread, int timeout )
        {
            Contract.Requires( thread != null );

            if( !thread.Join( TestFrame.DisableDebugTimeout( timeout ) ) )
                throw TimeoutException( timeout );
        }

        /// <summary>The Process extension method that kills a process while asserting the test timeout.</summary>
        /// <param name="process">The process to act on.</param>
        /// <returns>An int.</returns>
        public static int KillAssertTimeout( this Process process )
        {
            Contract.Requires( process != null );

            return process.KillAssertTimeout( TestFrame.DefaultTestTimeout );
        }

        /// <summary>The Process extension method that kills a process while asserting the test timeout.</summary>
        /// <exception cref="TimeoutException">Thrown when a Timeout error condition occurs.</exception>
        /// <param name="process">The process to act on.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>An int.</returns>
        [SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes" )]
        [Log( LogLevel = LogAttributeLevel.Off )]
        public static int KillAssertTimeout( this Process process, int timeout )
        {
            Contract.Requires( process != null );

            try
            {
                try
                {
                    process.Kill();
                }
                catch( Exception e )
                {
                    logger.Warn( e );
                }

                if( !process.WaitForExit( TestFrame.DisableDebugTimeout( timeout ) ) )
                    throw TimeoutException( timeout );

                return process.ExitCode;
            }
            finally
            {
                process.Dispose();
            }
        }

        /// <summary><see cref="Task{T}.Result"/> wrapper that asserts the test timeout.</summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="task">The task to act on.</param>
        /// <returns>A T.</returns>
        public static T ResultAssertTimeout<T>( this Task<T> task )
        {
            Contract.Requires( task != null );

            return task.ResultAssertTimeout( TestFrame.DefaultTestTimeout );
        }

        /// <summary><see cref="Task{T}.Result"/> wrapper that asserts the test timeout.</summary>
        /// <exception cref="TimeoutException">Thrown when a Timeout error condition occurs.</exception>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="task">The task to act on.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>A T.</returns>
        [Log( LogLevel = LogAttributeLevel.Off )]
        public static T ResultAssertTimeout<T>( this Task<T> task, int timeout )
        {
            Contract.Requires( task != null );

            bool timeoutFailed;

            try { timeoutFailed = !task.Wait( TestFrame.DisableDebugTimeout( timeout ) ); }
            catch( AggregateException e ) { throw e.InnerException; }

            if( timeoutFailed )
            {
                task.IgnoreFaults();
                throw TimeoutException( timeout );
            }
            return task.Result;
        }

        /// <summary>Executes the given action on the thread pool while asserting the test timeout.</summary>
        /// <param name="action">The action.</param>
        public static void RunAssertTimeout( this Action action )
        {
            Contract.Requires( action != null );

            action.RunAssertTimeout( TestFrame.DefaultTestTimeout );
        }

        /// <summary>Executes the given action on the thread pool while asserting the test timeout.</summary>
        /// <param name="action">The action.</param>
        /// <param name="timeout">The timeout.</param>
        [SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes" )]
        [Log( LogLevel = LogAttributeLevel.Off )]
        public static void RunAssertTimeout( this Action action, int timeout )
        {
            Contract.Requires( action != null );

            Task task = TaskEx.Run(action);

            bool timeoutFailed;

            try { timeoutFailed = !task.Wait( TestFrame.DisableDebugTimeout( timeout ) ); }
            catch( AggregateException e ) { throw e.InnerException; }

            if( timeoutFailed )
            {
                task.IgnoreFaults();
                throw TimeoutException( timeout );
            }
        }

        /// <summary>Executes the given action on the thread pool while asserting the test timeout.</summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="function">The function.</param>
        /// <returns>A T.</returns>
        public static T RunAssertTimeout<T>( this Func<T> function )
        {
            Contract.Requires( function != null );

            return function.RunAssertTimeout( TestFrame.DefaultTestTimeout );
        }

        /// <summary>Executes the given action on the thread pool while asserting the test timeout.</summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="function">The function.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>A T.</returns>
        [SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes" )]
        [Log( LogLevel = LogAttributeLevel.Off )]
        public static T RunAssertTimeout<T>( this Func<T> function, int timeout )
        {
            Contract.Requires( function != null );

            var task = TaskEx.Run(function);

            bool timeoutFailed;

            try { timeoutFailed = !task.Wait( TestFrame.DisableDebugTimeout( timeout ) ); }
            catch( AggregateException e ) { throw e.InnerException; }

            if( timeoutFailed )
            {
                task.IgnoreFaults();
                throw TimeoutException( timeout );
            }
            return task.Result;
        }

        /// <summary><see cref="BlockingCollection{T}.Take()"/> wrapper that asserts the test timeout.</summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="blockingCollection">The asyncCollection to act on.</param>
        /// <returns>A T.</returns>
        public static T TakeAssertTimeout<T>( this BlockingCollection<T> blockingCollection )
        {
            Contract.Requires( blockingCollection != null );

            return blockingCollection.TakeAssertTimeout( TestFrame.DefaultTestTimeout );
        }

        /// <summary><see cref="BlockingCollection{T}.Take()"/> wrapper that asserts the test timeout.</summary>
        /// <exception cref="TimeoutException">Thrown when a Timeout error condition occurs.</exception>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="blockingCollection">The asyncCollection to act on.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>A T.</returns>
        [Log( LogLevel = LogAttributeLevel.Off )]
        public static T TakeAssertTimeout<T>( this BlockingCollection<T> blockingCollection, int timeout )
        {
            Contract.Requires( blockingCollection != null );

            if( !blockingCollection.TryTake( out T result, timeout ) )
                throw TimeoutException( timeout );

            return result;
        }

        /// <summary>
        /// Take an item from the blockingCollection until either func becomes true or the timeout expires.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="blockingCollection">The blockingCollection to act on.</param>
        /// <param name="func">The function.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process take until assert timeout in this collection.
        /// </returns>
        public static IEnumerable<T> TakeUntilAssertTimeout<T>( this BlockingCollection<T> blockingCollection,
                                                               Func<T, bool> func )
        {
            Contract.Requires( blockingCollection != null );
            Contract.Requires( func != null );

            return blockingCollection.TakeUntilAssertTimeout( func, TestFrame.DefaultTestTimeout );
        }

        /// <summary>
        /// Take an item from the blockingCollection until either func becomes true or the timeout expires.
        /// </summary>
        /// <exception cref="TimeoutException">Thrown when a Timeout error condition occurs.</exception>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="asyncCollection">The asyncCollection to act on.</param>
        /// <param name="func">The function.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process take until assert timeout in this collection.
        /// </returns>
        [Log( LogLevel = LogAttributeLevel.Off )]
        public static IEnumerable<T> TakeUntilAssertTimeout<T>( this BlockingCollection<T> asyncCollection,
                                                               Func<T, bool> func, int timeout )
        {
            Contract.Requires( asyncCollection != null );
            Contract.Requires( func != null );

            var result = new List<T>();

            using( var timeoutCancel = new CancellationTokenSource() )
            {
                timeoutCancel.CancelAfter( TestFrame.DisableDebugTimeout( timeout ) );

                for( ;;)
                    try
                    {
                        T item = asyncCollection.Take(timeoutCancel.Token);

                        result.Add( item );

                        if( func( item ) )
                            return result;
                    }
                    catch( OperationCanceledException )
                    {
                        throw TimeoutException( timeout );
                    }
            }
        }

        /// <summary><see cref="Task.Wait()"/> wrapper that asserts the test timeout.</summary>
        /// <param name="task">The task to act on.</param>
        public static void WaitAssertTimeout( this Task task )
        {
            Contract.Requires( task != null );

            task.WaitAssertTimeout( TestFrame.DefaultTestTimeout );
        }

        /// <summary><see cref="Task.Wait()"/> wrapper that asserts the test timeout.</summary>
        /// <exception cref="TimeoutException">Thrown when a Timeout error condition occurs.</exception>
        /// <param name="task">The task to act on.</param>
        /// <param name="timeout">The timeout.</param>
        [Log( LogLevel = LogAttributeLevel.Off )]
        public static void WaitAssertTimeout( this Task task, int timeout )
        {
            Contract.Requires( task != null );

            bool timeoutFailed;

            try { timeoutFailed = !task.Wait( TestFrame.DisableDebugTimeout( timeout ) ); }
            catch( AggregateException e ) { throw e.InnerException; }

            if( timeoutFailed )
            {
                task.IgnoreFaults();
                throw TimeoutException( timeout );
            }
        }

        /// <summary>Wrapper that asserts the test timeout.</summary>
        /// <param name="semaphore">The semaphore.</param>
        /// <param name="count">The count.</param>
        public static void WaitAssertTimeout( this SemaphoreSlim semaphore, int count = 1 )
        {
            Contract.Requires( semaphore != null );

            semaphore.WaitAssertTimeout( count, TestFrame.DefaultTestTimeout );
        }

        /// <summary>Wrapper that asserts the test timeout.</summary>
        /// <param name="semaphore">The semaphore.</param>
        /// <param name="count">The count.</param>
        /// <param name="timeout">The timeout.</param>
        /// <exception cref="TimeoutException">
        /// Test timeout + timeout.ToString(CultureInfo.InvariantCulture) + expired
        /// </exception>
        [Log( LogLevel = LogAttributeLevel.Off )]
        public static void WaitAssertTimeout( this SemaphoreSlim semaphore, int count, int timeout )
        {
            Contract.Requires( semaphore != null );

            using( var timeoutCancel = new CancellationTokenSource() )
            {
                timeoutCancel.CancelAfter( TestFrame.DisableDebugTimeout( timeout ) );

                try { while( count-- > 0 ) semaphore.Wait( timeoutCancel.Token ); }
                catch( OperationCanceledException )
                {
                    throw TimeoutException( timeout );
                }
            }
        }

        /// <summary><see cref="WaitHandle.WaitOne()"/> Wrapper that asserts the test timeout.</summary>
        /// <param name="waitHandle">The waitHandle to act on.</param>
        public static void WaitOneAssertTimeout( this WaitHandle waitHandle )
        {
            Contract.Requires( waitHandle != null );

            waitHandle.WaitOneAssertTimeout( TestFrame.DefaultTestTimeout );
        }

        /// <summary>cref="WaitHandle.WaitOne()"&gt; Wrapper that asserts the test timeout.</summary>
        /// <exception cref="TimeoutException">Thrown when a Timeout error condition occurs.</exception>
        /// <param name="waitHandle">The waitHandle to act on.</param>
        /// <param name="timeout">The timeout.</param>
        [Log( LogLevel = LogAttributeLevel.Off )]
        public static void WaitOneAssertTimeout( this WaitHandle waitHandle, int timeout )
        {
            Contract.Requires( waitHandle != null );

            if( !waitHandle.WaitOne( TestFrame.DisableDebugTimeout( timeout ) ) )
                throw TimeoutException( timeout );
        }

        private static void IgnoreFaults( this Task task ) =>
            task.ContinueWith( t => { }, TaskContinuationOptions.OnlyOnFaulted );

        private static TimeoutException TimeoutException( int timeout ) =>
            new TimeoutException( "Test timeout " + timeout.ToString( CultureInfo.InvariantCulture ) + " expired" );

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
}
