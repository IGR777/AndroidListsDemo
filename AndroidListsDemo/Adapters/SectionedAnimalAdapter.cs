using System;
using System.Collections.Generic;
using Android.Util;
using Android.Widget;
using Android.Views;
using System.Linq;
using Java.Util;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace TestLec5
{
	public class SectionedAnimalAdapter: AnimalAdapter
	{
		Dictionary<string, List<Animal>> _sections;

		public SectionedAnimalAdapter(IList<Animal> animals) : base(animals)
		{
			_sections = new Dictionary<string, List<Animal>> ();
			BuildSections ();

		}

		void BuildSections(){
			_sections.Clear ();
			foreach (var s in _animals.GroupBy (item => item.Name).OrderBy(gr=>gr.Key)) {
				_sections.Add (s.Key, s.ToList ());
			}
		}

		public override Animal this[int position]
		{
			get
			{
				foreach (var section in _sections.Keys) {
					var items = _sections [section];
					int size = items.Count + 1;
					if (position == 0)
						return null;
					if (position < size)
						return items[position - 1];
					position -= size;
				}
				return null;
			}
		}

		public override int Count {
			get {
				return base.Count + _sections.Count;
			}
		}

		public override int ViewTypeCount {
			get {
				return 2;
			}
		}

		public override int GetItemViewType (int position)
		{
			foreach (var section in _sections.Keys) {
				var items = _sections [section];
				int size = items.Count + 1;
				if (position == 0)
					return 0;
				if (position < size)
					return 1;
				position -= size;
			}
			throw new NullReferenceException ();
		}

		public override void Remove (Animal item)
		{
			base.Remove (item);
			BuildSections ();
		}

		public override void Remove (IEnumerable<Animal> items)
		{
			base.Remove (items);
			BuildSections ();
		}

		#region TODO9
		//TODO 09
		//public override void RollBack ()
		//{
		//	base.RollBack ();
		//	BuildSections ();
		//}
		#endregion

		public override Animal GetRawItem (int position)
		{

			foreach (var section in _sections.Keys) {
				var items = _sections [section];
				int size = items.Count + 1;
				if (position == 0)
					return null;
				if (position < size)
					return items[position - 1];
				position -= size;
			}
			return null;
		}

		protected override void SelectView (int position, bool value)
		{
			var vt = GetItemViewType (position);
			if (vt == 0)
				return;
			base.SelectView (position, value);
		}

		string GetSection(int position){
			foreach (var section in _sections.Keys) {
				var items = _sections [section];
				int size = items.Count + 1;
				if (position == 0 || (position < size))
					return section;
				position -= size;
			}
			throw new NullReferenceException();
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var type = GetItemViewType (position);
			if (type == 0) {
				var view = convertView;
				if(view ==null){
					view = LayoutInflater.From (parent.Context).Inflate (Resource.Layout.row_custom_header, parent, false);
					var sectionViewHolder = new SectionViewHolder ();
					sectionViewHolder.Text = view.FindViewById<TextView> (Resource.Id.list_header_title);
					view.Tag = sectionViewHolder;
				}
				var holder = (SectionViewHolder)view.Tag;
				holder.Text.Text = GetSection (position) + "s";
				return view;
			} else {
				var item = this [position];
				var view = convertView;
				if(view ==null){
					view = LayoutInflater.From (parent.Context).Inflate (Resource.Layout.row_custom_adapter, parent, false);
					var viewHolder = new ViewHolder();
					viewHolder.Text = view.FindViewById<TextView > (Resource.Id.row_custom_name);
					viewHolder.Weight = view.FindViewById<TextView > (Resource.Id.row_custom_weight);
					viewHolder.Icon = view.FindViewById<ImageView> (Resource.Id.row_custom_icon);
					view.Tag = viewHolder;
				}
				var holder = (ViewHolder)view.Tag;
				holder.Text.Text = item.Name;
				holder.Weight.Text = item.Weight.ToString ("F");
				if (_selectedItemsIds.Size () > 0 && _selectedItemsIds.Get (position)) {
					view.SetBackgroundColor (Android.Graphics.Color.CadetBlue); 
				} else {
					view.Background = GetBackground (item.Color);
				}
				holder.Icon.SetImageResource (GetImage (item.Name));
				return view;
			}
		}

		class ViewHolder : Java.Lang.Object{
			public TextView Text { get; set; }
			public TextView Weight { get; set; }
			public ImageView Icon { get; set; }
		}

		class SectionViewHolder : Java.Lang.Object{
			public TextView Text { get; set; }
		}
	}
}

