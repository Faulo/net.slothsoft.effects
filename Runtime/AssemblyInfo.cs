using System.Runtime.CompilerServices;
using Slothsoft.Effects;

[assembly: InternalsVisibleTo(AssemblyInfo.NAMESPACE_EDITOR)]
[assembly: InternalsVisibleTo(AssemblyInfo.NAMESPACE_TESTS_EDITMODE)]
[assembly: InternalsVisibleTo(AssemblyInfo.NAMESPACE_TESTS_PLAYMODE)]

namespace Slothsoft.Effects {
    static class AssemblyInfo {
        public const string ID = "net.slothsoft.test-runner";

        public const string NAMESPACE_RUNTIME = "Slothsoft.Effects";
        public const string NAMESPACE_EDITOR = "Slothsoft.Effects.Editor";

        public const string NAMESPACE_TESTS_PLAYMODE = "Slothsoft.Effects.Tests.PlayMode";
        public const string NAMESPACE_TESTS_EDITMODE = "Slothsoft.Effects.Tests.EditMode";

        public const string NAMESPACE_PROXYGEN = "DynamicProxyGenAssembly2";
    }
}