using System;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Slothsoft.TestRunner;
using UnityEditor;
using UnityEngine;

namespace Slothsoft.Effects.Tests.PlayMode {
    [TestFixture(TestOf = typeof(EffectEvent))]
    sealed class EffectEventTests {
        sealed class StubTrigger : MonoBehaviour {
            [SerializeField]
            public EffectEvent onTrigger = new();
        }
        [Serializable]
        sealed class StubEffect : IEffect {
            internal event Action onGlobal;
            internal event Action<GameObject> onGameObject;
            internal event Action<CollisionInfo> onCollision;

            public void Invoke() => onGlobal?.Invoke();
            public void Invoke(GameObject context) => onGameObject?.Invoke(context);
            public void Invoke(CollisionInfo collision) => onCollision?.Invoke(collision);
        }
        public enum EEventType {
            Global,
            GameObject,
            Collision
        }
        public static EEventType[] eventTypes = Enum
            .GetValues(typeof(EEventType))
            .Cast<EEventType>()
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
        public void TestCursedAction([ValueSource(nameof(eventTypes))] EEventType triggerType) {
            using var test = new TestGameObject<StubTrigger>();
            var material = new CollisionMaterial();
            var collision = new CollisionInfo(new(test.gameObject, material), Vector3.one, 1);

            var substitute = new StubEffect();

            var obj = new SerializedObject(test.sut);
            var property = obj.FindProperty(nameof(StubTrigger.onTrigger));
            Assert.IsNotNull(property, $"Failed to find: {nameof(StubTrigger.onTrigger)}");

            var actions = property.FindPropertyRelative(nameof(EffectEvent.effects));
            Assert.IsNotNull(actions, $"Failed to find: {nameof(EffectEvent.effects)}");

            actions.InsertArrayElementAtIndex(0);
            var action = actions.GetArrayElementAtIndex(0);
            action.managedReferenceValue = substitute;
            obj.ApplyModifiedPropertiesWithoutUndo();

            bool wasCalled = false;
            switch (triggerType) {
                case EEventType.Global:
                    substitute.onGlobal += () => {
                        wasCalled = true;
                    };
                    break;
                case EEventType.GameObject:
                    substitute.onGameObject += obj => {
                        wasCalled = true;
                        Assert.AreEqual(test.gameObject, obj);
                    };
                    break;
                case EEventType.Collision:
                    substitute.onCollision += collision => {
                        wasCalled = true;
                        Assert.AreEqual(collision, collision);
                    };
                    break;
            }

            switch (triggerType) {
                case EEventType.Global:
                    test.sut.onTrigger.Invoke();
                    break;
                case EEventType.GameObject:
                    test.sut.onTrigger.Invoke(test.gameObject);
                    break;
                case EEventType.Collision:
                    test.sut.onTrigger.Invoke(collision);
                    break;
            }

            Assert.IsTrue(wasCalled);
        }

        [Test]
        public void TestAddListener([ValueSource(nameof(eventTypes))] EEventType actionType, [ValueSource(nameof(eventTypes))] EEventType triggerType) {
            using var test = new TestGameObject();
            var material = new CollisionMaterial();
            var collision = new CollisionInfo(new(test.gameObject, material), Vector3.one, 1);

            object action = actionType switch {
                EEventType.Global => Substitute.For<Action>(),
                EEventType.GameObject => Substitute.For<Action<GameObject>>(),
                EEventType.Collision => Substitute.For<Action<CollisionInfo>>(),
                _ => throw new NotImplementedException(actionType.ToString()),
            };

            var sut = new EffectEvent();

            switch (actionType) {
                case EEventType.Global:
                    sut.AddListener(action as Action);
                    break;
                case EEventType.GameObject:
                    sut.AddListener(action as Action<GameObject>);
                    break;
                case EEventType.Collision:
                    sut.AddListener(action as Action<CollisionInfo>);
                    break;
            }


            switch (triggerType) {
                case EEventType.Global:
                    sut.Invoke();
                    break;
                case EEventType.GameObject:
                    sut.Invoke(test.gameObject);
                    break;
                case EEventType.Collision:
                    sut.Invoke(collision);
                    break;
            }

            switch (actionType) {
                case EEventType.Global:
                    (action as Action).Received(1).Invoke();
                    break;
                case EEventType.GameObject:
                    var expectedGameObject = triggerType == EEventType.Global
                        ? default
                        : test.gameObject;
                    (action as Action<GameObject>).Received(1).Invoke(expectedGameObject);
                    break;
                case EEventType.Collision:
                    var expectedCollision = triggerType switch {
                        EEventType.Global => CollisionInfo.empty,
                        EEventType.GameObject => new CollisionInfo(test.gameObject),
                        EEventType.Collision => collision,
                        _ => throw new NotImplementedException(),
                    };
                    (action as Action<CollisionInfo>).Received(1).Invoke(expectedCollision);
                    break;
            }
        }

        [Test]
        public void TestRemoveActionListener([ValueSource(nameof(eventTypes))] EEventType actionType, [ValueSource(nameof(eventTypes))] EEventType triggerType) {
            using var test = new TestGameObject();
            var material = new CollisionMaterial();
            var collision = new CollisionInfo(new(test.gameObject, material), Vector3.one, 1);

            object action = actionType switch {
                EEventType.Global => Substitute.For<Action>(),
                EEventType.GameObject => Substitute.For<Action<GameObject>>(),
                EEventType.Collision => Substitute.For<Action<CollisionInfo>>(),
                _ => throw new NotImplementedException(actionType.ToString()),
            };

            var sut = new EffectEvent();

            switch (actionType) {
                case EEventType.Global:
                    sut.AddListener(action as Action);
                    sut.RemoveListener(action as Action);
                    break;
                case EEventType.GameObject:
                    sut.AddListener(action as Action<GameObject>);
                    sut.RemoveListener(action as Action<GameObject>);
                    break;
                case EEventType.Collision:
                    sut.AddListener(action as Action<CollisionInfo>);
                    sut.RemoveListener(action as Action<CollisionInfo>);
                    break;
            }

            switch (triggerType) {
                case EEventType.Global:
                    sut.Invoke();
                    break;
                case EEventType.GameObject:
                    sut.Invoke(test.gameObject);
                    break;
                case EEventType.Collision:
                    sut.Invoke(collision);
                    break;
            }

            switch (actionType) {
                case EEventType.Global:
                    (action as Action).DidNotReceiveWithAnyArgs().Invoke();
                    break;
                case EEventType.GameObject:
                    (action as Action<GameObject>).DidNotReceiveWithAnyArgs().Invoke(default);
                    break;
                case EEventType.Collision:
                    (action as Action<CollisionInfo>).DidNotReceiveWithAnyArgs().Invoke(default);
                    break;
            }
        }
    }
}
