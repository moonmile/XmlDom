using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moonmile.HtmlDom
{
	/// <summary>
	/// HTML attribute class
	/// </summary>
	public class HtmlAttr
	{
		public string Key { get; set; }
		public string Value { get; set; }

		public HtmlAttr()
		{
		}

		public HtmlAttr(string key, string value)
		{
			this.Key = key;
			this.Value = value;
		}
	}
	/// <summary>
	/// XML Attribute collection
	/// </summary>
	public class HtmlAttrCollection : List<HtmlAttr>
	{
		public string this[string key]
		{
			get
			{
				var q = this.Find(n => n.Key == key);
				return (q == null) ? "" : q.Value;
			}
		}

	}


}
