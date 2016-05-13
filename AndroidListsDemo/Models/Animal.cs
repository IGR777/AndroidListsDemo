using System;

namespace TestLec5
{
	public class Animal
	{
		public string Name { get; set; }

		public string Color { get; set; }

		public double Weight { get; set; }

		public override string ToString ()
		{
			return Name;
		}

		public override int GetHashCode ()
		{
			return Name.GetHashCode () + Color.GetHashCode () + Weight.GetHashCode ();
		}
	}
}

