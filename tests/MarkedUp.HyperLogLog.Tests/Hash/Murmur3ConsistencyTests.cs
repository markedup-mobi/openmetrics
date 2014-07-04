using System.Runtime.InteropServices;
using System.Text;
using MarkedUp.HyperLogLog.Hash;
using NUnit.Framework;

namespace MarkedUp.HyperLogLog.Tests.Hash
{
    /// <summary>
    /// Tests to ensure that <see cref="Murmur3"/> produces consistent results when working with
    /// data that is value-equivalent (but not referentially equal)
    /// </summary>
    [TestFixture]
    public class Murmur3ConsistencyTests
    {
        [Test]
        public void Should_compute_equivalent_hash_value_for_byte_array()
        {
            var str1 = Encoding.UTF8.GetBytes("this is a string");
            var str2 = Encoding.UTF8.GetBytes("this is a string");

            var hash1 = Murmur3.HashBytes(str1);
            var hash2 = Murmur3.HashBytes(str2);

            Assert.AreEqual(hash1, hash2);
        }

        [Test]
        public void Should_compute_equivalent_hash_value_for_strings()
        {
            var str1 = "this is a string";
            var str2 = "this is a string";

            var hash1 = Murmur3.Hash(str1);
            var hash2 = Murmur3.Hash(str2);

            Assert.AreEqual(hash1, hash2);
        }

        [Test]
        public void Should_compute_equivalent_hash_value_for_primitives()
        {
            var int1 = 1;
            var int2 = 1;

            var hash1 = Murmur3.Hash(int1);
            var hash2 = Murmur3.Hash(int2);

            Assert.AreEqual(hash1, hash2);
        }

        [Test]
        public void Should_NOT_compute_equivalent_hash_value_for_byte_equivalent_primitives()
        {
            var int1 = 1;
            var bool1 = true;

            //bitwise, these two values should be equivalent
            //HOWEVER, Murmur3 takes the byte-array length into account as part of its hash.
            //thus, the values SHOULD NOT BE EQUIVALENT
            var hash1 = Murmur3.Hash(int1);
            var hash2 = Murmur3.Hash(bool1);

            Assert.AreNotEqual(hash1, hash2);
        }

        class TestPoco
        {
            public string Name { get; set; }

            public int Value { get; set; }

            public override string ToString()
            {
                return string.Format("{0}:{1}", Name, Value);
            }
        }

        [Test]
        public void Should_compute_equivalent_hash_values_for_equivalent_POCOs()
        {
            var poco1 = new TestPoco() {Name = "Aaron", Value = 1337};
            var poco2 = new TestPoco() { Name = "Aaron", Value = 1337 };

            var hash1 = Murmur3.Hash(poco1);
            var hash2 = Murmur3.Hash(poco2);

            Assert.AreEqual(hash1, hash2);
        }
    }
}
