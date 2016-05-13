using System;
namespace TestLec5
{
	public class Number
	{
		public int Value { get; set; }

		public override string ToString ()
		{
			return Value.ToString();
		}

		public override int GetHashCode ()
		{
			return Value.GetHashCode ();
		}
	}
}

