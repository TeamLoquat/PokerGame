
namespace Poker.Tests
{
    using Core;

    using Factories;

    using Interfaces;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests on used Factories
    /// </summary>
    [TestClass]
    public class FactoriesTests
    {
        private IHumanFactory humanFactory;
        private IBotFactory botFactory; 

        [TestInitialize]
        public void TestInitialize()
        {
            this.humanFactory = new HumanFactory();
            this.botFactory = new BotFactory();
        }

        [TestMethod]
        public void Create_FromBotFactory_ShouldReturnIBot()
        {
            var newBot = this.botFactory.Create(1);

            Assert.IsInstanceOfType(newBot, typeof(IBot), "botFactory should return an instance of type IBot");
        }

        [TestMethod]
        public void Create_FromBotFactoryWithId2_ShouldReturnIBotWithName_Bot_2()
        {
            var newBot = this.botFactory.Create(2);

            Assert.AreEqual("Bot 2", newBot.Name, "botFactory should name bots correctly(Bot + id)");
        }

        [TestMethod]
        public void Create_FromBotFactory_ShouldReturnIBotWithChips_10000()
        {
            var newBot = this.botFactory.Create(1);

            Assert.AreEqual(10000, newBot.Chips, "botFactory should give correct amount of chips to newly created bots");
        }

        [TestMethod]
        public void Create_FromHumanFactory_ShouldReturnIHuman()
        {
            var newHuman = this.humanFactory.Create();

            Assert.IsInstanceOfType(newHuman, typeof(IHuman), "humanFactory should return an instance of type IHuman");
        }

        [TestMethod]
        public void Create_FromHumanFactory_ShouldReturnIHumanWithChips_10000()
        {
            var newHuman = this.humanFactory.Create();

            Assert.AreEqual(10000, newHuman.Chips, "humanFactory should give correct amount of chips to newly created humans");
        }

        [TestMethod]
        public void Create_FromBotFactory_ShouldReturnBotWithTurn_False()
        {
            var newBot = this.botFactory.Create(1);

            Assert.IsFalse(newBot.Turn, "Bot should have turn = false on itialization");
        }

        [TestMethod]
        public void Create_FromHumanFactory_ShouldReturnHumanWithTurn_True()
        {
            var newHuman = this.botFactory.Create(1);

            Assert.IsTrue(newHuman.Turn, "Human should have turn = true on itialization");
        }
    }

    /// Tests on Database used
    /// </summary>
    [TestClass]
    public class DatabaseTests
    {
        private IDatabase database;

        [TestInitialize]
        public void TestInitialize()
        {
            var botFactory = new BotFactory();
            var humanFactory = new HumanFactory();

            this.database = new Database(botFactory,humanFactory);
        }

        [TestMethod]
        public void Initialize_OnDatabase_ShouldInitializeListOfPlayers()
        {
            this.database.Initialize();

            Assert.IsNotNull(this.database.Players,"Database.Initialize should define list of players");
        }

        [TestMethod]
        public void Initialize_OnDatabase_ShouldInitializeDictionaryOfButtons()
        {
            this.database.Initialize();

            Assert.IsNotNull(this.database.Buttons, "Database.Initialize should define dict of buttons");
        }
    }
}
