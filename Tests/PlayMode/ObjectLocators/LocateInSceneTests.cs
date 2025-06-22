using NUnit.Framework;
using Slothsoft.Events.ObjectLocators;
using Slothsoft.TestRunner;

namespace Slothsoft.Events.Tests.PlayMode.ObjectLocators {
    [TestFixture(TestOf = typeof(LocateInScene))]
    sealed class LocateInSceneTests {
        [TestCase(false)]
        [TestCase(true)]
        public void TestLocate(bool expectedResult) {
            using var test = new TestGameObject();

            var expected = expectedResult
                ? test.gameObject.transform
                : null;

            var sut = new LocateInScene() {
                transform = expected
            };

            bool actualResult = sut.TryLocate(test.gameObject, out var actual);

            if (expectedResult) {
                Assert.IsTrue(actualResult);
                Assert.AreEqual(expected, actual);
            } else {
                Assert.IsFalse(actualResult);
            }
        }
    }
}
