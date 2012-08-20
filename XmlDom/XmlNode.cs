using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moonmile.XmlDom
{
	/// <summary>
	/// XML node class
	/// </summary>
	public class XmlNode 
	{
		#region property
		public string TagName { get; set; }
		protected string _value;
		/// <summary>
		/// Value propery
		/// If first child node is text node, Value property is equal text node.
		/// </summary>
		public string Value
		{
			get { return _value; }
			set
			{
				_value = value;
				/*
				_value = value;
				if (this.Children != null)
					if (this.Children.Count > 0)
						if (this.Children[0].TagName == "#text")
							this.Children[0].Value = _value;
				*/
			}
		}
		public XmlNodeCollection Children { get; set; }
		public XmlAttrCollection Attrs { get; set; }
		public XmlNode Parent { get; set; }
		#endregion

		#region constractor 
		public XmlNode()
		{
			this.TagName = "#empty";
			this.Value = "";
			this.Children = new XmlNodeCollection();
			this.Children.Parent = this;
			this.Attrs = new XmlAttrCollection();
		}

		public XmlNode(string tag, string value = "")
		{
			this.TagName = tag;
			this.Value = value;
			this.Children = new XmlNodeCollection();
			this.Children.Parent = this;
			this.Attrs = new XmlAttrCollection();
		}
		#endregion

		#region creator for method chain
		public XmlNode Node(string tag, string value = "")
		{
			var nd = new XmlNode(tag, value);
			this.Children.Add(nd);
			return nd;
		}
		public XmlNode AddNode(string tag, string value = "")
		{
			this.Node(tag, value);
			return this;
		}

		public XmlNode AddAttr(string key, string value)
		{
			this.Attrs.Add(new XmlAttr(key, value));
			return this;
		}
		public XmlNode SetValue(string value)
		{
			this.Value = value;
			return this;
		}
		#endregion

		#region xml string serialize
		public string Xml
		{
			get
			{
                string s = "";
                if (this.TagName == "" || this.TagName == "#text" || this.TagName == "#comment")
                {
                    s = this.Value.Trim();
                }
                else
                {
                    s += string.Format("<{0}", this.TagName);
                    foreach (var at in this.Attrs)
                    {
                        s += string.Format(" {0}=\"{1}\"", at.Key, at.Value);
                    }
                    if (this.Children.Count > 0)
                    {
                        s += ">";
						s += this.Value;
                        foreach (var el in this.Children)
                        {
                            s += el.Xml;
                        }
                        s += string.Format("</{0}>", this.TagName);
                    }
					else if (this.Value != "")
					{
						s += ">";
						s += this.Value;
						s += string.Format("</{0}>", this.TagName);
					} 
					else 
                    {
                        s += "/>";
                    }
                }
                return s;
			}
		}
		#endregion

		public XmlNode Root
		{
			get
			{
				XmlNode nd = this;
				while (nd.Parent != null)
				{
					nd = nd.Parent;
				}
				return nd;
			}
		}

		public XmlNode NextSubling
		{
			get
			{
				if (this.Parent == null)
				{
					return null;
				}
				var it = this.Parent.Children.GetEnumerator();
				while ( it.MoveNext() ) {
					if ( it.Current == this ) {
						if ( it.MoveNext() ) {
							return it.Current;
						} else {
							return null;
						}
					}
				}
				return null;
			}
		}

#region empty object
		protected static XmlNode _empty = new XmlNode("#empty");
		public static XmlNode Empty {
			get { return _empty ; }
		}
		public bool IsEmpty() {
			return XmlNode.Empty.Equals( this );
		}
#endregion

		#region sweet operator 
		public static XmlNode operator / ( XmlNode node, string tag ) 
		{
			var nd = node.Children.Find( n => n.TagName == tag ) ;
			return nd ?? XmlNode.Empty ;
		}
		public static implicit operator string(XmlNode node)
		{
			return node.Value;
		}
		public static string operator %(XmlNode node, string key)
		{
			return node.Attrs[key];
		}

#if false
		public static implicit operator XmlNode(IEnumerable<object> lst)
		{
			return lst.FirstOrDefault() ?? XmlNode.Empty;
		}
#endif
		#endregion
	}

	/// <summary>
	/// XML node collection
	/// </summary>
	public class XmlNodeCollection : List<XmlNode> {
		public XmlNode Parent { get; set; }
		public new XmlNode Add(XmlNode nd) 
		{
			base.Add(nd);
			nd.Parent = this.Parent;
			return nd;
		}

	}

	/// <summary>
	/// sample class
	/// </summary>
	class Test
	{
		public void test1()
		{
			var root = new XmlNode("root")
				.Node("person")
				.Node("name", "masuda")
				.Root;

			var root2 = new XmlNode("root")
				.Node("person")
				.AddNode("name", "masuda")
				.AddNode("age", "44")
				.AddNode("address", "itabashi-ku")
				.Root;

			var root3 = new XmlNode("root")
				.Node("person")
				.AddAttr("name", "masuda")
				.AddAttr("age", "44")
				.SetValue("masuda tomoaki")
				.Root;


			var root4 = new XmlNode("persons")
				.Node("person").AddAttr("id", "1")
					.AddNode("name", "masuda tomoaki")
					.AddNode("age", "44")
				.Parent
				.Node("person").AddAttr("id", "2")
					.AddNode("name", "yamada taro")
					.AddNode("age", "20")
				.Root;
		}
	}
}
