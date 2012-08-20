using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonmile.XmlDom;

namespace TestXmlDom
{
	[TestClass]
	public class TestXmlDocument
	{
		[TestMethod]
		public void TestParse1()
		{
			string xml = "<root><name>masuda tomoaki</name></root>";
			var doc = new XmlDocument(xml);

			var q = new XmlNavigator(doc)
				.Where(n => n.TagName == "name")
				.FirstOrDefault();

			Assert.AreEqual("masuda tomoaki", q.Value);
		}

		[TestMethod]
		public void TestParse2()
		{
			string xml = @"
<root>
 <name>masuda</name>
 <name>yamada</name>
 <name>yamasaki</name>
</root>";
			var doc = new XmlDocument(xml);
			var q = new XmlNavigator(doc)
				.Where(n => n.TagName == "name")
				;
			
			Assert.AreEqual(3, q.Count());
			Assert.AreEqual("masuda", q.First().Value);
		}

		[TestMethod]
		public void TestParse3()
		{
			string xml = @"
<root>
 <person id='1'>
  <name>masuda</name>
  <age>44</age>
 </person>
 <person id='2'>
  <name>yamada</name>
  <age>20</age>
 </person>
 <person id='3'>
  <name>tanaka</name>
  <age>10</age>
 </person>
</root>";
			var doc = new XmlDocument(xml);
			var q = new XmlNavigator(doc)
				.Where(n => n.Attrs["id"] == "2")
				.FirstOrDefault();
			Assert.AreEqual("person", q.TagName);
			// ExDoc記述
			Assert.AreEqual("yamada", q / "name");
			Assert.AreEqual("20", q / "age");
		}
	}
}
