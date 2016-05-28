using System;
using System.Collections.Generic;


namespace SimpleEventSystems {

    /// <summary>
    ///     Manages subscriptions to and invocations of <see cref="IEvent" />s.
    /// </summary>
    public class EventSystem : IEventSystem {

        private Dictionary<Type, Delegate> m_subscriberDictionary;

        public EventSystem() { m_subscriberDictionary = new Dictionary<Type, Delegate>(); }

        /// <summary>
        ///     Invokes an event, calling all subscribed methods.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of event to invoke.
        /// </typeparam>
        /// <param name="eventData">
        ///     The event to pass to all subscribers.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="eventData" /> is null.
        /// </exception>
        public void InvokeEvent<T>( T eventData ) where T : IEvent {
            if( eventData == null ) { throw new ArgumentNullException( "eventData" ); }

            if( m_subscriberDictionary.ContainsKey( typeof( T ) ) &&
                m_subscriberDictionary[typeof( T )] != null ) {
                Delegate[] subscribers = m_subscriberDictionary[typeof( T )].GetInvocationList();
#if LOG_INFO
                Console.WriteLine( "#{0}# Invoking event {1}. {2} subscribers.",
                                   typeof( EventSystem ).Name,
                                   typeof( T ).Name,
                                   subscribers.Length );
#endif
                foreach( Delegate subscriber in subscribers ) {
#if LOG_INFO
                    Console.WriteLine( "#{0}# Calling subscriber {1}.{2}",
                                       typeof( EventSystem ).Name,
                                       subscriber.Target.GetType().Name,
                                       subscriber.Method.Name );
#endif
                    subscriber.DynamicInvoke( eventData );
                }
#if LOG_INFO
                Console.WriteLine( "#{0}# Finished calling subscribers.",
                                   typeof( EventSystem ).Name );
#endif
            }
#if LOG_INFO
            Console.WriteLine( "#{0}# Did not invoke event {1}, no subscribers.",
                               typeof( EventSystem ).Name,
                               typeof( T ) );
#endif
        }

        /// <summary>
        ///     Adds a method to the collection of subscribers of an event.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of event to listen to.
        /// </typeparam>
        /// <param name="methodTarget">
        ///     The method to call when the event is invoked.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="methodTarget" /> is null.
        /// </exception>
        public void Subscribe<T>( Action<T> methodTarget ) where T : IEvent {
            if( methodTarget == null ) { throw new ArgumentNullException( "methodTarget" ); }

            if( !m_subscriberDictionary.ContainsKey( typeof( T ) ) ) {
                m_subscriberDictionary.Add( typeof( T ), null );
            }

#if LOG_INFO
            Console.WriteLine( "#{0}# Subscribing method '{1}.{2}'.",
                               typeof( EventSystem ).Name,
                               methodTarget.Target.GetType().Name,
                               methodTarget.Method.Name );
#endif

            m_subscriberDictionary[typeof( T )] =
                Delegate.Combine( m_subscriberDictionary[typeof( T )], methodTarget );
        }

        /// <summary>
        ///     Removes a method from the collection of subscribers for an event.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of event to stop listening to.
        /// </typeparam>
        /// <param name="methodTarger">
        ///     The method to be removed.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="methodTarget" /> is null.
        /// </exception>
        public void Unsubscribe<T>( Action<T> methodTarget ) where T : IEvent {
            if( methodTarget == null ) { throw new ArgumentNullException( "methodTarget" ); }

            if( m_subscriberDictionary.ContainsKey( typeof( T ) ) ) {
#if LOG_INFO
                Console.WriteLine( "#{0}# Unsubscribing method '{1}.{2}'.",
                                   typeof( EventSystem ).Name,
                                   methodTarget.Target.GetType().Name,
                                   methodTarget.Method.Name );
#endif

                m_subscriberDictionary[typeof( T )] =
                    Delegate.RemoveAll( m_subscriberDictionary[typeof( T )], methodTarget );
            }
        }

    }

}
