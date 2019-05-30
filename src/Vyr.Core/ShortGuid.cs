using System;

namespace Vyr.Core
{
    public struct ShortGuid
    {
        public static readonly ShortGuid Empty = new ShortGuid(Guid.Empty);

        private Guid guid;
        private string value;

        public ShortGuid(string value)
        {
            this.value = value;
            this.guid = Decode(value);
        }

        public ShortGuid(Guid guid)
        {
            this.value = Encode(guid);
            this.guid = guid;
        }

        public Guid Guid
        {
            get { return this.guid; }
            set
            {
                if (value != this.guid)
                {
                    this.guid = value;
                    this.value = Encode(value);
                }
            }
        }

        public string Value
        {
            get { return this.value; }
            set
            {
                if (value != this.value)
                {
                    this.value = value;
                    this.guid = Decode(value);
                }
            }
        }

        public override string ToString()
        {
            return this.value;
        }

        public override bool Equals(object obj)
        {
            if (obj is ShortGuid)
                return this.guid.Equals(((ShortGuid)obj).guid);
            if (obj is Guid)
                return this.guid.Equals((Guid)obj);
            if (obj is string)
                return this.guid.Equals(((ShortGuid)obj).guid);
            return false;
        }

        public override int GetHashCode()
        {
            return this.guid.GetHashCode();
        }

        public static ShortGuid NewGuid()
        {
            return new ShortGuid(Guid.NewGuid());
        }

        public static string Encode(string value)
        {
            var guid = new Guid(value);
            return Encode(guid);
        }

        public static string Encode(Guid guid)
        {
            string encoded = Convert.ToBase64String(guid.ToByteArray());
            encoded = encoded
                .Replace("/", "_")
                .Replace("+", "-");
            return encoded.Substring(0, 22);
        }

        public static Guid Decode(string value)
        {
            value = value
                .Replace("_", "/")
                .Replace("-", "+");
            byte[] buffer = Convert.FromBase64String(value + "==");
            return new Guid(buffer);
        }

        public static bool operator ==(ShortGuid x, ShortGuid y)
        {
            if ((object)x == null) return (object)y == null;
            return x.guid == y.guid;
        }

        public static bool operator !=(ShortGuid x, ShortGuid y)
        {
            return !(x == y);
        }

        public static implicit operator string(ShortGuid shortGuid)
        {
            return shortGuid.value;
        }

        public static implicit operator Guid(ShortGuid shortGuid)
        {
            return shortGuid.guid;
        }

        public static implicit operator ShortGuid(string shortGuid)
        {
            return new ShortGuid(shortGuid);
        }

        public static implicit operator ShortGuid(Guid guid)
        {
            return new ShortGuid(guid);
        }
    }
}
