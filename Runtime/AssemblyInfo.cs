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

        public const string MENU = "Cursed Broom/";
        public const string MENU_EFFECT = "Cursed Broom/Effect/";
        public const string MENU_GAMESTATE = "Cursed Broom/Game State/";
        public const string MENU_LEVEL = "Cursed Broom/Level/";
        public const string MENU_LEVEL_TILE = "2D/Tiles/";
        public const string MENU_AVATAR = "Cursed Broom/Avatar/";
        public const string MENU_AVATAR_MOVEMENT = "Cursed Broom/Avatar/Movement/";
        public const string MENU_STORY = "Cursed Broom/Story/";
        public const string MENU_INPUT = "Cursed Broom/Input/";
        public const string MENU_EVENTS = "Cursed Broom/Events/";
    }
}