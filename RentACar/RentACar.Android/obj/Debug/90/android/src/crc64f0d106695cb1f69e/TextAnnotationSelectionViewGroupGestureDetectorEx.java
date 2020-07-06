package crc64f0d106695cb1f69e;


public class TextAnnotationSelectionViewGroupGestureDetectorEx
	extends android.view.GestureDetector.SimpleOnGestureListener
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onSingleTapUp:(Landroid/view/MotionEvent;)Z:GetOnSingleTapUp_Landroid_view_MotionEvent_Handler\n" +
			"n_onSingleTapConfirmed:(Landroid/view/MotionEvent;)Z:GetOnSingleTapConfirmed_Landroid_view_MotionEvent_Handler\n" +
			"";
		mono.android.Runtime.register ("Syncfusion.SfPdfViewer.XForms.Droid.TextAnnotationSelectionViewGroupGestureDetectorEx, Syncfusion.SfPdfViewer.XForms.Android", TextAnnotationSelectionViewGroupGestureDetectorEx.class, __md_methods);
	}


	public TextAnnotationSelectionViewGroupGestureDetectorEx ()
	{
		super ();
		if (getClass () == TextAnnotationSelectionViewGroupGestureDetectorEx.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfPdfViewer.XForms.Droid.TextAnnotationSelectionViewGroupGestureDetectorEx, Syncfusion.SfPdfViewer.XForms.Android", "", this, new java.lang.Object[] {  });
	}

	public TextAnnotationSelectionViewGroupGestureDetectorEx (crc64f0d106695cb1f69e.TextAnnotationSelectionViewGroup p0)
	{
		super ();
		if (getClass () == TextAnnotationSelectionViewGroupGestureDetectorEx.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfPdfViewer.XForms.Droid.TextAnnotationSelectionViewGroupGestureDetectorEx, Syncfusion.SfPdfViewer.XForms.Android", "Syncfusion.SfPdfViewer.XForms.Droid.TextAnnotationSelectionViewGroup, Syncfusion.SfPdfViewer.XForms.Android", this, new java.lang.Object[] { p0 });
	}


	public boolean onSingleTapUp (android.view.MotionEvent p0)
	{
		return n_onSingleTapUp (p0);
	}

	private native boolean n_onSingleTapUp (android.view.MotionEvent p0);


	public boolean onSingleTapConfirmed (android.view.MotionEvent p0)
	{
		return n_onSingleTapConfirmed (p0);
	}

	private native boolean n_onSingleTapConfirmed (android.view.MotionEvent p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
