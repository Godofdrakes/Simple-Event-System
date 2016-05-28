using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace SimpleEventSystems {

    /// <summary>
    ///     Manages subscriptions to and invocations of <see cref="IEvent" />s.
    /// </summary>
    public class EventSystem : IEventSystem {

        private Dictionary<Type, Delegate> m_subscriberDictionary;

        public EventSystem() {
            m_subscriberDictionary = new Dictionary<Type, Delegate>();
            LogInfo = false;
        }

        /// <summary>
        ///     Logs non-essential information. Can be usefull for debugging.
        /// </summary>
        public bool LogInfo { get; set; }

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

            if( m_subscriberDictionary.ContainsKey( typeof( T ) ) ) {
                Delegate[] subscribers = m_subscriberDictionary[typeof( T )].GetInvocationList();
                if( LogInfo ) {
                    Debug.WriteLine( "#{0}# Invoking event {1}. {2} subscribers.",
                                     typeof( EventSystem ),
                                     typeof( T ).Name,
                                     subscribers.Length );
                }
                foreach( Delegate subscriber in subscribers ) {
                    if( LogInfo ) {
                        Debug.WriteLine( "#{0}# Calling subscriber {1}.{2}",
                                         typeof( EventSystem ),
                                         subscriber.Target.GetType().Name,
                                         subscriber.Method.Name );
                    }
                    try {
                        subscriber.DynamicInvoke( eventData );
                    }
                    catch( Exception e ) {
                        if( LogInfo ) {
                            Debug.WriteLine( "#{0}# Exception {1} thrown during invocation." );
                        }
                        throw;
                    }
                }
            }
            else if( LogInfo ) {
                Debug.WriteLine( "#{0}# Did not invoke event {1}, no subscribers.",
                                 typeof( EventSystem ),
                                 typeof( T ) );
            }
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
                m_subscriberDictionary[typeof( T )] =
                    Delegate.RemoveAll( m_subscriberDictionary[typeof( T )], methodTarget );
            }
        }

    }

}
