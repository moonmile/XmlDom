using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moonmile.HtmlDom
{
	/// <summary>
	/// HtmlNode をツリー構造でサーチするクラス
	/// </summary>
	public class HtmlNavigator : IEnumerable<HtmlNode>
	{
		HtmlNode _root;

		public HtmlNavigator(HtmlNode root)
		{
			_root = root;
		}
		public HtmlNavigator(HtmlDocument doc)
		{
			_root = doc.documentElement;
		}

		public IEnumerator<HtmlNode> GetEnumerator()
		{
			return new Enumerator(_root);
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return new Enumerator(_root);
		}

		protected class Enumerator : IEnumerator<HtmlNode>
		{
			HtmlNode _root;
			HtmlNode _cur;

			public Enumerator(HtmlNode root)
			{
				_root = root;
				_cur = null;
			}


			public HtmlNode Current
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
				HtmlNode cur = _cur;
				while (true)
				{
					HtmlNode pa1 = cur.Parent;
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

	/// <summary>
	/// IEnumerable<HtmlNode> 用の拡張クラス
	/// </summary>
	public static class IEnumableExtentsions
	{
		/// <summary>
		/// Update メソッド
		/// </summary>
		/// <param name="lst"></param>
		/// <param name="func"></param>
		/// <returns></returns>
		public static IEnumerable<HtmlNode> Update(this IEnumerable<HtmlNode> lst, Func<HtmlNode, string> func)
		{
			foreach (var el in lst)
			{
				func(el);
			}
			return lst;
		}

		/// <summary>
		/// Remove メソッド
		/// </summary>
		/// <param name="lst"></param>
		/// <param name="func"></param>
		/// <returns></returns>
		public static IEnumerable<HtmlNode> Remove(this IEnumerable<HtmlNode> lst)
		{
			foreach (var el in lst)
			{
				el.Parent.Children.Remove(el);
			}
			return lst;
		}
		/// <summary>
		/// AppendChild メソッド
		/// </summary>
		/// <param name="lst"></param>
		/// <param name="func"></param>
		/// <returns></returns>
		public static IEnumerable<HtmlNode> AppendChild(this IEnumerable<HtmlNode> lst, HtmlNode node )
		{
			// 最初の要素に追加する
			foreach (var el in lst)
			{
				el.Children.Add(node);
				node.Parent = el;
				break;
			}
			return lst;
		}
		
		/// <summary>
		/// 指定位置の要素を取得する簡易メソッド
		/// </summary>
		/// <param name="lst"></param>
		/// <param name="n"></param>
		/// <returns></returns>
		public static HtmlNode Item(this IEnumerable<HtmlNode> lst, int n = 0)
		{
			var l = lst.ToList();
			if (n < l.Count)
			{
				return l[n];
			}
			else
			{
				return HtmlNode.Empty;
			}
		}
	}
}
