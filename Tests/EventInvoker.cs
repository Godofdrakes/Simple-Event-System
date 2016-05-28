using System;
using SimpleEventSystems;


namespace Tests {

    public class TestSubscriber {

        private IEventSystem m_eventSystem;

        public TestSubscriber( IEventSystem eventSystem ) {
            m_eventSystem = eventSystem;
            EventWasHandled = false;
        }

        public bool EventWasHandled { get; private set; }

        public void Subscribe() { m_eventSystem.Subscribe<TestEvent>( EventHandler ); }

        public void Unsubscribe() { m_eventSystem.Unsubscribe<TestEvent>( EventHandler ); }

        private void EventHandler( TestEvent testEvent ) { EventWasHandled = true; }

    }

}
