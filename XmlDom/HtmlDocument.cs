using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using mshtml;

namespace Moonmile.HtmlDom
{
	public class HtmlDocument : HtmlNode
	{
		public HtmlNode documentElement { 
			get
			{
				if (this.Children.Count() > 0)
				{
					return Children[0];
				}
				else
				{
					return HtmlNode.Empty;
				}
			}
		}

		public new string Html
		{
			get	{ return this.documentElement.Html;	}
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public HtmlDocument() 
		{
		}

		/// <summary>
		/// コンストラクタ（Html文字列で初期化）
		/// </summary>
		/// <param name="html"></param>
		public HtmlDocument( string html)
		{
			this.LoadHtml(html);
		}

		/// <summary>
		/// ファイルから構築する
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public HtmlDocument Load(string path)
		{
			// return this.Load(XDocument.Load(path));
			return this;
		}

		/// <summary>
		/// Loading method
		/// HtmlDocument to create a HTML string
		/// </summary>
		/// <param name="html">HTML string</param>
		/// <returns></returns>
		public HtmlDocument LoadHtml(string html)
		{
			// Creating an object using a mshtml.HTMLDocument
			var doc = new HTMLDocument() as IHTMLDocument2;
			doc.write(new object[] { html });
			Load(doc);
			return this;
		}

		/// <summary>
		/// convert to HtmlDocument from IHTMLDocument2 object
		/// </summary>
		/// <param name="doc"></param>
		/// <returns></returns>
		protected HtmlDocument Load(IHTMLDocument2 doc)
		{
			// root element must be BODY tag.
			HtmlNode root = LoadHtmlNode((IHTMLDOMNode)doc.body);
			this.Children.Add(root);
			return this;
		}

		/// <summary>
		/// IHTMLDOMNode から HtmlNode を作成
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		protected HtmlNode LoadHtmlNode(IHTMLDOMNode node)
		{
			var nn = new HtmlNode(node.nodeName, node.nodeValue.ToString());
			if (nn.TagName == "#text")
				return nn;
			if (nn.TagName == "#comment")
			{
				// append comment tag.
				nn.TagName = "comment";
				string v = System.Web.HttpUtility.HtmlEncode(nn.Value);
				nn.Children.Add(new HtmlNode("#text", v));
				nn.Value = v;
				return nn;
			}

			// append attributes
			IHTMLAttributeCollection attrs = node.attributes;
			if (attrs != null)
			{
				foreach (IHTMLDOMAttribute at in attrs)
				{
					if (at.specified)
					{
						string nodeValue = "";
						if (at.nodeValue != null)
							nodeValue = at.nodeValue.ToString();
						nn.Attrs.Add(new HtmlAttr { Key = at.nodeName, Value = nodeValue });
					}
				}
			}

			var col = node.childNodes as IHTMLDOMChildrenCollection;
			if (col != null)
			{
				foreach (IHTMLDOMNode nd in col)
				{
					HtmlNode el = LoadHtmlNode(nd);
					el.Parent = nn;
					nn.Children.Add(el);
				}
				if (nn.Children.Count > 0 && nn.Children[0].TagName == "#text")
					nn.Value = nn.Children[0].Value;
				if (nn.Children.Count > 0 && nn.Children[0].TagName == "#comment")
					nn.Value = nn.Children[0].Value;
			}
			return nn;
		}
	}
}
