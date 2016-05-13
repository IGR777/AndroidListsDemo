using System;
using Android.Widget;
using System.Collections.Generic;
using Android.Views;
using Android.Util;
using System.Linq;
using Android.Graphics.Drawables;
using Android.Graphics;

namespace TestLec5
{
	public class AnimalAdapter : BaseAdapter<Animal>
	{
		protected IList<Animal> _animals;
		protected SparseBooleanArray _selectedItemsIds;
		#region TODO9
		//TODO 09
		//protected IList<Animal> _rollbackAnimals;
		#endregion


		public AnimalAdapter(IList<Animal> animals)
		{
			_animals = animals;

			#region TODO6
			//TODO 06
			//_selectedItemsIds= new SparseBooleanArray();
			#endregion
		}

		public override Animal this[int position]
		{
			get
			{
				return _animals[position];
			}
		}


		public override int Count
		{
			get
			{
				return _animals.Count;
			}
		}

		public override long GetItemId(int position)
		{
			return position;
		} 

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.row_custom_adapter, parent, false);

			var name = view.FindViewById<TextView >(Resource.Id.row_custom_name);
			var weight = view.FindViewById<TextView >(Resource.Id.row_custom_weight);
			var icon = view.FindViewById<ImageView> (Resource.Id.row_custom_icon);

			name.Text = _animals[position].Name;
			weight.Text = _animals[position].Weight.ToString("F");
			icon.SetImageResource (GetImage (_animals [position].Name));

			#region TODO6
			//TODO 06
			//if (_selectedItemsIds.Size () > 0 && _selectedItemsIds.Get (position)) {
			//	view.SetBackgroundColor (Android.Graphics.Color.CadetBlue); 
			//} else {
			//	view.Background = GetBackground (_animals [position].Color);
			//}
			#endregion
			return view;
		}




		protected Drawable GetBackground (string name)
		{
			switch (name) {
			case "Black":
				return new ColorDrawable (Color.Black);
			case "Pink":
				return new ColorDrawable (Color.DeepPink);
			case "Orange":
				return new ColorDrawable (Color.Orange);
			case "Yellow":
				return new ColorDrawable (Color.DarkGoldenrod);
			case "Brown":
				return new ColorDrawable (Color.Brown);
			}
			return null;
		}

		protected int GetImage (string name)
		{
			switch (name) {
			case "Dog":
				return Resource.Drawable.dog;
			case "Cat":
				return Resource.Drawable.cat;
			case "Tiger":
				return Resource.Drawable.tiger;
			case "Lion":
				return Resource.Drawable.lion;
			case "Cheburashka":
				return Resource.Drawable.cheb;
			}
			return 0;
		}

		public virtual void Remove(Animal item){
			#region TODO9
			//TODO 09
			//_rollbackAnimals = _animals.ToList ();
			#endregion
			_animals.Remove (item);
		}

		public virtual void Remove(IEnumerable<Animal> items){
			#region TODO9
			//TODO 09
			//_rollbackAnimals = _animals.ToList ();
			#endregion
			foreach (var item in items) {
				_animals.Remove (item);
			}
		}

		public virtual Animal GetRawItem(int position){
			return _animals [position];	
		}

		protected virtual void SelectView(int position, bool value){

			#region TODO6
			//TODO 06
			//if(value)
			//	_selectedItemsIds.Put(position,value);
			//else
			//	_selectedItemsIds.Delete(position);

			//NotifyDataSetChanged();
			#endregion
		}
		#region TODO6
		//TODO 06

		//public void ToggleSelection(int position){
		//	SelectView(position, !_selectedItemsIds.Get(position));
		//}

		//public int SelectedCount {
		//	get{
		//		return _selectedItemsIds.Size();
		//	}
		//}

		//public void RemoveSelection(){
		//	_selectedItemsIds=new SparseBooleanArray();
		//	NotifyDataSetChanged();
		//}

		//public SparseBooleanArray SelectedIds{
		//	get {
		//		return _selectedItemsIds;
		//	}
		//}
		#endregion

		#region TODO7
		//TODO 07
		//public override View GetView(int position, View convertView, ViewGroup parent)
		//{
		//	var item = this [position];
		//	var view = convertView;
		//	if (view == null) {
		//		view = LayoutInflater.From (parent.Context).Inflate (Resource.Layout.row_custom_adapter, parent, false);
		//		var viewHolder = new ViewHolder ();
		//		viewHolder.Text = view.FindViewById<TextView> (Resource.Id.row_custom_name);
		//		viewHolder.Weight = view.FindViewById<TextView> (Resource.Id.row_custom_weight);
		//		viewHolder.Icon = view.FindViewById<ImageView> (Resource.Id.row_custom_icon);
		//		view.Tag = viewHolder;
		//	}
		//	var holder = (ViewHolder)view.Tag;
		//	holder.Text.Text = item.Name;
		//	holder.Weight.Text = item.Weight.ToString ("F");
		//	if (_selectedItemsIds.Size () > 0 && _selectedItemsIds.Get (position)) {
		//		view.SetBackgroundColor (Android.Graphics.Color.CadetBlue);
		//	} else {
		//		view.Background = GetBackground (item.Color);
		//	}
		//	holder.Icon.SetImageResource (GetImage (item.Name));
		//	return view;
		//}



		//class ViewHolder : Java.Lang.Object{
		//	public TextView Text { get; set; }
		//	public TextView Weight { get; set; }
		//	public ImageView Icon { get; set; }
		//}
		#endregion

		#region TODO9
		public virtual void RollBack ()
		{
			//TODO 09 
			//_animals = _rollbackAnimals;
			//_rollbackAnimals = null;
		}
		#endregion
	}
}

