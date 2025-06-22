using NUnit.Framework;
using NUnit.Framework.Internal;
using Slothsoft.Effects.ObjectLocators;
using Slothsoft.TestRunner;

namespace Slothsoft.Effects.Tests.PlayMode.ObjectLocators {
    [TestFixture(TestOf = typeof(LocateFromContext))]
    sealed class LocateFromContextTests {
        [TestCase(false)]
        [TestCase(true)]
        public void TestLocate(bool expectedResult) {
            using var test = new TestGameObject();

            var context = expectedResult
                ? test.gameObject
                : null;

            var sut = new LocateFromContext();

            bool actualResult = sut.TryLocate(context, out var actual);

            if (expectedResult) {
                Assert.IsTrue(actualResult);
                Assert.AreEqual(test.gameObject.transform, actual);
            } else {
                Assert.IsFalse(actualResult);
            }
        }
    }
}
