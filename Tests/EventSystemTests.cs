using System;
using NUnit.Framework;
using SimpleEventSystems;


namespace Tests {

    [ TestFixture ]
    public class EventSystemTests {

        [ Test ]
        public void BasicExample() {
            EventSystem eventSystem = new EventSystem();
            TestSubscriber subscriber = new TestSubscriber( eventSystem );

            // Sub
            subscriber.Subscribe();

            // Call
            eventSystem.InvokeEvent( new TestEvent() );

            // Unsub
            subscriber.Unsubscribe();

            Assert.True( subscriber.EventWasHandled );
        }

        [ Test ]
        public void EmptySubscribers() {
            EventSystem eventSystem = new EventSystem();

            eventSystem.InvokeEvent( new TestEvent() );

            Assert.Pass( "No exceptions were encountered.\n" );
        }

        [ Test ]
        [ TestCase( 5 ) ]
        [ TestCase( 50 ) ]
        [ TestCase( 500 ) ]
        public void MultipleSubscribers( int subscriberCount ) {
            EventSystem eventSystem = new EventSystem();

            TestSubscriber[] subscribers = new TestSubscriber[subscriberCount];
            for( int index = 0; index < subscriberCount; index++ ) {
                subscribers[index] = new TestSubscriber( eventSystem );
            }

            // sub
            for( int index = 0; index < subscriberCount; index++ ) {
                subscribers[index].Subscribe();
            }

            // Call
            eventSystem.InvokeEvent( new TestEvent() );

            // Unsub
            for( int index = 0; index < subscriberCount; index++ ) {
                subscribers[index].Unsubscribe();
            }

            for( int index = 0; index < subscriberCount; index++ ) {
                Assert.True( subscribers[index].EventWasHandled );
            }
        }

    }

}
