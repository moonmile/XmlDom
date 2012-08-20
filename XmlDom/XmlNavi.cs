using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moonmile.XmlDom
{
	/// <summary>
	/// XmlNode をツリー構造でサーチするクラス
	/// </summary>
	public class XmlNavigator : IEnumerable<XmlNode>
	{
		XmlNode _root;

		public XmlNavigator(XmlNode root)
		{
			_root = root;
		}

		public IEnumerator<XmlNode> GetEnumerator()
		{
			return new Enumerator(_root);
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return new Enumerator(_root);
		}

		protected class Enumerator : IEnumerator<XmlNode>
		{
			XmlNode _root;
			XmlNode _cur;

			public Enumerator(XmlNode root)
			{
				_root = root;
				_cur = null;
			}


			public XmlNode Current
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
				if (_cur.Children.Count > 0)
				{
					_cur = _cur.Children[0];
					return true;
				}
				if (_cur.NextSubling != null)
				{
					_cur = _cur.NextSubling;
					return true;
				}
				XmlNode cur = _cur;
				while (true)
				{
					XmlNode pa1 = cur.Parent;
					if (pa1 == null)
					{
						_cur = null;
						return false;
					}
					if (pa1.NextSubling != null)
					{
						_cur = pa1.NextSubling;
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


}
