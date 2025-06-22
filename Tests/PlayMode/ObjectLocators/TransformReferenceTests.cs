using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Slothsoft.Events.ObjectLocators;

namespace Slothsoft.Events.Tests.PlayMode.ObjectLocators {
    [TestFixture(TestOf = typeof(TransformReference))]
    sealed class TransformReferenceTests {
        [Test]
        public void TestLocatorIsUsed() {
            var stub = Substitute.For<ITransformLocator>();

            var sut = new TransformReference() {
                locator = stub,
            };

            sut.TryGetTransform(null, out var _);

            stub.Received(1).TryLocate(null, out _);
        }
    }
}
