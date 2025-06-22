using NUnit.Framework;
using Slothsoft.TestRunner;
using UnityEngine;

namespace Slothsoft.Effects.Tests.PlayMode {
    [TestFixture(TestOf = typeof(CollisionIdentifier))]
    sealed class CollisionIdentifierTests {
        [Test]
        public void GivenSameId_WhenCompare_ThenIsEqual() {
            var sut = new CollisionIdentifier();

#pragma warning disable CS1718 // Comparison made to same variable
            Assert.That(sut == sut, Is.True);
            Assert.That(sut != sut, Is.False);
#pragma warning restore CS1718 // Comparison made to same variable
        }

        [Test]
        public void GivenEqualIds_WhenCompare_ThenIsEqual() {
            var sut1 = new CollisionIdentifier();
            var sut2 = new CollisionIdentifier();

            Assert.That(sut1 == sut2, Is.True);
            Assert.That(sut1 != sut2, Is.False);
        }

        [Test]
        public void GivenEqualGameObjects_WhenCompare_ThenIsEqual() {
            using var test = new TestGameObject();

            var sut1 = new CollisionIdentifier(test.gameObject);
            var sut2 = new CollisionIdentifier(test.gameObject);

            Assert.That(sut1 == sut2, Is.True);
            Assert.That(sut1 != sut2, Is.False);
        }

        [Test]
        public void GivenEqualMaterials_WhenCompare_ThenIsEqual() {
            using var test = new TestGameObject();
            var material = new CollisionMaterial();

            var sut1 = new CollisionIdentifier(test.gameObject, material);
            var sut2 = new CollisionIdentifier(test.gameObject, material);

            Assert.That(sut1 == sut2, Is.True);
            Assert.That(sut1 != sut2, Is.False);
        }

        [Test]
        public void GivenEqualNormals_WhenCompare_ThenIsEqual() {
            using var test = new TestGameObject();
            var material = new CollisionMaterial();
            var normal = Vector2Int.up;

            var sut1 = new CollisionIdentifier(test.gameObject, material, normal);
            var sut2 = new CollisionIdentifier(test.gameObject, material, normal);

            Assert.That(sut1 == sut2, Is.True);
            Assert.That(sut1 != sut2, Is.False);
        }

        [Test]
        public void GivenDifferentNormals_WhenCompare_ThenIsNotEqual() {
            using var test = new TestGameObject();
            var material = new CollisionMaterial();
            var normal1 = Vector2Int.up;
            var normal2 = Vector2Int.right;

            var sut1 = new CollisionIdentifier(test.gameObject, material, normal1);
            var sut2 = new CollisionIdentifier(test.gameObject, material, normal2);

            Assert.That(sut1 == sut2, Is.False);
            Assert.That(sut1 != sut2, Is.True);
        }

        [Test]
        public void GivenDifferentMaterials_WhenCompare_ThenIsNotEqual() {
            using var test = new TestGameObject();
            var material1 = new CollisionMaterial();
            var material2 = new CollisionMaterial();

            var sut1 = new CollisionIdentifier(test.gameObject, material1);
            var sut2 = new CollisionIdentifier(test.gameObject, material2);

            Assert.That(sut1 == sut2, Is.False);
            Assert.That(sut1 != sut2, Is.True);
        }

        [Test]
        public void GivenDifferentGameObjects_WhenCompare_ThenIsNotEqual() {
            using var test1 = new TestGameObject();
            using var test2 = new TestGameObject();

            var sut1 = new CollisionIdentifier(test1.gameObject);
            var sut2 = new CollisionIdentifier(test2.gameObject);

            Assert.That(sut1 == sut2, Is.False);
            Assert.That(sut1 != sut2, Is.True);
        }
    }
}
