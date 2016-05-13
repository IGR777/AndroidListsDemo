using System;
using System.Collections.Generic;
using Android.Content;
using Android.Widget;
using System.Collections;
using System.Linq;
using Android.Views;

namespace TestLec5
{
	public class StableArrayAdapter : AnimalAdapter
	{
		public StableArrayAdapter (List<Animal> objects) : base (objects)
		{
		}

		public override long GetItemId (int position)
		{
			if (position < 0 || position >= _animals.Count) {
				return -1;
			}
			return _animals[position].GetHashCode ();
		}

		public override bool HasStableIds {
			get {
				return true;
			}
		}

		public void SwapElements (int indexOne, int indexTwo)
		{
			var tmp = _animals [indexOne];
			_animals [indexOne] = _animals [indexTwo];
			_animals [indexTwo] = tmp;
			Console.WriteLine ("Elements swapped {0}, {1}", indexOne, indexTwo);
		}
	}
}

