package crc6446009a0d5670cebe;


public class HorizontalBox5
	extends android.view.View
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDraw:(Landroid/graphics/Canvas;)V:GetOnDraw_Landroid_graphics_Canvas_Handler\n" +
			"";
		mono.android.Runtime.register ("Com.Syncfusion.Sfbusyindicator.HorizontalBox5, Syncfusion.SfBusyIndicator.Android", HorizontalBox5.class, __md_methods);
	}


	public HorizontalBox5 (android.content.Context p0)
	{
		super (p0);
		if (getClass () == HorizontalBox5.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Sfbusyindicator.HorizontalBox5, Syncfusion.SfBusyIndicator.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public HorizontalBox5 (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == HorizontalBox5.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Sfbusyindicator.HorizontalBox5, Syncfusion.SfBusyIndicator.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public HorizontalBox5 (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == HorizontalBox5.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Sfbusyindicator.HorizontalBox5, Syncfusion.SfBusyIndicator.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public HorizontalBox5 (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == HorizontalBox5.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Sfbusyindicator.HorizontalBox5, Syncfusion.SfBusyIndicator.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public void onDraw (android.graphics.Canvas p0)
	{
		n_onDraw (p0);
	}

	private native void n_onDraw (android.graphics.Canvas p0);

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
