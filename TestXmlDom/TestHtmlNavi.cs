using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonmile.HtmlDom;

namespace TestXmlDom
{
	[TestClass]
	public class TestHtmlNavi
	{
        [TestMethod]
        public void TestTagName()
        {
            string html = @"<body><h1>title</h1>message</body>";
			HtmlDocument doc = new HtmlDocument(html);

			var q = new HtmlNavigator( doc )
				.Where( n => n.TagName == "h1" ) 
				.Select( n => n );
            Assert.AreEqual(1, q.Count());
            Assert.AreEqual("title", q.First().Value );
        }

		[TestMethod]
        public void TestTagName2()
        {
            string html = @"<body><h2>title1</h2>message<h2>title2</h2></body>";
            HtmlDocument doc = new HtmlDocument(html);

			var q = new HtmlNavigator(doc)
				.Where(n => n.TagName == "h2")
				.Select(n => n);

            Assert.AreEqual(2, q.Count());
            Assert.AreEqual("title1", q.First().Value);
            Assert.AreEqual("title2", q.ToList()[1].Value);
		}

        [TestMethod]
        public void TestTagName3()
        {
            string html = @"
<body>
    <h2>title1</h2>
        <span>message</span>
    <h2>title2</h2>
        <span>message2</span>
</body>
";
            HtmlDocument doc = new HtmlDocument(html);

			var q = new HtmlNavigator(doc)
				.Where(n => n.TagName == "span")
				.Select(n => n);

            Assert.AreEqual(2, q.Count());
            Assert.AreEqual("message", q.ToList()[0].Value);
            Assert.AreEqual("message2", q.ToList()[1].Value);
		}

		[TestMethod]
		public void TestTagName4()
		{
			string html = @"
<body>
    <h2>title1</h2>
        <span id='msg1'>message</span>
    <h2>title2</h2>
        <span id='msg2'>message2</span>
</body>
";
			HtmlDocument doc = new HtmlDocument(html);

			var q = new HtmlNavigator(doc)
				.Where(n => n % "id" == "msg2")
				.Select(n => n);

			Assert.AreEqual(1, q.Count());
			Assert.AreEqual("message2", q.First().Value);
		}


        [TestMethod]
        public void TestUpdate1()
        {
            string html = @"
<body>
    <h2>title1</h2>
        <span id='m1'>message</span>
    <h2>title2</h2>
        <span id='m2'>message2</span>
</body>
";
            HtmlDocument doc = new HtmlDocument(html);
			var q = new HtmlNavigator(doc)
				.Where(n => n % "id" == "m2")
				.Update(n => n.Value = "new message");

			q = new HtmlNavigator(doc)
				.Where(n => n.TagName == "span")
				.Select(n => n);

            Assert.AreEqual(2, q.Count());
            Assert.AreEqual("message", q.Item(0).Value);
            Assert.AreEqual("new message", q.Item(1).Value);
		}

        [TestMethod]
        public void TestRemove1()
        {
            string html = @"
<body>
    <h2>title1</h2>
    <span id='m1'>message</span>
    <h2>title2</h2>
    <span id='m2'>message2</span>
</body>
";
            HtmlDocument doc = new HtmlDocument(html);
			var q = new HtmlNavigator(doc)
				.Where(n => n % "id" == "m2")
				.Remove();

			q = new HtmlNavigator(doc)
				.Where(n => n.TagName == "span")
				.Select(n => n);

            Assert.AreEqual(1, q.Count());
            Assert.AreEqual("message", q.Item().Value);
		}

#if false
		[TestMethod]
        public void TestRemove2()
        {
            string html = @"
<body>
    <h2>title1</h2>
    <span id='m1'>message</span>
    <h2>title2</h2>
    <span id='m2'>message2</span>
</body>
";
            HtmlDocument doc = new HtmlDocument(html);
            var el = doc.Where(n => n.TagName == "span");
            doc.Remove( el );
            Assert.AreEqual(@"<body><h2>title1</h2><h2>title2</h2></body>", doc.Html );
		}
#endif
        [TestMethod]
        public void TestInsert1()
        {
            string html = @"
<body>
    <h2>title1</h2>
    <div id='m1'></div>
    <h2>title2</h2>
    <div id='m2'></div>
</body>
";
            HtmlDocument doc = new HtmlDocument(html);
			var q = new HtmlNavigator(doc)
				.Where(n => n % "id" == "m1")
				.AppendChild(new HtmlNode("p", "new message"));
			
            
            Assert.AreEqual("<body><h2>title1</h2><div id=\"m1\"><p>new message</p></div><h2>title2</h2><div id=\"m2\"/></body>", doc.Html );
        }
	}
}
