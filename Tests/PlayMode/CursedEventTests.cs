using System;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Slothsoft.TestRunner;
using UnityEditor;
using UnityEngine;

namespace Slothsoft.Effects.Tests.PlayMode {
    [TestFixture(TestOf = typeof(EffectEvent))]
    sealed class CursedEventTests {
        sealed class CursedEventBridge : MonoBehaviour {
            [SerializeField]
            public EffectEvent onTrigger = new();
        }
        [Serializable]
        sealed class CursedActionStub : IEffect {
            internal event Action onGlobal;
            internal event Action<GameObject> onGameObject;
            internal event Action<CollisionInfo> onCollision;

            public void Invoke() => onGlobal?.Invoke();
            public void Invoke(GameObject context) => onGameObject?.Invoke(context);
            public void Invoke(CollisionInfo collision) => onCollision?.Invoke(collision);
        }
        public enum EventType {
            Global,
            GameObject,
            Collision
        }
        public static EventType[] eventTypes = Enum
            .GetValues(typeof(EventType))
            .Cast<EventType>()
            .ToArray();

        [TestCase(0, false)]
        [TestCase(1, true)]
        public void GivenActions_WhenCallHasPersistentListeners_ThenReturn(int count, bool expected) {
            var sut = new EffectEvent {
                effects = new IEffect[count]
            };

            Assert.That(sut.hasPersistentListeners, Is.EqualTo(expected));
        }

        [Test]
        public void TestCursedAction([ValueSource(nameof(eventTypes))] EventType triggerType) {
            using var test = new TestGameObject<CursedEventBridge>();
            var material = new CollisionMaterial();
            var collision = new CollisionInfo(new(test.gameObject, material), Vector3.one, 1);

            var substitute = new CursedActionStub();

            var obj = new SerializedObject(test.sut);
            var property = obj.FindProperty(nameof(CursedEventBridge.onTrigger));
            Assert.IsNotNull(property, "failed to find CursedEvent");

            var actions = property.FindPropertyRelative("actions");
            Assert.IsNotNull(actions, "failed to find actions");

            actions.InsertArrayElementAtIndex(0);
            var action = actions.GetArrayElementAtIndex(0);
            action.managedReferenceValue = substitute;
            obj.ApplyModifiedPropertiesWithoutUndo();

            bool wasCalled = false;
            switch (triggerType) {
                case EventType.Global:
                    substitute.onGlobal += () => {
                        wasCalled = true;
                    };
                    break;
                case EventType.GameObject:
                    substitute.onGameObject += obj => {
                        wasCalled = true;
                        Assert.AreEqual(test.gameObject, obj);
                    };
                    break;
                case EventType.Collision:
                    substitute.onCollision += collision => {
                        wasCalled = true;
                        Assert.AreEqual(collision, collision);
                    };
                    break;
            }

            switch (triggerType) {
                case EventType.Global:
                    test.sut.onTrigger.Invoke();
                    break;
                case EventType.GameObject:
                    test.sut.onTrigger.Invoke(test.gameObject);
                    break;
                case EventType.Collision:
                    test.sut.onTrigger.Invoke(collision);
                    break;
            }

            Assert.IsTrue(wasCalled);
        }

        [Test]
        public void TestAddListener([ValueSource(nameof(eventTypes))] EventType actionType, [ValueSource(nameof(eventTypes))] EventType triggerType) {
            using var test = new TestGameObject();
            var material = new CollisionMaterial();
            var collision = new CollisionInfo(new(test.gameObject, material), Vector3.one, 1);

            object action = actionType switch {
                EventType.Global => Substitute.For<Action>(),
                EventType.GameObject => Substitute.For<Action<GameObject>>(),
                EventType.Collision => Substitute.For<Action<CollisionInfo>>(),
                _ => throw new NotImplementedException(actionType.ToString()),
            };

            var sut = new EffectEvent();

            switch (actionType) {
                case EventType.Global:
                    sut.AddListener(action as Action);
                    break;
                case EventType.GameObject:
                    sut.AddListener(action as Action<GameObject>);
                    break;
                case EventType.Collision:
                    sut.AddListener(action as Action<CollisionInfo>);
                    break;
            }


            switch (triggerType) {
                case EventType.Global:
                    sut.Invoke();
                    break;
                case EventType.GameObject:
                    sut.Invoke(test.gameObject);
                    break;
                case EventType.Collision:
                    sut.Invoke(collision);
                    break;
            }

            switch (actionType) {
                case EventType.Global:
                    (action as Action).Received(1).Invoke();
                    break;
                case EventType.GameObject:
                    var expectedGameObject = triggerType == EventType.Global
                        ? default
                        : test.gameObject;
                    (action as Action<GameObject>).Received(1).Invoke(expectedGameObject);
                    break;
                case EventType.Collision:
                    var expectedCollision = triggerType switch {
                        EventType.Global => CollisionInfo.empty,
                        EventType.GameObject => new CollisionInfo(test.gameObject),
                        EventType.Collision => collision,
                        _ => throw new NotImplementedException(),
                    };
                    (action as Action<CollisionInfo>).Received(1).Invoke(expectedCollision);
                    break;
            }
        }

        [Test]
        public void TestRemoveActionListener([ValueSource(nameof(eventTypes))] EventType actionType, [ValueSource(nameof(eventTypes))] EventType triggerType) {
            using var test = new TestGameObject();
            var material = new CollisionMaterial();
            var collision = new CollisionInfo(new(test.gameObject, material), Vector3.one, 1);

            object action = actionType switch {
                EventType.Global => Substitute.For<Action>(),
                EventType.GameObject => Substitute.For<Action<GameObject>>(),
                EventType.Collision => Substitute.For<Action<CollisionInfo>>(),
                _ => throw new NotImplementedException(actionType.ToString()),
            };

            var sut = new EffectEvent();

            switch (actionType) {
                case EventType.Global:
                    sut.AddListener(action as Action);
                    sut.RemoveListener(action as Action);
                    break;
                case EventType.GameObject:
                    sut.AddListener(action as Action<GameObject>);
                    sut.RemoveListener(action as Action<GameObject>);
                    break;
                case EventType.Collision:
                    sut.AddListener(action as Action<CollisionInfo>);
                    sut.RemoveListener(action as Action<CollisionInfo>);
                    break;
            }

            switch (triggerType) {
                case EventType.Global:
                    sut.Invoke();
                    break;
                case EventType.GameObject:
                    sut.Invoke(test.gameObject);
                    break;
                case EventType.Collision:
                    sut.Invoke(collision);
                    break;
            }

            switch (actionType) {
                case EventType.Global:
                    (action as Action).DidNotReceiveWithAnyArgs().Invoke();
                    break;
                case EventType.GameObject:
                    (action as Action<GameObject>).DidNotReceiveWithAnyArgs().Invoke(default);
                    break;
                case EventType.Collision:
                    (action as Action<CollisionInfo>).DidNotReceiveWithAnyArgs().Invoke(default);
                    break;
            }
        }
    }
}
