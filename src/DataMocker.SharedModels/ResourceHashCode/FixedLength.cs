using System;
namespace DataMocker.SharedModels
{
    internal class FixedLength : IHashCode
    {
        private readonly IHashCode _origin;
        private readonly int _length;

        public FixedLength(IHashCode origin)
            : this(origin, 8)
        {
        }

        public FixedLength(IHashCode origin, int length)
        {
            _origin = origin;
            _length = length;
        }

        public string ToHexString()
        {
            return _origin
                .ToHexString()
                .Substring(0, _length);
        }
    }
}

