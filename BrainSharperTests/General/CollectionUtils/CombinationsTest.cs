﻿namespace BrainSharperTests.General.CollectionUtils
{
    using System.Collections.Generic;

    using NUnit.Framework;
    using BrainSharper.General.Utils;

    [TestFixture]
    public class CombinationsTest
    {
        [Test]
        public void TestCombinationsOfSimpleArray()
        {
            // Given
            var collection = new[] { "A", "B", "C", "D" };
            var expectedSubsets = new List<IList<string>>
                                      {
                                          new [] { "A" },
                                          new [] { "B" },
                                          new [] { "A", "B" },
                                          new [] { "C" },
                                          new [] { "A", "C" },
                                          new [] { "B", "C" },
                                          new [] { "A", "B", "C" },
                                          new [] { "D" },
                                          new [] { "A", "D" },
                                          new [] { "B", "D" },
                                          new [] { "C", "D" },
                                          new [] { "A", "B", "D" },
                                          new [] { "A", "C", "D" },
                                          new [] { "B", "C", "D" },
                                          new [] { "A", "B", "C", "D" }
                                      };

            // When
            var combinations = collection.GetAllCombinations();

            // Then
            Assert.AreEqual(15, combinations.Count);
        }
    }
}
