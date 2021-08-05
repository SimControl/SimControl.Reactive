// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

// TODO CR

#if false
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace SimControl.Reactive
{
    /*
    internal interface IActiveObject: INotifyPropertyChanged
    {
        Task RunAsync();

        bool RunSynchronously { get; set; }
    }

    public class ActiveObject: IActiveObject
    {
        public ActiveObject()
        {
            if (SynchronizationContext.Current != null)
                TaskScheduler.FromCurrentSynchronizationContext();
            else
                taskScheduler = new NonConcurrentTaskScheduler();
        }

        public ActiveObject(TaskScheduler taskScheduler) { this.taskScheduler = taskScheduler; }

        public Task RunAsync() { throw new NotImplementedException(); }

        private TaskScheduler taskScheduler;

        protected Task Schedule(Action action)
        {
            return Task.Factory.StartNew(action, CancellationToken.None, TaskCreationOptions.AttachedToParent,
                taskScheduler);


            //return TaskEx.Run(action);

        }

        protected Task Schedule(Action action, TimeSpan delay) { return TaskEx.Run(action); }
        protected Task Schedule(Action action, CancellationToken cancellationToken) { return TaskEx.Run(action); }
        protected Task Schedule(Action action, TimeSpan delay, CancellationToken cancellationToken) { return TaskEx.Run(action); }

        protected Task<T> Schedule<T>(Func<T> func, TimeSpan delay) { return TaskEx.Run(func); }
        protected Task<T> Schedule<T>(Func<T> func) { return TaskEx.Run(func); }
        protected Task<T> Schedule<T>(Func<T> func, TimeSpan delay, CancellationToken cancellationToken) { return TaskEx.Run(func); }
        protected Task<T> Schedule<T>(Func<T> func, CancellationToken cancellationToken) { return TaskEx.Run(func); }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool RunSynchronously
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }

    public class Light: ActiveObject
    {
        public Light() { }
        public Light(TaskScheduler taskScheduler): base(taskScheduler) { }

        public Task BlinkAsync(int times) { return Schedule(() => Blink(times)); }
        public Task OffAsync() { return Schedule(Off); }
        public Task OnAsync() { return Schedule<int>(On); }
        public Task ResetAsync() { return Schedule(Reset); }

        private void Off() { IsOn = false; }

        private int On()
        {
            IsOn = true;
            Counter++;
            Schedule(Off, delay);
            return Counter;
        }

        private async Task<int> Blink(int times)
        {
            while (times > 0)
            {
                if (IsOn)
                {
                    IsOn = false;
                    await TaskEx.Delay(BlinkDelay).ConfigureAwait(false);
                }
                else
                {
                    IsOn = true;
                    Counter++;
                    times--;
                    await TaskEx.Delay(BlinkDelay).ConfigureAwait(false);
                }
            }
            
            return Counter;
        }

        private void Reset()
        {
            IsOn = false;
            Counter = 0;
        }

        public int Counter { get; private set; }
        
        public TimeSpan BlinkDelay 
        { 
            get { return blinkDelay; } 
            set { Schedule(() => blinkDelay = value); } 
        }

        public TimeSpan Delay 
        { 
            get { return delay; } 
            set { Schedule(() => delay = value); } 
        }

        public bool IsOn { get; private set; }

        private TimeSpan delay;
        private TimeSpan blinkDelay;
    }


    // Provides a task scheduler that ensures a maximum concurrency level while  
    // running on top of the thread pool. 
    public class NonConcurrentTaskScheduler: TaskScheduler
    {
        // Indicates whether the current thread is processing work items.
        [ThreadStatic]
        private static bool _currentThreadIsProcessingItems;

        // The list of tasks to be executed  
        private readonly LinkedList<Task> _tasks = new LinkedList<Task>(); // protected by lock(_tasks) 

        // Indicates whether the scheduler is currently processing work items.  
        private bool _delegatesQueuedOrRunning = false;

        // Queues a task to the scheduler.  
        protected override sealed void QueueTask(Task task)
        {
            // Add the task to the list of tasks to be processed.  If there aren't enough  
            // delegates currently queued or running to process tasks, schedule another.  
            lock (_tasks)
            {
                _tasks.AddLast(task);
                if (!_delegatesQueuedOrRunning)
                {
                    _delegatesQueuedOrRunning = true;
                    NotifyThreadPoolOfPendingWork();
                }
            }
        }

        // Inform the ThreadPool that there's work to be executed for this scheduler.  
        private void NotifyThreadPoolOfPendingWork()
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                // Note that the current thread is now processing work items. 
                // This is necessary to enable inlining of tasks into this thread.
                _currentThreadIsProcessingItems = true;
                try
                {
                    // Process all available items in the queue. 
                    while (true)
                    {
                        Task item;
                        lock (_tasks)
                        {
                            // When there are no more items to be processed, 
                            // note that we're done processing, and get out. 
                            if (_tasks.Count == 0)
                            {
                                _delegatesQueuedOrRunning = false;
                                break;
                            }

                            // Get the next item from the queue
                            item = _tasks.First.Value;
                            _tasks.RemoveFirst();
                        }

                        // Execute the task we pulled out of the queue 
                        base.TryExecuteTask(item);
                    }
                }
                    // We're done processing items on the current thread 
                finally
                {
                    _currentThreadIsProcessingItems = false;
                }
            });
        }

        // Attempts to execute the specified task on the current thread.  
        protected override sealed bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            // If this thread isn't already processing a task, we don't support inlining 
            if (!_currentThreadIsProcessingItems)
                return false;

            // If the task was previously queued, remove it from the queue 
            if (taskWasPreviouslyQueued)
                // Try to run the task.  
                if (TryDequeue(task))
                    return base.TryExecuteTask(task);
                else
                    return false;
            else
                return base.TryExecuteTask(task);
        }

        // Attempt to remove a previously scheduled task from the scheduler.  
        protected override sealed bool TryDequeue(Task task)
        {
            lock (_tasks)
                return _tasks.Remove(task);
        }

        // Gets the maximum concurrency level supported by this scheduler.  
        public override sealed int MaximumConcurrencyLevel
        {
            get { return 1; }
        }

        // Gets an enumerable of the tasks currently scheduled on this scheduler.  
        protected override sealed IEnumerable<Task> GetScheduledTasks()
        {
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(_tasks, ref lockTaken);
                if (lockTaken)
                    return _tasks;
                else
                    throw new NotSupportedException();
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(_tasks);
            }
        }
    }
*/

    /*

    /// <summary>
    /// DispatchedOperation is used to mark resource operations which are dispatched to the ConnectedResource.
    /// </summary>
    [Serializable, AttributeUsage(AttributeTargets.Method)]
    public sealed class AsyncCall: MethodInterceptionAspect
    {
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            ((ActiveObject) args.Instance).RegisterEvent(new Effect(args.Proceed));
        }

        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
    }

    [Serializable, AttributeUsage( AttributeTargets.Property )]
    public sealed class Locked : MethodInterceptionAspect
    {
        public override void OnInvoke( MethodInterceptionArgs args )
        {
            ((ActiveObject)args.Instance).RegisterEvent( new Effect( args.Proceed ) );
        }

        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
    }


    public interface IActiveObject: IDisposable
    {
        //IActiveObject(IScheduler scheduler);

        void OnEventEntry(Delegate del, object[] args);
        void OnEventExit(Delegate del, object[] args);
        void OnEventException(Delegate del, object[] args, Exception e);

        Scheduler Scheduler { get; }
    }

    public interface IScheduler: IActiveObject, IDisposable
    {
    }

    public abstract class ActiveObject
    {
        [LogAttribute(typeof(ActiveObject))]
        protected ActiveObject(string name)
        {
            if (name == null) throw new ArgumentException("ActiveObject parameter name must not be null", "name");

            Name = name;
            lock (staticLocker) ActiveObjectCount++;
        }

        public void Delete() { RegisterEvent(new Effect(DeleteAction)); }

//        public void WaitForPendingActions() { RegisterWaitEvent(new Effect(DummyAction)).Wait(); }

        public bool IsValid {  get { return Scheduler != null; } }
        public string FullName { get; private set; }
        public string Name { get; private set; }

//        public ActiveObjectCollection Parent { get; private set; }
        public Scheduler Scheduler { get; private set; }

        public static int ActiveObjectCount { get; private set; }

        public override string ToString()  {  return Log.Object(typeof(ActiveObject), FullName, IsValid); }

        protected ActiveObjectCollection Parent;

        protected internal virtual void DeleteAction()
        {
            Scheduler.DeleteActiveObject(this);

            if (Parent != null) Parent.RemoveChild(this);

            Name = null;
            Parent = null;
            Scheduler = null;

            lock (staticLocker) ActiveObjectCount--;
        }

        internal protected void RegisterEvent(Effect del) { Scheduler.AddEvent(this, TimeSpan.Zero, del); }
        protected void RegisterEvent<TA>(Effect<TA> del, TA a) { Scheduler.AddEvent(this, TimeSpan.Zero, del, a); }
        protected void RegisterEvent<TA, TB>(Effect<TA, TB> del, TA a, TB b) { Scheduler.AddEvent(this, TimeSpan.Zero, del, a, b); }
        protected void RegisterEvent<TA, TB, TC>(Effect<TA, TB, TC> del, TA a, TB b, TC c) { Scheduler.AddEvent(this, TimeSpan.Zero, del, a, b, c); }
        protected void RegisterEvent<TA, TB, TC, TD>(Effect<TA, TB, TC, TD> del, TA a, TB b, TC c, TD d) { Scheduler.AddEvent(this, TimeSpan.Zero, del, a, b, c, d); }
        protected void RegisterEvent<TA, TB, TC, TD, TE>(Effect<TA, TB, TC, TD, TE> del, TA a, TB b, TC c, TD d, TE e) { Scheduler.AddEvent(this, TimeSpan.Zero, del, a, b, c, d, e); }
        protected void RegisterEvent<TA, TB, TC, TD, TE, TF>(Effect<TA, TB, TC, TD, TE, TF> del, TA a, TB b, TC c, TD d, TE e, TF f) { Scheduler.AddEvent(this, TimeSpan.Zero, del, a, b, c, d, e, f); }
        protected void RegisterEvent<TA, TB, TC, TD, TE, TF, TG>(Effect<TA, TB, TC, TD, TE, TF, TG> del, TA a, TB b, TC c, TD d, TE e, TF f, TG g) { Scheduler.AddEvent(this, TimeSpan.Zero, del, a, b, c, d, e, f, g); }
        protected void RegisterEvent<TA, TB, TC, TD, TE, TF, TG, TH>(Effect<TA, TB, TC, TD, TE, TF, TG, TH> del, TA a, TB b, TC c, TD d, TE e, TF f, TG g, TH h) { Scheduler.AddEvent(this, TimeSpan.Zero, del, a, b, c, d, e, f, g, h); }
        protected void RegisterEvent<TA, TB, TC, TD, TE, TF, TG, TH, TI>(Effect<TA, TB, TC, TD, TE, TF, TG, TH, TI> del, TA a, TB b, TC c, TD d, TE e, TF f, TG g, TH h, TI i) { Scheduler.AddEvent(this, TimeSpan.Zero, del, a, b, c, d, e, f, g, h, i); }
        protected void RegisterEvent<TA, TB, TC, TD, TE, TF, TG, TH, TI, TJ>(Effect<TA, TB, TC, TD, TE, TF, TG, TH, TI, TJ> del, TA a, TB b, TC c, TD d, TE e, TF f, TG g, TH h, TI i, TJ j) { Scheduler.AddEvent(this, TimeSpan.Zero, del, a, b, c, d, e, f, g, h, i, j); }

        protected ActionEvent RegisterEvent(TimeSpan timeSpan, Effect del) { return Scheduler.AddEvent(this, timeSpan, del); }
        protected ActionEvent RegisterEvent<TA>(TimeSpan timeSpan, Effect<TA> del, TA a) { return Scheduler.AddEvent(this, timeSpan, del, a); }
        protected ActionEvent RegisterEvent<TA, TB>(TimeSpan timeSpan, Effect<TA, TB> del, TA a, TB b) { return Scheduler.AddEvent(this, timeSpan, del, a, b); }
        protected ActionEvent RegisterEvent<TA, TB, TC>(TimeSpan timeSpan, Effect<TA, TB, TC> del, TA a, TB b, TC c) { return Scheduler.AddEvent(this, timeSpan, del, a, b, c); }
        protected ActionEvent RegisterEvent<TA, TB, TC, TD>(TimeSpan timeSpan, Effect<TA, TB, TC, TD> del, TA a, TB b, TC c, TD d) { return Scheduler.AddEvent(this, timeSpan, del, a, b, c, d); }
        protected ActionEvent RegisterEvent<TA, TB, TC, TD, TE>(TimeSpan timeSpan, Effect<TA, TB, TC, TD, TE> del, TA a, TB b, TC c, TD d, TE e) { return Scheduler.AddEvent(this, timeSpan, del, a, b, c, d, e); }
        protected ActionEvent RegisterEvent<TA, TB, TC, TD, TE, TF>(TimeSpan timeSpan, Effect<TA, TB, TC, TD, TE, TF> del, TA a, TB b, TC c, TD d, TE e, TF f) { return Scheduler.AddEvent(this, timeSpan, del, a, b, c, d, e, f); }
        protected ActionEvent RegisterEvent<TA, TB, TC, TD, TE, TF, TG>(TimeSpan timeSpan, Effect<TA, TB, TC, TD, TE, TF, TG> del, TA a, TB b, TC c, TD d, TE e, TF f, TG g) { return Scheduler.AddEvent(this, timeSpan, del, a, b, c, d, e, f, g); }
        protected ActionEvent RegisterEvent<TA, TB, TC, TD, TE, TF, TG, TH>(TimeSpan timeSpan, Effect<TA, TB, TC, TD, TE, TF, TG, TH> del, TA a, TB b, TC c, TD d, TE e, TF f, TG g, TH h) { return Scheduler.AddEvent(this, timeSpan, del, a, b, c, d, e, f, g, h); }
        protected ActionEvent RegisterEvent<TA, TB, TC, TD, TE, TF, TG, TH, TI>(TimeSpan timeSpan, Effect<TA, TB, TC, TD, TE, TF, TG, TH, TI> del, TA a, TB b, TC c, TD d, TE e, TF f, TG g, TH h, TI i) { return Scheduler.AddEvent(this, timeSpan, del, a, b, c, d, e, f, g, h, i); }
        protected ActionEvent RegisterEvent<TA, TB, TC, TD, TE, TF, TG, TH, TI, TJ>(TimeSpan timeSpan, Effect<TA, TB, TC, TD, TE, TF, TG, TH, TI, TJ> del, TA a, TB b, TC c, TD d, TE e, TF f, TG g, TH h, TI i, TJ j) { return Scheduler.AddEvent(this, timeSpan, del, a, b, c, d, e, f, g, h, i, j); }

        protected bool UnregisterEvent(ActionEvent actionEvent) { return Scheduler.RemoveEvent(actionEvent); }

        protected internal void AssertIsValid() { if (!IsValid) throw new ObjectDisposedException("FullName"); }
        protected void AssertThreadContext() { if (System.Threading.Thread.CurrentThread != Scheduler.Thread) throw new InvalidOperationException("Effect called from wrong thread context for object: " + Name); }

        protected void FireEvent(Delegate del, params Object[] args)
        {
            if (del != null)
                foreach (Delegate sink in del.GetInvocationList())
                    try
                    {
                        ActiveObject Active = sink.Target as ActiveObject;

                        if (Active != null)
                            if (Active.Scheduler == Scheduler)
                                sink.DynamicInvoke(args);
                            else
                                Active.Scheduler.AddEvent(Active, TimeSpan.Zero, sink, args);
                        else
                        {
                            InvokeDelegate(sink, args);

//                            AsyncFire asyncFire = new AsyncFire(InvokeDelegate);
//                            asyncFire.BeginInvoke(sink, args, new AsyncCallback(CleanupFireEvent), asyncFire);
                        }
                    }
                    catch (Exception e)
                    {
                        logger.ErrorException(Log.LaException(this), e);
                        throw;
                    }
        }

        internal virtual void SetParent(ActiveObjectCollection parent)
        {
            Parent = parent;
            FullName = (parent == null) ? Name : parent.FullName + "." + Name;

            if (parent == null)
                SetScheduler((Scheduler) this);
            else if (parent.Scheduler != null)
                SetScheduler(parent.Scheduler);
        }

        internal virtual void SetScheduler(Scheduler scheduler) { Scheduler = scheduler; }

        //        private void DummyAction() {}

        private delegate void AsyncFire(Delegate del, object[] args);

        private void CleanupFireEvent(IAsyncResult asyncResult)
        {
            try
            {
                AsyncFire asyncFire = (AsyncFire) asyncResult.AsyncState;
                asyncFire.EndInvoke(asyncResult);
            }
            catch (Exception e) { logger.ErrorException(Log.LaException(this), e); } // might not be threadsafe
        }

        private static void InvokeDelegate(Delegate del, object[] args)
        {
            ISynchronizeInvoke synchronizer = del.Target as ISynchronizeInvoke;

            if (synchronizer != null && synchronizer.InvokeRequired)
                synchronizer.Invoke(del, args);
            else
                del.DynamicInvoke(args);
        }

        private Object staticLocker = new Object();
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
    }

    public class ActiveObjectNotifyProperties: ActiveObject, INotifyPropertyChanged
    {
        public ActiveObjectNotifyProperties(string name) : base(name) { }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { RegisterEvent(() => propertyChanged += value); }
            remove { RegisterEvent(() => propertyChanged -= value); }
        }

        protected void OnPropertyChanged(string propertyName) { FireEvent(propertyChanged, this, new PropertyChangedEventArgs(propertyName)); }

        private PropertyChangedEventHandler propertyChanged;
    }

    public abstract class ActiveObjectCollection: ActiveObjectNotifyProperties, IEnumerable<ActiveObject>, INotifyCollectionChanged
    {
        protected ActiveObjectCollection(string name) : base(name) { }

        public IEnumerator<ActiveObject> GetEnumerator() { return (IEnumerator<ActiveObject>) childrenArray.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return childrenArray.GetEnumerator(); }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { RegisterEvent(() => collectionChanged += value); }
            remove { RegisterEvent(() => collectionChanged -= value); }
        }

        public void Add(params ActiveObject[] child) { RegisterEvent<ActiveObject[]>(AddAction, child); }

        public void AddAction(params ActiveObject[] newChildren)
        {
            AssertIsValid();

            logger.Debug(Log.LaMessage(this, newChildren));

            foreach (ActiveObject child in newChildren)
            {
//                ActiveObject oldItem = null;

                if (children == null) this.children = new Dictionary<string, ActiveObject>();
                if (children.ContainsKey(child.Name))
                {
  //                  oldItem = this.children[child.Name];
                    children[child.Name].DeleteAction();
                }
                children[child.Name] = child;
                child.SetParent(this);
            }
            childrenArray = children.Values.ToArray<ActiveObject>();

//            FireEvent(collectionChanged, this, oldItem == null ? new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, child) : new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, child, oldItem));  TODO: EventArgs parameters
        }

        protected internal override void DeleteAction()
        {
            foreach (ActiveObject child in (childrenArray == null ? children.Values.ToArray<ActiveObject>() : childrenArray))
                child.DeleteAction();

            children = null;
            childrenArray = null;
            base.DeleteAction();
        }

        internal void RemoveChild(ActiveObject child)
        {
            children.Remove(child.Name);
            childrenArray = this.children.Values.ToArray<ActiveObject>();

            //            FireEvent(collectionChanged, this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, child));
        }

        internal override void SetScheduler(Scheduler scheduler)
        {
            base.SetScheduler(scheduler);
            if (children != null)
                foreach (ActiveObject child in children.Values)
                    child.SetScheduler(scheduler);
        }

        private Dictionary<string, ActiveObject> children;
        private ActiveObject[] childrenArray = new ActiveObject[0];
        private NotifyCollectionChangedEventHandler collectionChanged;

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
    }
    */
}
#endif
