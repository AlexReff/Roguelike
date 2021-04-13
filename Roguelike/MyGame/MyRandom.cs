using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelike
{
    internal class MyRandom : Troschuetz.Random.Generators.AbstractGenerator
    {
        private Random _rand;
        private readonly byte[] _uintBuffer;

        public MyRandom() : this(Guid.NewGuid().GetHashCode()) { }
        public MyRandom(int seed) : base((uint)seed)
        {
            _rand = new Random(seed);
            _uintBuffer = new byte[4];
        }

        public MyRandom(uint seed) : base(seed)
        {
            _rand = new Random((int)seed);
            _uintBuffer = new byte[4];
        }

        public override int NextInclusiveMaxValue() => _rand.Next();

        public override double NextDouble() => _rand.NextDouble();

        public override bool Reset(uint seed)
        {
            return Reset((int)seed);
        }

        public bool Reset(int seed)
        {
            _rand = new Random(seed);
            return true;
        }

        //public new int Next(int minValue, int maxValue) => _rand.Next(minValue, maxValue);

        public override uint NextUIntInclusiveMaxValue()
        {
            _rand.NextBytes(_uintBuffer);
            return BitConverter.ToUInt32(_uintBuffer, 0);
        }

        public override bool CanReset => false;
    }
}
