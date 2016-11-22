namespace BotTests {

    using BotTests.Mocks;
    using System.Linq;
    using Xunit;

    public class ModuleTests {

        #region Private Fields + Properties

        MockBot bot;

        #endregion Private Fields + Properties

        #region Public Constructors

        public ModuleTests() {
            bot = new MockBot();
            bot.Run();
        }

        #endregion Public Constructors

        #region Public Methods

        [Fact]
        public void CheckModuleCount() {
            Assert.True(bot.Service.Modules.Count() == 4);
        }

        #endregion Public Methods
    }
}