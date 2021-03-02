using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    /// <summary>
    /// This Tests the fibonacci formula to make sure is returning the expected score.
    /// </summary>
    public class ScoreTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void ScoreTestSimplePasses()
        {
            uint scoreValue2 = ScoreCalculator.ReturnScore(1); //test score for 1 ship;
            uint scoreValue3 = ScoreCalculator.ReturnScore(2); //test score for 2 ships;
            uint scoreValue = ScoreCalculator.ReturnScore(5); //test score for 5 ships;

            Assert.AreEqual(400, scoreValue);
            Assert.AreEqual(10, scoreValue2);
            Assert.AreEqual(40, scoreValue3);
        }


    }
}
