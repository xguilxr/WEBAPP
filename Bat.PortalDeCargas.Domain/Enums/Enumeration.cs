using System;
using System.Linq;
using System.Reflection;

namespace Bat.PortalDeCargas.Domain.Enums
{
    public abstract class Enumeration<T> : IEquatable<Enumeration<T>>, IComparable<Enumeration<T>>
        where T : Enumeration<T>
    {
        protected Enumeration(int value, string name)
        {
            Value = value;
            Name = name;
        }

        public string Name { get; }
        public int Value { get; }

        public int CompareTo(Enumeration<T> other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (ReferenceEquals(null, other))
            {
                return 1;
            }

            return Value.CompareTo(other.Value);
        }

        public bool Equals(Enumeration<T> other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((Enumeration<T>)obj);
        }

        public override int GetHashCode() => Value;

        public static implicit operator int(Enumeration<T> val)
        {
            if (val == null)
            {
                throw new InvalidCastException("Argument cannot be null");
            }

            return val.Value;
        }

        public static implicit operator string(Enumeration<T> val)
        {
            if (val == null)
            {
                throw new InvalidCastException("Argument cannot be null");
            }

            return val.Name;
        }

        public static implicit operator Enumeration<T>(int val)
        {
            var arr = GetAll();

            return arr.FirstOrDefault(e => e.Value == val);
        }

        public static implicit operator Enumeration<T>(string val)
        {
            return GetAll().FirstOrDefault(e => string.Equals(e.Name, val, StringComparison.Ordinal));
        }

        public override string ToString() => Name;

        private static T[] GetAll()
        {
            var enumerationType = typeof(T);

            return enumerationType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(info => enumerationType.IsAssignableFrom(info.FieldType)).Select(info => info.GetValue(null))
                .Cast<T>().ToArray();
        }
    }
}
