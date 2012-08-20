using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moonmile.HtmlDom
{
	/// <summary>
	/// HTML node class
	/// </summary>
	public class HtmlNode 
	{
		#region property
		protected string _tagName;
		public string TagName
		{
			get { return _tagName; }
			set { _tagName = value.ToLower(); }
		}
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
				if (this.Children != null)
					if (this.Children.Count > 0)
						if (this.Children[0].TagName == "#text")
							this.Children[0].Value = _value;
			}
		}
		public HtmlNodeCollection Children { get; set; }
		public HtmlAttrCollection Attrs { get; set; }
		public HtmlNode Parent { get; set; }
		#endregion

		#region constractor 
		public HtmlNode()
		{
			this.TagName = "#empty";
			this.Value = "";
			this.Children = new HtmlNodeCollection();
			this.Children.Parent = this;
			this.Attrs = new HtmlAttrCollection();
		}

		public HtmlNode(string tag, string value = "")
		{
			this.TagName = tag;
			this.Value = value;
			this.Children = new HtmlNodeCollection();
			this.Children.Parent = this;
			this.Attrs = new HtmlAttrCollection();

			if (this.TagName == "" || this.TagName == "#comment")
				return;
			if (this.TagName != "#text" && value != "")
				this.Children.Add(new HtmlNode("#text", value));
		}
		#endregion

		#region creator for method chain
		public HtmlNode Node(string tag, string value = "")
		{
			var nd = new HtmlNode(tag, value);
			this.Children.Add(nd);
			return nd;
		}
		public HtmlNode AddNode(string tag, string value = "")
		{
			this.Node(tag, value);
			return this;
		}

		public HtmlNode AddAttr(string key, string value)
		{
			this.Attrs.Add(new HtmlAttr(key, value));
			return this;
		}
		public HtmlNode SetValue(string value)
		{
			this.Value = value;
			return this;
		}
		#endregion

		#region Html string serialize
		public string Html
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
						// s += this.Value;
                        foreach (var el in this.Children)
                        {
                            s += el.Html;
                        }
                        s += string.Format("</{0}>", this.TagName);
                    }
					/*
					else if (this.Value != "")
					{
						s += ">";
						s += this.Value;
						s += string.Format("</{0}>", this.TagName);
					}
					*/
					else 
                    {
                        s += "/>";
                    }
                }
                return s;
			}
		}
		#endregion

		public HtmlNode Root
		{
			get
			{
				HtmlNode nd = this;
				while (nd.Parent != null)
				{
					nd = nd.Parent;
				}
				return nd;
			}
		}

		public HtmlNode NextSubling
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
		protected static HtmlNode _empty = new HtmlNode("#empty");
		public static HtmlNode Empty {
			get { return _empty ; }
		}
		public bool IsEmpty() {
			return HtmlNode.Empty.Equals( this );
		}
#endregion

		#region sweet operator 
		public static HtmlNode operator / ( HtmlNode node, string tag ) 
		{
			var nd = node.Children.Find( n => n.TagName == tag ) ;
			return nd ?? HtmlNode.Empty ;
		}
		public static implicit operator string(HtmlNode node)
		{
			return node.Value;
		}
		public static string operator %(HtmlNode node, string key)
		{
			return node.Attrs[key];
		}

#if false
		public static implicit operator HtmlNode(IEnumerable<object> lst)
		{
			return lst.FirstOrDefault() ?? HtmlNode.Empty;
		}
#endif
		#endregion
	}

	/// <summary>
	/// HTML node collection
	/// </summary>
	public class HtmlNodeCollection : List<HtmlNode> {
		public HtmlNode Parent { get; set; }
		public new HtmlNode Add(HtmlNode nd) 
		{
			base.Add(nd);
			nd.Parent = this.Parent;
			return nd;
		}

		/// <summary>
		/// auto convert to one HtmlElement from HtmlElementCollection
		/// </summary>
		/// <param name="col"></param>
		/// <returns></returns>
		public static implicit operator HtmlNode(HtmlNodeCollection col)
		{
			if (col.Count > 0)
			{
				return col[0];
			}
			else
			{
				return HtmlNode.Empty;
			}
		}

		/// <summary>
		/// auto convert to HtmlElementCollection from one HtmlElement
		/// </summary>
		/// <param name="el"></param>
		/// <returns></returns>
		public static implicit operator HtmlNodeCollection(HtmlNode el)
		{
			var col = new HtmlNodeCollection();
			if (!el.IsEmpty())
			{
				col.Add(el);
			}
			return col;
		}
	}
}
