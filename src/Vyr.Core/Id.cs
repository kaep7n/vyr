using System;

namespace Vyr.Core
{
    public class Id
    {
        public static readonly Id Empty = new Id(Guid.Empty);

        private Guid guid;
        private string value;

        public Id()
            : this(Guid.NewGuid())
        {

        }

        public Id(string value)
        {
            this.value = value;
            this.guid = Decode(value);
        }

        public Id(Guid guid)
        {
            this.value = Encode(guid);
            this.guid = guid;
        }

        public Guid Guid => this.guid;

        public string Value => this.value;

        private static string Encode(Guid guid)
        {
            return Convert.ToBase64String(guid.ToByteArray());
        }

        private static Guid Decode(string value)
        {
            var buffer = Convert.FromBase64String(value + "==");

            return new Guid(buffer);
        }

        public override string ToString()
        {
            return this.value;
        }

        public override bool Equals(object obj)
        {
            if (obj is Id)
            {
                return this.guid.Equals(((Id)obj).guid);
            }

            if (obj is Guid)
            {
                return this.guid.Equals((Guid)obj);
            }

            return obj is string ? this.guid.Equals(((Id)obj).guid) : false;
        }

        public override int GetHashCode()
        {
            return this.guid.GetHashCode();
        }

        public static bool operator ==(Id x, Id y)
        {
            return x == null ? y == null : x.guid == y.guid;
        }

        public static bool operator !=(Id x, Id y)
        {
            return !(x == y);
        }

        public static implicit operator string(Id shortGuid)
        {
            return shortGuid.value;
        }

        public static implicit operator Guid(Id shortGuid)
        {
            return shortGuid.guid;
        }

        public static implicit operator Id(string shortGuid)
        {
            return new Id(shortGuid);
        }

        public static implicit operator Id(Guid guid)
        {
            return new Id(guid);
        }
    }
}
