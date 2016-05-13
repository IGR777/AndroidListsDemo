using Android.App;
using Android.Widget;
using Android.OS;
using Java.Util;
using System.Collections.Generic;
using System;
using Java.Lang;
using Android.Views;
using Android.Util;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Text;
using Android.Text.Style;
using Android.Support.V7.Widget;

namespace TestLec5
{
	[Activity (Label = "AndroidListsDemo"
	           , MainLauncher = true
	          )]
	public class MainActivity : AppCompatActivity 
	#region TODO6
	//TODO 06
	//, ActionMode.ICallback 
	#endregion
	{
		#region TODO6
		//TODO 06
		//bool inActionMode;
		//ActionMode _mode;
		//AnimalAdapter _adapter;
		#endregion


		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
			var list = FindViewById<ListView> (Resource.Id.myList);

			var animals = GetAnimals ();
			var adapter = new ArrayAdapter<Animal> (this, Android.Resource.Layout.SimpleListItem1, animals);

			#region TODO4
			//TODO 04
			//var adapter = new ArrayAdapter<Animal> (this, Resource.Layout.row_custom, Resource.Id.row_custom_text, animals);
			#endregion

			#region TODO5
			//TODO 05
			//var adapter = new AnimalAdapter(animals);
			#endregion

			#region TODO8
			//TODO 08
			//var adapter = new SectionedAnimalAdapter(animals);
			#endregion

			#region TODO6
			//TODO 06
			//_adapter =adapter;
			#endregion

			list.Adapter = adapter;

			#region TODO2
			//TODO 02
			//bool inDeletion = false;
			//list.ItemClick +=  (sender, e) => {
			//	#region TODO6
			//	//TODO 06
			//	//if(inActionMode){
			//	//	adapter.ToggleSelection(e.Position);
			//	//	_mode.Title=(adapter.SelectedCount.ToString()+" selected");
			//	//	return;
			//	//}
			//	#endregion
			//	if(!inDeletion){
			//		inDeletion = true;
			//		e.View.Animate()
			//			.SetDuration(500)
			//			.Alpha(0.0f)
			//			.WithEndAction(new Runnable(()=>{
			//				#region TODO5
			//				//TODO 05
			//				//var item = adapter.GetRawItem(e.Position);
			//				#endregion
			//				var item = list.Adapter.GetItem(e.Position);
			//				adapter.Remove(item);
			//				adapter.NotifyDataSetChanged();
			//				e.View.Alpha = 1;
			//				#region TODO9
			//				//TODO 09
			//				//ShowSnackBar (1);
			//				#endregion
			//				inDeletion = false;
			//			}));
			//	}
			//};
			#endregion

			#region TODO3
			//TODO 03
			//list.EmptyView = FindViewById<TextView> (Resource.Id.empty);
			#endregion

			#region TODO6
			//TODO 06
			//list.ItemLongClick += (sender, e) => {
			//	adapter.ToggleSelection(e.Position);
			//	var hasCheckedItems=adapter.SelectedCount>0;

			//	if(hasCheckedItems && _mode==null)
			//		_mode = StartActionMode(this);

			//	else if(!hasCheckedItems && _mode !=null)
			//		_mode.Finish();

			//	if(_mode !=null)
			//		_mode.Title=(adapter.SelectedCount.ToString()+" selected");
			//};
			#endregion
		}

		#region TODO10
		//TODO 10
		//protected override void OnCreate (Bundle savedInstanceState)
		//{
		//	base.OnCreate (savedInstanceState);

		//	// Set our view from the "main" layout resource
		//	SetContentView (Resource.Layout.Main_expanded);
		//	var list = FindViewById<ExpandableListView> (Resource.Id.myExpandableList);

		//	var animals = GetAnimals ();
		//	var adapter = new ExpandableAnimalAdapter (animals);

		//	list.SetAdapter (adapter);
		//}
		#endregion

		#region TODO6
		//TODO 06
		//public bool OnActionItemClicked (ActionMode mode, IMenuItem item)
		//{
		//	switch(item.ItemId){
		//	case Resource.Id.ActionModeDeleteItem:
		//		SparseBooleanArray selected = _adapter.SelectedIds;
		//		List<Animal> itemList = new List<Animal> ();
		//		var keys = new List<int> ();
		//		for (int i = (selected.Size () - 1); i >= 0; i--) {
		//			//checkisvaluecheckedby user
		//			if (selected.ValueAt (i)) {
		//				keys.Add (selected.KeyAt (i));
		//				var selectedItem = _adapter.GetRawItem (selected.KeyAt (i));
		//				itemList.Add (selectedItem);
		//			}
		//		}
		//		_adapter.Remove (itemList);
		//		_mode.Finish();
		//		#region TODO9
		//		//TODO 09
		//		//ShowSnackBar (itemList.Count);
		//		#endregion
		//		return true;
		//	default:
		//		return false;
		//	}
		//}

		//public bool OnCreateActionMode (ActionMode mode, IMenu menu)
		//{
		//	mode.MenuInflater.Inflate (Resource.Menu.menu, menu);
		//	inActionMode = true;
		//	return true;
		//}

		//public void OnDestroyActionMode (ActionMode mode)
		//{
		//	_adapter.RemoveSelection ();
		//	_adapter.NotifyDataSetChanged ();
		//	_mode = null;
		//	inActionMode = false;
		//}

		//public bool OnPrepareActionMode (ActionMode mode, IMenu menu)
		//{
		//	return false;
		//}
		#endregion

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
		#region TODO9
		//TODO 09
		//void ShowSnackBar (int count)
		//{
		//	var snack = Snackbar.Make (this.FindViewById (Android.Resource.Id.Content), count + " deleted", 5000);
		//	snack.SetAction (" ", (v) => {
		//		_adapter.RollBack ();
		//		_adapter.NotifyDataSetChanged ();
		//	});
		//	var act = snack.View.FindViewById<AppCompatButton> (Resource.Id.snackbar_action);
		//	act.SetBackgroundResource(Resource.Drawable.back);
		//	snack.Show ();
		//}
		#endregion
	}


}


