using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moonmile.XmlDom
{
	/// <summary>
	/// XML attribute class
	/// </summary>
	public class XmlAttr
	{
		public string Key { get; set; }
		public string Value { get; set; }

		public XmlAttr()
		{
		}

		public XmlAttr(string key, string value)
		{
			this.Key = key;
			this.Value = value;
		}
	}
	/// <summary>
	/// XML Attribute collection
	/// </summary>
	public class XmlAttrCollection : List<XmlAttr>
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
