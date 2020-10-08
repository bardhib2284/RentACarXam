package crc644c2e2be3552eb7c9;


public class SfRangeSliderVisualRenderer
	extends crc644c2e2be3552eb7c9.SfRangeSliderRenderer
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Syncfusion.SfRangeSlider.XForms.Droid.SfRangeSliderVisualRenderer, Syncfusion.SfRangeSlider.XForms.Android", SfRangeSliderVisualRenderer.class, __md_methods);
	}


	public SfRangeSliderVisualRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == SfRangeSliderVisualRenderer.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfRangeSlider.XForms.Droid.SfRangeSliderVisualRenderer, Syncfusion.SfRangeSlider.XForms.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public SfRangeSliderVisualRenderer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == SfRangeSliderVisualRenderer.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfRangeSlider.XForms.Droid.SfRangeSliderVisualRenderer, Syncfusion.SfRangeSlider.XForms.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public SfRangeSliderVisualRenderer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == SfRangeSliderVisualRenderer.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfRangeSlider.XForms.Droid.SfRangeSliderVisualRenderer, Syncfusion.SfRangeSlider.XForms.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}

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
