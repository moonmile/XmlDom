using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Moonmile.XmlDom
{
	/// <summary>
	/// XNode をツリー構造でサーチするクラス
	/// </summary>
	public class XNavigator : IEnumerable<XNode>
	{
		XNode _root;

		public XNavigator(XNode root)
		{
			_root = root;
		}

		public IEnumerator<XNode> GetEnumerator()
		{
			return new Enumerator(_root);
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return new Enumerator(_root);
		}

		protected class Enumerator : IEnumerator<XNode>
		{
			XNode _root;
			XNode _cur;

			public Enumerator(XNode root)
			{
				_root = root;
			}

			public XNode Current
			{
				get { return _cur; }
			}

			public void Dispose()
			{
			}

			object System.Collections.IEnumerator.Current
			{
				get { return _cur; }
			}

			public bool MoveNext()
			{
				if (_cur == null)
				{
					_cur = _root;
					return true;
				}
				if (_cur.NodeType == System.Xml.XmlNodeType.Element &&
					((XElement)_cur).Nodes().Count() > 0)
				{
					_cur = ((XElement)_cur).FirstNode;
					return true;
				}
				if (_cur.NextNode != null)
				{
					_cur = _cur.NextNode;
					return true;
				}
				XNode cur = _cur;
				while (true)
				{
					XElement pa1 = cur.Parent;
					if (pa1 == null)
					{
						_cur = null;
						return false;
					}
					if (pa1.NextNode != null)
					{
						_cur = pa1.NextNode;
						return true;
					}
					cur = pa1;
				}
			}

			public void Reset()
			{
				_cur = null;
			}
		}
	}
	/// <summary>
	/// XNodeをXElement風に使う拡張クラス
	/// </summary>
	public static class XNodeExtentions
	{
		public static string TagName(this XNode n)
		{
			if (n.NodeType == System.Xml.XmlNodeType.Element)
			{
				return ((XElement)n).Name.ToString();
			}
			else
			{
				return "";
			}
		}
		public static string Value(this XNode n)
		{
			if (n.NodeType == System.Xml.XmlNodeType.Element)
			{
				return ((XElement)n).Value;
			}
			else
			{
				return "";
			}
		}
		public static string Attrs(this XNode n, string key)
		{
			if (n.NodeType == System.Xml.XmlNodeType.Element)
			{
				var attr = ((XElement)n).Attribute(key);
				if (attr != null)
				{
					return attr.Value;
				}
			}
			return "";
		}
		public static XNode Child(this XNode nd, string tag)
		{
			if (nd.NodeType == System.Xml.XmlNodeType.Element)
			{
				return ((XElement)nd).Nodes().Where(n => n.TagName() == tag).FirstOrDefault();
			}
			else
			{
				return null;
			}

		}

	}

}
