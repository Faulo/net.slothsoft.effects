using System;
using NUnit.Framework;
using Slothsoft.TestRunner;
using UnityEditor;
using UnityEngine;

namespace Slothsoft.Effects.Editor.Tests {
    [TestFixture(TestOf = typeof(SerializedPropertyArraySource))]
    sealed class SerializedPropertyArraySourceTests {
        sealed class StubObject : ScriptableObject {
            [SerializeReference]
            public EffectEvent[] events = Array.Empty<EffectEvent>();
        }

        readonly TestObjectStore store = new();
        StubObject stub;
        SerializedObject serialized;
        SerializedProperty property;
        SerializedPropertyArraySource sut;

        [SetUp]
        public void SetUpSuT() {
            stub = store.CreateScriptableObject<StubObject>();
            stub.events = new EffectEvent[] { new(), new(), new() };
            serialized = new(stub);
            property = serialized.FindProperty(nameof(StubObject.events));
            sut = new(property);
        }

        [TearDown]
        public void TearDownSuT() {
            property.Dispose();
            serialized.Dispose();
            store.Dispose();
        }

        [TestCase(0)]
        [TestCase(1)]
        public void WhenGet_ThenReturnProperty(int index) {
            Assert.That(sut[index], Is.InstanceOf<SerializedProperty>().With.Property(nameof(SerializedProperty.managedReferenceValue)).EqualTo(stub.events[index]));
        }

        [TestCase(0)]
        [TestCase(1)]
        public void WhenGet_ThenReturnDifferentEachTime(int index) {
            Assert.That(sut[index], Is.Not.SameAs(sut[index]));
        }

        [TestCase(0, 1)]
        [TestCase(1, 0)]
        public void WhenSwapViaMoveArrayElement_ThenSwap(int source, int destination) {
            var a = stub.events[source];
            var b = stub.events[destination];

            property.MoveArrayElement(source, destination);
            serialized.ApplyModifiedProperties();

            Assert.That(stub.events[source], Is.SameAs(b));
            Assert.That(stub.events[destination], Is.SameAs(a));
        }

        [TestCase(0, 1)]
        [TestCase(1, 0)]
        public void WhenSwapViaSetter_ThenSwap(int source, int destination) {
            var a = stub.events[source];
            var b = stub.events[destination];

            object aProp = sut[source];
            object bProp = sut[destination];

            sut[destination] = aProp;
            sut[source] = bProp;

            sut.ApplyBuffer();

            Assert.That(stub.events[source], Is.SameAs(b));
            Assert.That(stub.events[destination], Is.SameAs(a));
        }
    }
}
