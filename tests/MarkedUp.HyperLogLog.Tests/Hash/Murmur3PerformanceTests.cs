using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
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
        /// and ensure that a minimal number of hashes collide.
        /// </summary>
        [Test]
        public void Should_have_low_number_of_Guid_collisions()
        {
            var targetItems = 1000000; //10 million guids
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
            var epsilon = (double) (targetItems - hashes.Count)/targetItems;
            Assert.IsTrue(epsilon <= 0.00125d, string.Format("Larger number of hash collisions ({0}%) than acceptable.", epsilon * 100));
            Assert.Pass("Test completed in {0} seconds with {1}% collisions", timespan.TotalSeconds, epsilon*100);
        }

        /// <summary>
        /// We're going to test the performance of the hashing algorithm itself using its <see cref="Murmur3.HashBytes"/>
        /// method with a 4-byte payload (best case performance) and compare its performance to MD5.
        /// </summary>
        [Test]
        public void Should_outperform_MD5_best_case_performance()
        {
            //going to use a reasonably large string, because it forces additional cleanup work to occur in the tail
            var testBytes = Encoding.Unicode.GetBytes("this");
            var targetItems = 1000000; //1 million hash attempts
            var i = targetItems;
            var start = DateTime.UtcNow.Ticks;
            while (i > 0)
            {
                Murmur3.Hash(testBytes);
                i--;
            }
            var end = DateTime.UtcNow.Ticks;
            var murmur3Elapsed = new TimeSpan(end - start);

            start = DateTime.UtcNow.Ticks;
            var m5 = MD5.Create();
            i = targetItems;
            while (i > 0)
            {
                m5.ComputeHash(testBytes);
                i--;
            }
            end = DateTime.UtcNow.Ticks;
            var md5Elapsed = new TimeSpan(end - start);
            Assert.IsTrue(murmur3Elapsed.Ticks < md5Elapsed.Ticks, "Expected Murmur3 to be faster than MD5");
            Assert.Pass("Murmur3 completed in {0} seconds; MD5 in {1} seconds", murmur3Elapsed.TotalSeconds, md5Elapsed.TotalSeconds);
        }

        /// <summary>
        /// We're going to test the performance of the hashing algorithm itself using its <see cref="Murmur3.HashBytes"/>
        /// method with a real-world string payload (average case performance) and compare its performance to MD5.
        /// </summary>
        [Test]
        public void Should_outperform_MD5_average_case_performance()
        {
            //going to use a reasonably large string, because it forces additional cleanup work to occur in the tail
            var testBytes = Encoding.Unicode.GetBytes("firstname.lastname@emaildomain.com");
            var targetItems = 1000000; //1 million hash attempts
            var i = targetItems;
            var start = DateTime.UtcNow.Ticks;
            while (i > 0)
            {
                Murmur3.Hash(testBytes);
                i--;
            }
            var end = DateTime.UtcNow.Ticks;
            var murmur3Elapsed = new TimeSpan(end - start);

            start = DateTime.UtcNow.Ticks;
            var m5 = MD5.Create();
            i = targetItems;
            while (i > 0)
            {
                m5.ComputeHash(testBytes);
                i--;
            }
            end = DateTime.UtcNow.Ticks;
            var md5Elapsed = new TimeSpan(end - start);
            Assert.IsTrue(murmur3Elapsed.Ticks < md5Elapsed.Ticks, "Expected Murmur3 to be faster than MD5");
            Assert.Pass("Murmur3 completed in {0} seconds; MD5 in {1} seconds", murmur3Elapsed.TotalSeconds, md5Elapsed.TotalSeconds);
        }
    }
}
