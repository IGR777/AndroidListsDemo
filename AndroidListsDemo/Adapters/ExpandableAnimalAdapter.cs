using System;
using System.Collections.Generic;
using Android.Util;
using Android.Widget;
using System.Linq;
using Android.Views;
using Java.Lang;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Support.V4.Content;

namespace TestLec5
{
	public class ExpandableAnimalAdapter : BaseExpandableListAdapter
	{
		protected IList<Animal> _animals;
		Dictionary<string, List<Animal>> _sections;

		public ExpandableAnimalAdapter (IList<Animal> animals)
		{
			_animals = animals;
			_sections = new Dictionary<string, List<Animal>> ();
			BuildSections ();
		}

		public override int GroupCount {
			get {
				return _sections.Keys.Count;
			}
		}

		public override bool HasStableIds {
			get {
				return false;
			}
		}

		public override Java.Lang.Object GetChild (int groupPosition, int childPosition)
		{
			return null;
		}

		public override long GetChildId (int groupPosition, int childPosition)
		{
			return 0;
		}

		public override int GetChildrenCount (int groupPosition)
		{
			var sect = _sections.Keys.ToList () [groupPosition];
			return _sections [sect].Count;
		}

		public override View GetGroupView (int position, bool isExpandable, View convertView, ViewGroup parent)
		{
			GroupViewHolder holder = null;
			var view = convertView;

			if (view != null)
				holder = view.Tag as GroupViewHolder;

			if (holder == null) {
				view = LayoutInflater.From (parent.Context).Inflate (Resource.Layout.row_expandable_header, null);
				holder = new GroupViewHolder ();
				holder.Text = view as CheckedTextView;
			}
			var sect = _sections.Keys.ToList () [position];

			var name = _sections [sect].First ().Name;
			holder.Text.Text = name;
			holder.Text.Checked = isExpandable;
			return view;
		}

		public override View GetChildView (int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
		{
			var view = convertView;
			var sect = _sections.Keys.ToList () [groupPosition];
			var item = _sections [sect] [childPosition];
			if (view == null) {
				view = LayoutInflater.From (parent.Context).Inflate (Resource.Layout.row_custom_adapter, parent, false);
				var viewHolder = new ChildViewHolder ();
				viewHolder.Text = view.FindViewById<TextView> (Resource.Id.row_custom_name);
				viewHolder.Weight = view.FindViewById<TextView> (Resource.Id.row_custom_weight);
				viewHolder.Icon = view.FindViewById<ImageView> (Resource.Id.row_custom_icon);
				view.Tag = viewHolder;
			}
			var holder = (ChildViewHolder)view.Tag;
			holder.Text.Text = item.Name;
			holder.Weight.Text = item.Weight.ToString ("F");
			holder.Icon.SetImageResource (GetImage (item.Name));
			return view;
		}

		public override Java.Lang.Object GetGroup (int groupPosition)
		{
			return null;
		}

		public override long GetGroupId (int groupPosition)
		{
			return groupPosition;
		}

		public override bool IsChildSelectable (int groupPosition, int childPosition)
		{
			return true;
		}

		void BuildSections ()
		{
			_sections.Clear ();
			foreach (var s in _animals.GroupBy (item => item.Color).OrderBy (gr => gr.Key)) {
				_sections.Add (s.Key, s.ToList ());
			}
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


		class GroupViewHolder : Java.Lang.Object
		{
			public CheckedTextView Text { get; set; }
		}

		class ChildViewHolder : Java.Lang.Object
		{
			public TextView Text { get; set; }
			public TextView Weight { get; set; }
			public ImageView Icon { get; set; }
		}
	}
}

