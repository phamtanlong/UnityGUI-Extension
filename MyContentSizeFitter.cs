using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

namespace MySpace
{
	/// <summary>
	///   <para>Resizes a RectTransform to fit the size of its content.</para>
	/// </summary>
	[ExecuteInEditMode]
    [AddComponentMenu("Extension/My Content Size Fitter", 141)]
    [RequireComponent(typeof(RectTransform))]
	public class MyContentSizeFitter : UIBehaviour, ILayoutController, ILayoutSelfController
    {
        public Vector2 Margin = Vector2.zero;
        public Vector2 MinSize = new Vector2(100, 100);
		public Vector2 MaxSize = new Vector2(9999, 9999);

		[SerializeField]
		protected ContentSizeFitter.FitMode m_HorizontalFit;
		[SerializeField]
		protected ContentSizeFitter.FitMode m_VerticalFit;
		[NonSerialized]
		private RectTransform m_Rect;
		private DrivenRectTransformTracker m_Tracker;

		/// <summary>
		///   <para>The fit mode to use to determine the width.</para>
		/// </summary>
		public ContentSizeFitter.FitMode horizontalFit
		{
			get
			{
				return this.m_HorizontalFit;
			}
			set
			{
				if (!SetPropertyUtility.SetStruct<ContentSizeFitter.FitMode>(ref this.m_HorizontalFit, value))
					return;
				this.SetDirty();
			}
		}

		/// <summary>
		///   <para>The fit mode to use to determine the height.</para>
		/// </summary>
		public ContentSizeFitter.FitMode verticalFit
		{
			get
			{
				return this.m_VerticalFit;
			}
			set
			{
				if (!SetPropertyUtility.SetStruct<ContentSizeFitter.FitMode>(ref this.m_VerticalFit, value))
					return;
				this.SetDirty();
			}
		}

		private RectTransform rectTransform
		{
			get
			{
				if ((UnityEngine.Object) this.m_Rect == (UnityEngine.Object) null)
					this.m_Rect = this.GetComponent<RectTransform>();
				return this.m_Rect;
			}
		}

        protected MyContentSizeFitter()
        {
		}

        protected override void OnEnable()
        {
            base.OnEnable();
            this.SetDirty();
        }

		/// <summary>
		///   <para>See MonoBehaviour.OnDisable.</para>
		/// </summary>
		protected override void OnDisable()
		{
			this.m_Tracker.Clear();
			LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
			base.OnDisable();
		}

		protected override void OnRectTransformDimensionsChange()
		{
			this.SetDirty();
		}

		private void HandleSelfFittingAlongAxis(int axis)
		{
			ContentSizeFitter.FitMode fitMode = axis != 0 ? this.verticalFit : this.horizontalFit;
			if (fitMode == ContentSizeFitter.FitMode.Unconstrained)
			{
				this.m_Tracker.Add((UnityEngine.Object) this, this.rectTransform, DrivenTransformProperties.None);
			}
			else
			{
				this.m_Tracker.Add((UnityEngine.Object) this, this.rectTransform, axis != 0 ? DrivenTransformProperties.SizeDeltaY : DrivenTransformProperties.SizeDeltaX);
				if (fitMode == ContentSizeFitter.FitMode.MinSize)
					this.rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, LayoutUtility.GetMinSize(this.m_Rect, axis));
				else {
					var size = LayoutUtility.GetPreferredSize(this.m_Rect, axis);

					if (axis != 0) { //vertical
						var add = Margin.y;

						if (size + add < MinSize.y)
							size = MinSize.y - add;
						
						if (size + add > MaxSize.y)
							size = MaxSize.y - add;
						
					} else { //horizontal
						var add = Margin.x;

						if (size + add < MinSize.x)
							size = MinSize.x - add;
						
						if (size + add > MaxSize.x)
							size = MaxSize.x - add;
					}

					this.rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, size);
				}
			}
		}

//        private void HandleSelfFittingAlongAxis(int axis)
//        {
//            FitMode fitting = (axis == 0 ? horizontalFit : verticalFit);
//            if (fitting == FitMode.Unconstrained)
//                return;
//
//            m_Tracker.Add(this, rectTransform, (axis == 0 ? DrivenTransformProperties.SizeDeltaX : DrivenTransformProperties.SizeDeltaY));
//
//            // Set size to min or preferred size
//            RectTransform.Axis aa = (RectTransform.Axis)axis;
//            float add = aa == RectTransform.Axis.Vertical ? Margin.y : Margin.x;
//            float minSize = aa == RectTransform.Axis.Vertical ? MinSize.y : MinSize.x;
//
//            float size = LayoutUtility.GetMinSize(m_Rect, axis) + add;
//            if (size < minSize) size = minSize;
//
//            if (fitting == FitMode.MinSize)
//            {
//                rectTransform.SetSizeWithCurrentAnchors(aa, size);
//            }
//            else if (fitting == FitMode.PreferredSize)
//            {
//                rectTransform.SetSizeWithCurrentAnchors(aa, size);
//            }
//        }

		/// <summary>
		///   <para>Method called by the layout system.</para>
		/// </summary>
		public virtual void SetLayoutHorizontal()
		{
			this.m_Tracker.Clear();
			this.HandleSelfFittingAlongAxis(0);
		}

		/// <summary>
		///   <para>Method called by the layout system.</para>
		/// </summary>
		public virtual void SetLayoutVertical()
		{
			this.HandleSelfFittingAlongAxis(1);
		}

		/// <summary>
		///   <para>Mark the ContentSizeFitter as dirty.</para>
		/// </summary>
		protected void SetDirty()
		{
			if (!this.IsActive())
				return;
			LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
		}

#if UNITY_EDITOR
		protected override void OnValidate()
		{
			this.SetDirty();
		}
#endif

    }
}
