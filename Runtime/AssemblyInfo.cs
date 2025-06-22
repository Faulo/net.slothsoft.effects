using System.Runtime.CompilerServices;
using Slothsoft.Events;

[assembly: InternalsVisibleTo(AssemblyInfo.NAMESPACE_EDITOR)]
[assembly: InternalsVisibleTo(AssemblyInfo.NAMESPACE_TESTS_EDITMODE)]
[assembly: InternalsVisibleTo(AssemblyInfo.NAMESPACE_TESTS_PLAYMODE)]

namespace Slothsoft.Events {
    static class AssemblyInfo {
        public const string ID = "net.slothsoft.test-runner";

        public const string NAMESPACE_RUNTIME = "Slothsoft.Events";
        public const string NAMESPACE_EDITOR = "Slothsoft.Events.Editor";

        public const string NAMESPACE_TESTS_PLAYMODE = "Slothsoft.Events.Tests.PlayMode";
        public const string NAMESPACE_TESTS_EDITMODE = "Slothsoft.Events.Tests.EditMode";

        public const string NAMESPACE_PROXYGEN = "DynamicProxyGenAssembly2";
    }
}