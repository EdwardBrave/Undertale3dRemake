using System;
using System.Collections.Generic;

namespace UI.Binding
{
	public struct Bonded<T>
    {
		public readonly string name;
	    public readonly T content;

	    public Bonded(string name, T content)
	    {
		    this.name = name;
		    this.content = content;
	    }
    }
    
    public static class BondedListExtension
	{
		public static T Get<T>(this List<Bonded<T>> list, string name)
		{
			try
			{
				var item = list.Find(o => o.name == name);
				return item.content;
			}
			catch (ArgumentNullException)
			{
				return default(T);
			}
		}
	}
}