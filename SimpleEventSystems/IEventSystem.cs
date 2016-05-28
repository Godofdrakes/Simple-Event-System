using System;


namespace SimpleEventSystems {

    public interface IEventSystem {

        /// <summary>
        ///     Invokes an event, calling all subscribed methods.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of event to invoke.
        /// </typeparam>
        /// <param name="eventData">
        ///     The event to pass to all subscribers.
        /// </param>
        void InvokeEvent<T>( T eventData ) where T : IEvent;

        /// <summary>
        ///     Adds a method to the collection of subscribers of an event.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of event to listen to.
        /// </typeparam>
        /// <param name="methodTarget">
        ///     The method to call when the event is invoked.
        /// </param>
        void Subscribe<T>( Action<T> methodTarget ) where T : IEvent;

        /// <summary>
        ///     Removes a method from the collection of subscribers for an event.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of event to stop listening to.
        /// </typeparam>
        /// <param name="methodTarger">
        ///     The method to be removed.
        /// </param>
        void Unsubscribe<T>( Action<T> methodTarget ) where T : IEvent;

    }

}
