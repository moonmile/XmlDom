using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
namespace Moonmile.XmlDom
{
	public class XmlDocument : XmlNode
	{
		public XmlNode documentElement { 
			get
			{
				if (this.Children.Count() > 0)
				{
					return Children[0];
				}
				else
				{
					return XmlNode.Empty;
				}
			}
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public XmlDocument() 
		{
		}

		/// <summary>
		/// コンストラクタ（XML文字列で初期化）
		/// </summary>
		/// <param name="xml"></param>
		public XmlDocument( string xml)
		{
			this.LoadXml(xml);
		}

		/// <summary>
		/// ファイルから構築する
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public XmlDocument Load(string path)
		{
			return this.Load(XDocument.Load(path));
		}

		/// <summary>
		/// XML文字列から構築する
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		public XmlDocument LoadXml(string xml)
		{
			StringReader sr = new StringReader(xml);
			return this.Load(XDocument.Load(sr));
		}

		/// <summary>
		/// XDocumentオブジェクトから構築する
		/// </summary>
		/// <param name="doc"></param>
		/// <returns></returns>
		public XmlDocument Load(XDocument doc)
		{
			var root = LoadXNode(doc.FirstNode);
			this.Children.Add(root);
			return this;
		}

		protected XmlNode LoadXNode(XNode node)
		{
			var nn = new XmlNode(node.TagName(), node.Value());
			if (node.NodeType == System.Xml.XmlNodeType.Element)
			{
				var el = node as XElement;
				foreach (var at in el.Attributes())
				{
					nn.Attrs.Add(new XmlAttr(at.Name.ToString(), at.Value));
				}
				foreach (var n in el.Nodes())
				{
					nn.Children.Add(LoadXNode(n));
				}
			}
			return nn;
		}
	}
}
