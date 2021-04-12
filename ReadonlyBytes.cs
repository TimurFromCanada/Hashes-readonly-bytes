using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using static hashes.ReadonlyBytesTests;

namespace hashes
{
	public class ReadonlyBytes : IEnumerable<byte>
	{
		internal int Length { get { return arrayBytes.Length; } }
		internal byte[] arrayBytes;
		private int hashCode = -1;

		public ReadonlyBytes(params byte[] array)
		{
			if (array == null)
				throw new ArgumentNullException();
			else
				arrayBytes = array;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is ReadonlyBytes))
				return false;

			if (obj is DerivedFromReadonlyBytes)
				return false;

			var readOnlyByte = obj as ReadonlyBytes;

			if (arrayBytes.Length != readOnlyByte.arrayBytes.Length)
				return false;

			for (var i = 0; i < readOnlyByte.Length; i++)
				if (arrayBytes[i] != readOnlyByte[i])
					return false;

			return true;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				if (hashCode != -1)
					return hashCode;

				var fnvOffSet = unchecked((int)2166136261);
				var fnvPrime = 16777619;
				hashCode = fnvOffSet;

				for (var i = 0; i < arrayBytes.Length; i++)
				{
					hashCode = hashCode ^ arrayBytes[i];
					hashCode *= fnvPrime;
				}

				return hashCode;
			}
		}

		public override string ToString()
		{
			var str = new StringBuilder();
			var textBegin = "[";
			var textEnd = "]";
			str.Append(textBegin);

			for (var i = 0; i < arrayBytes.Length; i++)
			{
				str.Append(arrayBytes[i].ToString() + ((i == arrayBytes.Length - 1) ? "" : ", "));
			}

			str.Append(textEnd);
			return str.ToString();
		}

		public byte this[int index]
		{
			get
			{
				if (index < 0 || index > arrayBytes.Length) throw new IndexOutOfRangeException();

				return arrayBytes[index];
			}
		}

		public IEnumerator<byte> GetEnumerator()
		{
			for (var i = 0; i < arrayBytes.Length; i++)
				yield return arrayBytes[i];
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}


