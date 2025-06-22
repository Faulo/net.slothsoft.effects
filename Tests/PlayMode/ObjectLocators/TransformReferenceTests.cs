using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Slothsoft.Effects.ObjectLocators;

namespace Slothsoft.Effects.Tests.PlayMode.ObjectLocators {
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
