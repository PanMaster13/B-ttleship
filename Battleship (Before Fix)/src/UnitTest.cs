using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Battleship.src
{
    [TestFixture()]
    class UnitTest
    {
        [Test()]
        public void TestCase()
        {
            GameController.StartGame();
            GameController.SetDifficulty(AIOption.Easy);
            AIOption expected = AIOption.Easy;
            AIOption actual = GameController._aiSetting;
            Assert.AreEqual(expected, actual);
        }
    }
}
