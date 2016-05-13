using System;
using System.Collections.Generic;
using Android.Animation;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace TestLec5
{
	[Register("TestLec5.DynamicListView")]
	public class DynamicListView : ListView //, View.IOnLongClickListener
	{

		private int _downY = -1;
		private int _downX = -1;
		private int _totalOffset = 0;
		private long _mobileItemId = -1;
		private BitmapDrawable _hoverCell;
		private Rect _hoverCellCurrentBounds;
		private Rect _hoverCellOriginalBounds;
		private bool _cellIsMobile ;

		private long _aboveItemId = -1;
		private long _belowItemId = -1;

		private int _previousFirstVisibleItem = -1;
		private int _previousVisibleItemCount = -1;
		private int _currentFirstVisibleItem;
		private int _currentVisibleItemCount;
		private ScrollState _currentScrollState;
		private ScrollState _scrollState;
		private int _lastEventY = -1;

		public List<object> Items;

		bool _isMobileScrolling = false;
		int _smoothScrollAmountAtEdge;
		bool _isWaitingForScrollFinish = false;
		int _activePointerId=-1;

		new StableArrayAdapter Adapter {
			get {
				return base.Adapter as StableArrayAdapter;
			}
		}

		public DynamicListView (IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer) : base (javaReference, transfer)
		{
			Initialize ();
		}

		public DynamicListView (Android.Content.Context context) : base (context)
		{
			Initialize ();
		}

		public DynamicListView (Android.Content.Context context, Android.Util.IAttributeSet attrs) : base (context, attrs)
		{
			Initialize ();
		}

		public DynamicListView (Android.Content.Context context, Android.Util.IAttributeSet attrs, int defStyleAttr) : base (context, attrs, defStyleAttr)
		{
			Initialize ();
		}

		public DynamicListView (Android.Content.Context context, Android.Util.IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base (context, attrs, defStyleAttr, defStyleRes)
		{
			Initialize ();
		}

		void Initialize ()
		{
			ItemLongClick += OnItemLongClick;
			//SetOnLongClickListener (this);
			//LongClick += OnLongClick;
			Scroll += Scrolled;
			ScrollStateChanged += OnScrollStateChanged;

			DisplayMetrics metrics = Context.Resources.DisplayMetrics;
			_smoothScrollAmountAtEdge = (int)(15 / metrics.Density);
		}

		void Scrolled (object sender, ScrollEventArgs e)
		{
			_currentFirstVisibleItem = e.FirstVisibleItem;
			_currentVisibleItemCount = e.VisibleItemCount;

			_previousFirstVisibleItem = (_previousFirstVisibleItem == -1) ? _currentFirstVisibleItem
					: _previousFirstVisibleItem;
			_previousVisibleItemCount = (_previousVisibleItemCount == -1) ? _currentVisibleItemCount
					: _previousVisibleItemCount;

			CheckAndHandleFirstVisibleCellChange ();
			CheckAndHandleLastVisibleCellChange ();

			_previousFirstVisibleItem = _currentFirstVisibleItem;
			_previousVisibleItemCount = _currentVisibleItemCount;
		}

		void OnScrollStateChanged (object sender, ScrollStateChangedEventArgs e)
		{
			_currentScrollState = e.ScrollState;
			_scrollState = e.ScrollState;
			IsScrollCompleted ();
		}

		/**
         * This method is in charge of invoking 1 of 2 actions. Firstly, if the listview
         * is in a state of scrolling invoked by the hover cell being outside the bounds
         * of the listview, then this scrolling event is continued. Secondly, if the hover
         * cell has already been released, this invokes the animation for the hover cell
         * to return to its correct position after the listview has entered an idle scroll
         * state.
         */
		private void IsScrollCompleted ()
		{
			if (_currentVisibleItemCount > 0 && _currentScrollState == ScrollState.Idle) {
				if (_cellIsMobile && _isMobileScrolling) {
					HandleMobileCellScroll ();
				} else if (_isWaitingForScrollFinish) {
					TouchEventsEnded ();
				}
			}
		}

		//public bool OnLongClick (View v)
		//{
		//	_totalOffset = 0;

		//	int position = PointToPosition (_downX, _downY);
		//	int itemNum = position - FirstVisiblePosition;


		//	View selectedView = GetChildAt (itemNum);

		//	_mobileItemId = this.Adapter.GetItemId (position);
		//	_hoverCell = GetAndAddHoverView (selectedView);
		//	selectedView.Visibility = ViewStates.Invisible;

		//	_cellIsMobile = true;

		//	updateNeighborViewsForID (_mobileItemId);

		//	return true;
		//}

		public void OnItemLongClick (object sender, ItemLongClickEventArgs e)
		{
			_totalOffset = 0;

			int position = PointToPosition (_downX, _downY);
			int itemNum = position - FirstVisiblePosition;


			View selectedView = GetChildAt (itemNum);

			_mobileItemId = this.Adapter.GetItemId (position);
			_hoverCell = GetAndAddHoverView (selectedView);
			selectedView.Visibility = ViewStates.Invisible;

			_cellIsMobile = true;

			updateNeighborViewsForID (_mobileItemId);

		}

		private BitmapDrawable GetAndAddHoverView (View v)
		{

			int w = v.Width;
			int h = v.Height;
			int top = v.Top;
			int left = v.Left;

			Bitmap b = GetBitmapWithBorder (v);

			BitmapDrawable drawable = new BitmapDrawable (Resources, b);

			_hoverCellOriginalBounds = new Rect (left, top, left + w, top + h);
			_hoverCellCurrentBounds = new Rect (_hoverCellOriginalBounds);

			drawable.SetBounds (_hoverCellCurrentBounds.Left, _hoverCellCurrentBounds.Top, _hoverCellCurrentBounds.Right, _hoverCellCurrentBounds.Bottom);

			return drawable;
		}

		/** Draws a black border over the screenshot of the view passed in. */
		private Bitmap GetBitmapWithBorder (View v)
		{
			Bitmap bitmap = GetBitmapFromView (v);
			Canvas can = new Canvas (bitmap);

			Rect rect = new Rect (0, 0, bitmap.Width, bitmap.Height);

			Paint paint = new Paint ();
			paint.SetStyle (Paint.Style.Stroke);
			paint.StrokeWidth = 15;
			paint.Color = (Color.Black);

			can.DrawBitmap (bitmap, 0, 0, null);
			can.DrawRect (rect, paint);

			return bitmap;
		}

		/** Returns a bitmap showing a screenshot of the view passed in. */
		private Bitmap GetBitmapFromView (View v)
		{
			Bitmap bitmap = Bitmap.CreateBitmap (v.Width, v.Height, Bitmap.Config.Argb8888);
			Canvas canvas = new Canvas (bitmap);
			v.Draw (canvas);
			return bitmap;
		}

		/**
	   * Stores a reference to the views above and below the item currently
	   * corresponding to the hover cell. It is important to note that if this
	   * item is either at the top or bottom of the list, mAboveItemId or mBelowItemId
	   * may be invalid.
	   */
		private void updateNeighborViewsForID (long itemID)
		{
			int position = GetPositionForID (itemID);
			var adapter = this.Adapter;
			_aboveItemId = adapter.GetItemId (position - 1);
			_belowItemId = adapter.GetItemId (position + 1);
			Console.WriteLine ("updateNeighborViewsForID above - {0}, below - {1}", _aboveItemId, _belowItemId);
		}

		/** Retrieves the position in the list corresponding to itemID */
		public int GetPositionForID (long itemID)
		{
			View v = GetViewForID (itemID);
			if (v == null) {
				return -1;
			} else {
				return GetPositionForView (v);
			}
		}

		/** Retrieves the view in the list corresponding to itemID */
		public View GetViewForID (long itemID)
		{
			int firstVisiblePosition = FirstVisiblePosition;
			var adapter = this.Adapter;
			for (int i = 0; i < ChildCount; i++) {
				View v = GetChildAt (i);
				int position = firstVisiblePosition + i;
				long id = adapter.GetItemId (position);
				if (id == itemID) {
					return v;
				}
			}
			return null;
		}

		/**
         * Determines if the listview scrolled up enough to reveal a new cell at the
         * top of the list. If so, then the appropriate parameters are updated.
         */
		public void CheckAndHandleFirstVisibleCellChange ()
		{
			if (_currentFirstVisibleItem != _previousFirstVisibleItem) {
				if (_cellIsMobile && _mobileItemId != -1) {
					updateNeighborViewsForID (_mobileItemId);
					HandleCellSwitch ();
				}
			}
		}

		/**
         * Determines if the listview scrolled down enough to reveal a new cell at the
         * bottom of the list. If so, then the appropriate parameters are updated.
         */
		public void CheckAndHandleLastVisibleCellChange ()
		{
			int currentLastVisibleItem = _currentFirstVisibleItem + _currentVisibleItemCount;
			int previousLastVisibleItem = _previousFirstVisibleItem + _previousVisibleItemCount;
			if (currentLastVisibleItem != previousLastVisibleItem) {
				if (_cellIsMobile && _mobileItemId != -1) {
					updateNeighborViewsForID (_mobileItemId);
					HandleCellSwitch ();
				}
			}
		}

		/**
	  * Stores a reference to the views above and below the item currently
	  * corresponding to the hover cell. It is important to note that if this
	  * item is either at the top or bottom of the list, mAboveItemId or mBelowItemId
	  * may be invalid.
	  */
		private void UpdateNeighborViewsForID (long itemID)
		{
			int position = GetPositionForID (itemID);
			_aboveItemId = Adapter.GetItemId (position - 1);
			_belowItemId = Adapter.GetItemId (position + 1);
		}

		/**
	* This method determines whether the hover cell has been shifted far enough
	* to invoke a cell swap. If so, then the respective cell swap candidate is
	* determined and the data set is changed. Upon posting a notification of the
	* data set change, a layout is invoked to place the cells in the right place.
	* Using a ViewTreeObserver and a corresponding OnPreDrawListener, we can
	* offset the cell being swapped to where it previously was and then animate it to
	* its new position.
	*/
		private void HandleCellSwitch ()
		{
			int deltaY = _lastEventY - _downY;
			int deltaYTotal = _hoverCellOriginalBounds.Top + _totalOffset + deltaY;

			View belowView = GetViewForID (_belowItemId);
			View mobileView = GetViewForID (_mobileItemId);
			View aboveView = GetViewForID (_aboveItemId);

			bool isBelow = (belowView != null) && (deltaYTotal > belowView.Top);
			bool isAbove = (aboveView != null) && (deltaYTotal < aboveView.Top);

			if (isBelow || isAbove) {

				var switchItemID = isBelow ? _belowItemId : _aboveItemId;
				View switchView = isBelow ? belowView : aboveView;
				int originalItem = GetPositionForView (mobileView);

				if (switchView == null) {
					UpdateNeighborViewsForID (_mobileItemId);
					return;
				}

				Adapter.SwapElements (originalItem, GetPositionForView (switchView));

				((BaseAdapter)Adapter).NotifyDataSetChanged ();

				_downY = _lastEventY;

				var switchViewStartTop = switchView.Top;

				mobileView.Visibility = ViewStates.Invisible;
				switchView.Visibility = ViewStates.Visible;

				UpdateNeighborViewsForID (_mobileItemId);

				var observer = ViewTreeObserver;

				EventHandler<ViewTreeObserver.PreDrawEventArgs> handler = null;
				handler = (sender, e) => {
					ViewTreeObserver.PreDraw -=handler;

					switchView = GetViewForID (switchItemID);

					_totalOffset += deltaY;

					int switchViewNewTop = switchView.Top;
					int delta = switchViewStartTop - switchViewNewTop;

					switchView.TranslationY = delta;

					ObjectAnimator animator = ObjectAnimator.OfFloat (switchView, "TranslationY", 0);
					animator.SetDuration (150);
					animator.Start ();
				};

				observer.PreDraw += handler;
			}
		}


		/**
     *  Determines whether this listview is in a scrolling state invoked
     *  by the fact that the hover cell is out of the bounds of the listview;
     */
		private void HandleMobileCellScroll ()
		{
			_isMobileScrolling = HandleMobileCellScroll (_hoverCellCurrentBounds);
		}

		/**
		 * This method is in charge of determining if the hover cell is above
		 * or below the bounds of the listview. If so, the listview does an appropriate
		 * upward or downward smooth scroll so as to reveal new items.
		 */
		public bool HandleMobileCellScroll (Rect r)
		{
			int offset = ComputeHorizontalScrollOffset ();
			int height = Height;
			int extent = ComputeVerticalScrollExtent ();
			int range = ComputeVerticalScrollRange ();
			int hoverViewTop = r.Top;
			int hoverHeight = r.Height();

			if (hoverViewTop <= 0 && offset > 0) {
				SmoothScrollBy (-_smoothScrollAmountAtEdge, 0);
				return true;
			}

			if (hoverViewTop + hoverHeight >= height && (offset + extent) < range) {
				SmoothScrollBy (_smoothScrollAmountAtEdge, 0);
				return true;
			}

			return false;
		}

		/**
	*  dispatchDraw gets invoked when all the child views are about to be drawn.
	*  By overriding this method, the hover cell (BitmapDrawable) can be drawn
	*  over the listview's items whenever the listview is redrawn.
	*/

		protected override void DispatchDraw (Canvas canvas)
		{
			base.DispatchDraw (canvas);
			if (_hoverCell != null) {
				_hoverCell.Draw (canvas);
			}
		}
		public override bool OnTouchEvent (MotionEvent e)
		{
			switch (e.Action & MotionEventActions.Mask) {
				case MotionEventActions.Down:
				_downX = (int)e.GetX();
				_downY = (int)e.GetY();
				_activePointerId = e.GetPointerId(0);
                break;
				case MotionEventActions.Move:
                	if (_activePointerId == -1) {
						break;
					}


				int pointerIndex = e.FindPointerIndex(_activePointerId);
				_lastEventY = (int) e.GetY(pointerIndex);
				int deltaY = _lastEventY - _downY;
                if (_cellIsMobile) {
                    _hoverCellCurrentBounds.OffsetTo(_hoverCellOriginalBounds.Left,
                            _hoverCellOriginalBounds.Top + deltaY + _totalOffset);
					_hoverCell.SetBounds (_hoverCellCurrentBounds.Left, _hoverCellCurrentBounds.Top, _hoverCellCurrentBounds.Right, _hoverCellCurrentBounds.Bottom);

					Invalidate ();
					HandleCellSwitch ();

					_isMobileScrolling = false;
					HandleMobileCellScroll ();
                    return false;
                }
                break;
				case MotionEventActions.Up:
				TouchEventsEnded ();
                break;
				case MotionEventActions.Cancel:

				TouchEventsCancelled ();
                break;
				case MotionEventActions.PointerUp:
				/* If a multitouch event took place and the original touch dictating
                 * the movement of the hover cell has ended, then the dragging event
                 * ends and the hover cell is animated to its corresponding position
                 * in the listview. */
				pointerIndex = (int)(e.Action & MotionEventActions.PointerIndexMask) >> (int)MotionEventActions.PointerIndexShift;
				var pointerId = e.GetPointerId(pointerIndex);
                if (pointerId == _activePointerId) {
					TouchEventsEnded ();
				}
                break;
            default:
                break;
			}
			return base.OnTouchEvent (e);
		}

		/**
	* Resets all the appropriate fields to a default state while also animating
	* the hover cell back to its correct location.
	*/
		private void TouchEventsEnded ()
		{
			var mobileView = GetViewForID (_mobileItemId);
			if (_cellIsMobile || _isWaitingForScrollFinish) {
				_cellIsMobile = false;
				_isWaitingForScrollFinish = false;
				_isMobileScrolling = false;
				_activePointerId = -1;

				// If the autoscroller has not completed scrolling, we need to wait for it to
				// finish in order to determine the final location of where the hover cell
				// should be animated to.
				if (_scrollState != ScrollState.Idle) {
					_isWaitingForScrollFinish = true;
					return;
				}

				_hoverCellCurrentBounds.OffsetTo (_hoverCellOriginalBounds.Left, mobileView.Top);

				ObjectAnimator hoverViewAnimator = ObjectAnimator.OfObject (_hoverCell, "bounds",
				        new BoundEvalutor(), _hoverCellCurrentBounds);

				hoverViewAnimator.Update += (sender, e) => {
					Invalidate ();
				};

				hoverViewAnimator.AnimationStart += (sender, e) => {
					Enabled = false;
				};

				hoverViewAnimator.AnimationEnd += (sender, e) => {
					_aboveItemId = -1;
					_mobileItemId = -1;
					_belowItemId = -1;
					mobileView.Visibility = ViewStates.Visible;
					_hoverCell = null;
					Enabled = true;
					Invalidate ();
				};
          
            	hoverViewAnimator.Start();
	        } else {

				TouchEventsCancelled ();
	        }
    	}

    /**
     * Resets all the appropriate fields to a default state.
     */
	    private void TouchEventsCancelled ()
		{
			View mobileView = GetViewForID (_mobileItemId);
			if (_cellIsMobile) {
				_aboveItemId = -1;
				_mobileItemId = -1;
				_belowItemId = -1;
				mobileView.Visibility = ViewStates.Visible;
				_hoverCell = null;
				Invalidate ();
			}
			_cellIsMobile = false;
			_isMobileScrolling = false;
			_activePointerId = -1;
		}


		class BoundEvalutor : Java.Lang.Object, ITypeEvaluator
		{
			public Java.Lang.Object Evaluate (float fraction, Java.Lang.Object startValue, Java.Lang.Object endValue)
			{
				return Evaluate (fraction, startValue as Rect, endValue as Rect);
			}

			public Rect Evaluate (float fraction, Rect startValue, Rect endValue)
			{
				return new Rect (interpolate (startValue.Left, endValue.Left, fraction),
						interpolate (startValue.Top, endValue.Top, fraction),
						interpolate (startValue.Right, endValue.Right, fraction),
						interpolate (startValue.Bottom, endValue.Bottom, fraction));
			}

			public int interpolate (int start, int end, float fraction)
			{
				return (int)(start + fraction * (end - start));
			}
		}
	}
}

