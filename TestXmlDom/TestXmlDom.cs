using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonmile.XmlDom;

namespace TestXmlDom
{
	/// <summary>
	/// UnitTest1 の概要の説明
	/// </summary>
	[TestClass]
	public class TestXmlDom
	{
		public TestXmlDom()
		{
			//
			// TODO: コンストラクター ロジックをここに追加します
			//
		}

		private TestContext testContextInstance;

		/// <summary>
		///現在のテストの実行についての情報および機能を
		///提供するテスト コンテキストを取得または設定します。
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region 追加のテスト属性
		//
		// テストを作成する際には、次の追加属性を使用できます:
		//
		// クラス内で最初のテストを実行する前に、ClassInitialize を使用してコードを実行してください
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// クラス内のテストをすべて実行したら、ClassCleanup を使用してコードを実行してください
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// 各テストを実行する前に、TestInitialize を使用してコードを実行してください
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// 各テストを実行した後に、TestCleanup を使用してコードを実行してください
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		[TestMethod]
		public void TestNormal1()
		{
			var root = new XmlNode("root")
				.Node("person")
				.Node("name", "masuda")
				.Root;

			Assert.AreEqual("<root><person><name>masuda</name></person></root>", root.Xml);
		}

		[TestMethod]
		public void TestNormal2()
		{
			var root = new XmlNode("root")
				.Node("person")
				.AddNode("name", "masuda")
				.AddNode("age", "44")
				.AddNode("address", "itabashi-ku")
				.Root;

			Assert.AreEqual("<root><person><name>masuda</name><age>44</age><address>itabashi-ku</address></person></root>", root.Xml);
		}

		[TestMethod]
		public void TestNormal3()
		{
			var root = new XmlNode("root")
				.Node("person")
				.AddAttr("name", "masuda")
				.AddAttr("age", "44")
				.SetValue("masuda tomoaki")
				.Root;

			Assert.AreEqual("<root><person name=\"masuda\" age=\"44\">masuda tomoaki</person></root>", root.Xml);
		}

		[TestMethod]
		public void TestNormal4()
		{
			var root = new XmlNode("persons")
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
				root.Xml);
		}
	
	}
}
