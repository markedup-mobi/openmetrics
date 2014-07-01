using System;
using System.Collections.Generic;
using MarkedUp.HyperLogLog.Hash;
using NUnit.Framework;

namespace MarkedUp.HyperLogLog.Tests.Hash
{
    /// <summary>
    /// Tests designed to ensure that Murmur3's hash values are consistent
    /// </summary>
    [TestFixture]
    public class Murmur3PerformanceTests
    {
        /// <summary>
        /// Each <see cref="Guid"/>'s value is unique and the likelihood of a collision
        /// should be infinitestimally small, so we're going to hash several million Guids
        /// and ensure that none of the hash values collide.
        /// </summary>
        [Test]
        public void Should_have_zero_Guid_collisions()
        {
            var targetItems = 10000000; //100 million guids
            var i = targetItems;
            var start = DateTime.UtcNow.Ticks;
            var hashes = new HashSet<int>();
            while (i > 0)
            {
                var hash = Murmur3.Hash(Guid.NewGuid());
                hashes.Add(hash);
                i--;
            }
            var end = DateTime.UtcNow.Ticks;
            var timespan = new TimeSpan(end - start);
            Assert.AreEqual(targetItems, hashes.Count);
            Assert.Pass("Test completed in {0} seconds", timespan.TotalSeconds);
        }
    }
}
