using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonmile.XmlDom;
using System.Xml.Linq;

namespace TestXmlDom
{
	[TestClass]
	public class TestXNavi
	{
		[TestMethod]
		public void TestNormal1()
		{
			XDocument doc = new XDocument(
				new XElement("root",
					new XElement("person",
						new XElement("name", "masuda"))));

			var q = new XNavigator(doc.FirstNode)
				.Where(n => n.TagName() == "name")
				.FirstOrDefault();
			Assert.AreEqual("masuda", q.Value());

		}
		
		[TestMethod]
		public void TestNormal2()
		{
			XDocument doc = new XDocument(
				new XElement("root",
					new XElement("person",
						new XElement("name", "masuda"),
						new XElement("name", "yamada"),
						new XElement("name", "yamasaki"))));

			// タグが検索できた場合
			var q = new XNavigator(doc.FirstNode)
				.Where(n => n.TagName() == "name")
				.Select( n => n );

			Assert.AreEqual(3, q.Count());
			Assert.AreEqual("masuda", q.First().Value());
		}

		[TestMethod]
		public void TestNormal3()
		{
			XDocument doc = new XDocument(
				new XElement("root",
					new XElement("person",
						new XAttribute("id", "1"),
						new XElement("name", "masuda"),
						new XElement("age","44")),
					new XElement("person",
						new XAttribute("id", "2"),
						new XElement("name", "yamada"),
						new XElement("age","20")),
					new XElement("person",
						new XAttribute("id", "3"),
						new XElement("name", "tanaka"),
						new XElement("age","10"))));

			// タグが検索できた場合
			var q = new XNavigator(doc.FirstNode)
				.Where(n => n.Attrs("id") == "2")
				.FirstOrDefault();
			Assert.AreEqual("person", q.TagName());
			// 拡張メソッドを利用
			Assert.AreEqual("yamada", q.Child("name").Value());
			Assert.AreEqual("20", q.Child("age").Value());
		}
		
		[TestMethod]
		public void TestNormal4()
		{
			XDocument doc = new XDocument(
				new XElement("root",
					new XElement("person",
						new XAttribute("id", "1"),
						new XElement("name", "masuda"),
						new XElement("age","44")),
					new XElement("person",
						new XAttribute("id", "2"),
						new XElement("name", "yamada"),
						new XElement("age","20")),
					new XElement("person",
						new XAttribute("id", "3"),
						new XElement("name", "tanaka"),
						new XElement("age","10"))));


			// クエリ文にしてみる
			var q = from n in new XNavigator(doc.FirstNode)
					where n.Attrs("id") == "2"
					select n;

			Assert.AreEqual("person", q.First().TagName());
			// 拡張メソッドを利用
			Assert.AreEqual("yamada", q.First().Child("name").Value());
			Assert.AreEqual("20", q.First().Child("age").Value());
		}

	}


}
