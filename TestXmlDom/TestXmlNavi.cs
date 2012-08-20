using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonmile.XmlDom;

namespace TestXmlDom
{
	[TestClass]
	public class TestXmlNavi
	{
		[TestMethod]
		public void TestNormal()
		{
			var root = new XmlNode("root")
				.Node("person")
				.Node("name", "masuda")
				.Root;

			// タグが検索できた場合
			var q = new XmlNavigator(root)
				.Where(n => n.TagName == "name")
				.FirstOrDefault();
			Assert.AreEqual("masuda", q.Value);

			// タグが見つからなかった場合
			q = new XmlNavigator(root)
				.Where(n => n.TagName == "error")
				.FirstOrDefault();
			Assert.AreEqual(null, q);

		}

		[TestMethod]
		public void TestNormal2()
		{
			var root = new XmlNode("root")
				.Node("person")
				.AddNode("name", "masuda")
				.AddNode("name", "yamada")
				.AddNode("name", "yamasaki")
				.Root;

			// タグが検索できた場合
			var q = new XmlNavigator(root)
				.Where(n => n.TagName == "name")
				;
			Assert.AreEqual(3, q.Count());
			Assert.AreEqual("masuda", q.First().Value);
		}

		[TestMethod]
		public void TestNormal3()
		{
			var root = new XmlNode("root")
				.Node("person").AddAttr("id", "1")
					.AddNode("name", "masuda")
					.AddNode("age", "44")
				.Parent
				.Node("person").AddAttr("id", "2")
					.AddNode("name", "yamada")
					.AddNode("age", "20")
				.Parent
				.Node("person").AddAttr("id", "3")
					.AddNode("name", "tanaka")
					.AddNode("age", "10")
				.Root;

			// タグが検索できた場合
			var q = new XmlNavigator(root)
				.Where(n => n.Attrs["id"] == "2")
				.FirstOrDefault();
			Assert.AreEqual("person", q.TagName);
			// ExDoc記述
			Assert.AreEqual("yamada", q / "name");
			Assert.AreEqual("20", q / "age");
		}

		[TestMethod]
		public void TestNormal4()
		{
			var root = new XmlNode("root")
				.Node("person").AddAttr("id", "1")
					.AddNode("name", "masuda")
					.AddNode("age", "44")
				.Parent
				.Node("person").AddAttr("id", "2")
					.AddNode("name", "yamada")
					.AddNode("age", "20")
				.Parent
				.Node("person").AddAttr("id", "3")
					.AddNode("name", "tanaka")
					.AddNode("age", "10")
				.Root;

			// クエリ文にしてみる
			var q = from n in new XmlNavigator(root)
					where n.Attrs["id"] == "2"
					select n;

			Assert.AreEqual("person", q.First().TagName);
			// ExDoc記述
			Assert.AreEqual("yamada", q.First() / "name");
			Assert.AreEqual("20", q.First() / "age");
		}

		[TestMethod]
		public void TestNormal5()
		{
			var root = new XmlNode("root")
				.Node("person").AddAttr("id", "1")
					.AddNode("name", "masuda")
					.AddNode("age", "44")
				.Parent
				.Node("person").AddAttr("id", "2")
					.AddNode("name", "yamada")
					.AddNode("age", "20")
				.Parent
				.Node("person").AddAttr("id", "3")
					.AddNode("name", "tanaka")
					.AddNode("age", "10")
				.Root;

			// クエリ構文にしてみる
			var nd = from n in new XmlNavigator(root)
					 where n % "id" == "2"
					 select n;
			XmlNode xn = nd.FirstOrDefault();

			// 本来は、以下のようにしたいのだが暗黙キャストが作れない
#if false
			XmlNode xxn = 
				from n in new XmlNavigator(root)
				where n % "id" == "2"
				select n;
#endif
			Assert.AreEqual("person", xn.TagName);
			// ExDoc記述
			Assert.AreEqual("yamada", xn / "name");
			Assert.AreEqual("20", xn / "age");
		}

		[TestMethod]
		public void TestNormal6()
		{
			var root = new XmlNode("root")
				.Node("person").AddAttr("id", "1")
					.AddNode("name", "masuda")
					.AddNode("age", "44")
				.Parent
				.Node("person").AddAttr("id", "2")
					.AddNode("name", "yamada")
					.AddNode("age", "20")
				.Parent
				.Node("person").AddAttr("id", "3")
					.AddNode("name", "tanaka")
					.AddNode("age", "10")
				.Root;

			// クエリメソッドを使う
			var xn = new XmlNavigator(root)
				.Where(n => n % "id" == "2")
				.FirstOrDefault();

			Assert.AreEqual("person", xn.TagName);
			// ExDoc記述
			Assert.AreEqual("yamada", xn / "name");
			Assert.AreEqual("20", xn / "age");
		}
	}


}
