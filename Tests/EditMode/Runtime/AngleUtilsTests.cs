using NUnit.Framework;
using UnityEngine;
namespace Slothsoft.Effects.Tests.EditMode {
    [TestFixture(TestOf = typeof(AngleUtils))]
    sealed class AngleUtilsTests {
        [TestCase(-720, 0)]
        [TestCase(-360, 0)]
        [TestCase(0, 0)]
        [TestCase(360, 0)]
        [TestCase(720, 0)]

        [TestCase(-540, 180)]
        [TestCase(180, 180)]
        [TestCase(540, 180)]
        public void TestNormalizeAngle(float angle, float normalizedAngle) {
            Assert.AreEqual(normalizedAngle, AngleUtils.NormalizeAngle(angle));
        }

        [TestCase(0, 1, 0)]
        [TestCase(180, 1, 0)]
        [TestCase(360, 1, 0)]

        [TestCase(45, 2, 0)]
        [TestCase(120, 2, 180)]
        [TestCase(180, 2, 180)]

        [TestCase(45, 3, 0)]
        [TestCase(120, 3, 120)]
        [TestCase(-120, 3, 240)]
        public void TestRoundAngle(float angle, int directions, float roundedAngle) {
            Assert.AreEqual(roundedAngle, AngleUtils.RoundAngle(angle, directions));
        }

        [TestCase(0, 0, 0)]
        [TestCase(0, 1, 90)]
        [TestCase(1, 0, 0)]
        [TestCase(0, -1, 270)]
        [TestCase(-1, 0, 180)]
        public void TestDirectionalAngle(float x, float y, float angle) {
            Assert.AreEqual(angle, AngleUtils.DirectionalAngle(new Vector2(x, y)), Mathf.Epsilon);
        }

        [TestCase(0, 0, 0, 0, 0)]
        public void TestDirectionalRotation(float x, float y, float angleX, float angleY, float angleZ) {
            Assert.AreEqual(Quaternion.Euler(angleX, angleY, angleZ), AngleUtils.DirectionalRotation(new Vector2(x, y)));
        }

        [TestCase(0, 0, 0)]
        [TestCase(0, 1, 0)]
        [TestCase(0, -1, 0)]

        [TestCase(1, 0, 1)]
        [TestCase(1, 1, 1)]
        [TestCase(1, -1, 1)]

        [TestCase(-1, 0, -1)]
        [TestCase(-1, 1, -1)]
        [TestCase(-1, -1, -1)]
        public void TestHorizontalSign(float x, float y, int sign) {
            Assert.AreEqual(sign, AngleUtils.HorizontalSign(new Vector2(x, y)));
        }

        [TestCase(1, 0)]
        [TestCase(-1, 180)]
        public void TestHorizontalAngle(int sign, float angle) {
            Assert.AreEqual(angle, AngleUtils.HorizontalAngle(sign));
        }

        [Test]
        public void TestHorizontalAngleThrowsException() {
            Assert.Throws<UnityEngine.Assertions.AssertionException>(() => AngleUtils.HorizontalAngle(0));
        }

        [TestCase(0, 0, 1)]
        [TestCase(0, 45, 0.707f)]
        [TestCase(0, 90, 0)]
        [TestCase(0, 135, -0.707f)]
        [TestCase(0, 180, -1)]
        public void TestAlignment(float angleA, float angleB, float alignment) {
            Assert.AreEqual(alignment, AngleUtils.Alignment(Quaternion.Euler(0, 0, angleA), Quaternion.Euler(0, 0, angleB)), 0.001f);
        }
    }
}
