
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace TestLec5
{
	[Activity (Label = "SecondaryActivity"
				#region TODO11
	           //TODO 11
	           //, MainLauncher = true
				#endregion
			  )]
	public class SecondaryActivity : AppCompatActivity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Create your application here

			SetContentView (Resource.Layout.Secondary);
			var list = FindViewById<DynamicListView> (Resource.Id.myList);

			list.Adapter = new StableArrayAdapter (GetAnimals ().ToList());
		}


		IList<Animal> GetAnimals ()
		{
			var list = new List<Animal> ();
			var rand = new System.Random (DateTime.Now.Millisecond);
			for (int i = 0; i < 40; i++) {
				var type = rand.Next () % 5;
				Animal animal = null;
				switch (type) {
				case 0:
					animal = new Animal { Name = "Dog", Color = "Black" };
					break;
				case 1:
					animal = new Animal { Name = "Cat", Color = "Pink" };
					break;
				case 2:
					animal = new Animal { Name = "Tiger", Color = "Orange" };
					break;
				case 3:
					animal = new Animal { Name = "Lion", Color = "Yellow" };
					break;
				case 4:
					animal = new Animal { Name = "Cheburashka", Color = "Brown" };
					break;
				default:
					animal = null;
					break;
				}
				animal.Weight = rand.NextDouble () * 100 + rand.NextDouble () * 10 + rand.NextDouble ();
				list.Add (animal);
			}
			return list;
		}
	}
}

