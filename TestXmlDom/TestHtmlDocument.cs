using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonmile.HtmlDom;

namespace TestXmlDom
{
	[TestClass]
	public class TestHtmlDocument
	{
		[TestMethod]
		public void TestLoad()
		{
			string html = @"<body>test</body>";
			HtmlDocument doc = new HtmlDocument(html);

			Assert.IsNotNull(doc.documentElement);
			Assert.AreEqual("body", doc.documentElement.TagName);
			Assert.AreEqual("test", doc.documentElement.Value);
		}

		[TestMethod]
		public void TestLoad2()
		{
			string html = @"<body><h1>title</h1></body>";
			HtmlDocument doc = new HtmlDocument(html);

			Assert.IsNotNull(doc.documentElement);
			Assert.AreEqual("body", doc.documentElement.TagName);
			Assert.AreEqual("", doc.documentElement.Value);
			HtmlNode root = doc.documentElement;
			HtmlNode el = root.Children;	// auto cast
			Assert.AreEqual("h1", el.TagName);
			Assert.AreEqual("title", el.Value);
		}

		[TestMethod]
		public void TestLoad3()
		{
			string html = @"<body><h1>title</h1>message</body>";
			HtmlDocument doc = new HtmlDocument(html);

			Assert.IsNotNull(doc.documentElement);
			Assert.AreEqual("body", doc.documentElement.TagName);
			Assert.AreEqual("", doc.documentElement.Value);
			HtmlNode root = doc.documentElement;
			Assert.AreEqual(2, root.Children.Count);
			HtmlNode el = root.Children;	// auto cast
			Assert.AreEqual("h1", el.TagName);
			Assert.AreEqual("title", el.Value);
			el = root.Children[1];
			Assert.AreEqual("#text", el.TagName);
			Assert.AreEqual("message", el.Value);
		}

		[TestMethod]
		public void TestHtml1()
		{
			string html = @"<body><h1>title</h1>message</body>";
			HtmlDocument doc = new HtmlDocument(html);

			Assert.AreEqual(html, doc.Html);
		}
#if false
		[TestMethod]
		public void TestHtml2()
		{
			string html = @"<body><h1>title</h1>message</body>";
			HtmlDocument doc = new HtmlDocument(html);
			Assert.AreEqual(html, doc.Html);
			doc.Where(el => el.TagName == "h1").Update(el => el.Value = "TITLE");
			Assert.AreEqual(@"<body><h1>TITLE</h1>message</body>", doc.Html);

			doc.Where(el => el.Value == "message").Update(el => el.Value = "MESSAGE");
			Assert.AreEqual(@"<body><h1>TITLE</h1>MESSAGE</body>", doc.Html);

		}
#endif
		[TestMethod]
		public void TestParse1()
		{
			string html = @"
<body>
  <h1><h2>title</h1></h2>
</body>";
			HtmlDocument doc = new HtmlDocument(html);
			Assert.AreEqual(@"<body><h1><h2>title</h2></h1></body>", doc.Html);
		}

		[TestMethod]
		public void TestParse2()
		{
			string html = @"
<body>
  <ul>
    <li>item1
    <li>item2
    <li>item3
  </ul>
</body>";
			HtmlDocument doc = new HtmlDocument(html);
			Assert.AreEqual(@"<body><ul><li>item1</li><li>item2</li><li>item3</li></ul></body>", doc.Html);
		}

		[TestMethod]
		public void TestParse3()
		{
			// a pattern of no body tag
			string html = @"
<h1>title</h1>
<div id='m1'>message</div>";
			HtmlDocument doc = new HtmlDocument(html);
			Assert.AreEqual("<body><h1>title</h1><div id=\"m1\">message</div></body>", doc.Html);
		}

		[TestMethod]
		public void TestParseComment()
		{
			// test pattern has a comment.
			// convert to new comment tag.
			string html = @"
<body>
 <h1>title</h1>
 <!-- comment -->
 <div id='m1'>message</div>
</body>";
			HtmlDocument doc = new HtmlDocument(html);
			Assert.AreEqual("<body><h1>title</h1><comment>comment</comment><div id=\"m1\">message</div></body>", doc.Html);
		}

		[TestMethod]
		public void TestParseComment2()
		{
			// test pattern has a comment tag.
			// convert to xml refenrece character
			string html = @"
<body>
 <h1>title</h1>
 <!-- <div>comment</div> -->
 <div id='m1'>message</div>
</body>";
			HtmlDocument doc = new HtmlDocument(html);
			Assert.AreEqual("<body><h1>title</h1><comment>&lt;div&gt;comment&lt;/div&gt;</comment><div id=\"m1\">message</div></body>", doc.Html);
		}
	}
}
