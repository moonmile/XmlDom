using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonmile.HtmlDom;

namespace TestXmlDom
{
	[TestClass]
	public class TestHtmlNode
	{
		[TestMethod]
		public void TestNormal1()
		{
			var root = new HtmlNode("root")
				.Node("person")
				.Node("name", "masuda")
				.Root;
			Assert.AreEqual("<root><person><name>masuda</name></person></root>", root.Html);
		}

		[TestMethod]
		public void TestNormal4()
		{
			var root = new HtmlNode("persons")
				.Node("person").AddAttr("id", "1")
					.AddNode("name", "masuda tomoaki")
					.AddNode("age", "44")
				.Parent
				.Node("person").AddAttr("id", "2")
					.AddNode("name", "yamada taro")
					.AddNode("age", "20")
				.Root;
			;

			Assert.AreEqual("<persons>" +
				"<person id=\"1\"><name>masuda tomoaki</name><age>44</age></person>" +
				"<person id=\"2\"><name>yamada taro</name><age>20</age></person>" +
				"</persons>",
				root.Html);
		}

	}
}
