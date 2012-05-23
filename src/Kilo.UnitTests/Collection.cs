using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;

namespace Kilo.UnitTests
{
	[TestClass]
	public class Collection
	{
		[TestMethod]
		public void Can_glue_IEnumerable()
		{
			IEnumerable<int> enumerable = new int[] { 1, 2, 3, 4, 5 };

			string result = Kilo.Collections.GlueContents(enumerable, ",");

			Assert.IsNotNull(result);
			Assert.AreEqual("1,2,3,4,5", result);
		}

		[TestMethod]
		public void Can_glue_with_last_element()
		{
			IEnumerable<int> enumerable = new int[] { 1, 2, 3, 4, 5 };

			string result = Kilo.Collections.GlueContents(enumerable, ", ", " and ");

			Assert.IsNotNull(result);
			Assert.AreEqual("1, 2, 3, 4 and 5", result);
		}

		[TestMethod]
		public void Can_glue_with_selector()
		{
			IEnumerable<string> enumerable = new string[] { "one", "two", "three", "four" };

			string result = Kilo.Collections.GlueContents(enumerable, " ", selector: e => e.Length);

			Assert.IsNotNull(result);
			Assert.AreEqual("3 3 5 4", result);
		}

		[TestMethod]
		public void Can_glue_name_value_collection()
		{
			NameValueCollection nvc = new NameValueCollection();
			nvc["first"] = "one";
			nvc["second"] = "two";
			nvc["third"] = "three";

			string result = Kilo.Collections.GlueContents(nvc, "&");

			Assert.IsNotNull(result);
			Assert.AreEqual("first=one&second=two&third=three", result);
		}

		[TestMethod]
		public void Can_glue_dictionary()
		{
			var dictionary = new Dictionary<string, int>();
			dictionary["one"] = 1;
			dictionary["two"] = 2;
			dictionary["three"] = 3;

			string result = Kilo.Collections.GlueContents(dictionary, " + ");

			Assert.IsNotNull(result);
			Assert.AreEqual("one=1 + two=2 + three=3", result);
		}

		[TestMethod]
		public void Can_glue_dictionary_with_selector()
		{
			var dictionary = new Dictionary<string, int>();
			dictionary["one"] = 1;
			dictionary["two"] = 2;
			dictionary["three"] = 3;

			string result = Kilo.Collections.GlueContents(dictionary, " + ", v => v + 100);

			Assert.IsNotNull(result);
			Assert.AreEqual("one=101 + two=102 + three=103", result);
		}

		[TestMethod]
		public void Can_use_for_each()
		{
			var enumerable = Enumerable.Range(0, 5);
			int total = 0;

			Kilo.Collections.ForEach(enumerable, (i) =>
			{
				total++;
			});

			Assert.AreEqual(5, total);
		}
		
		[TestMethod]
		public void Can_get_strong_type_from_NameValueCollection()
		{
			NameValueCollection nvc = new NameValueCollection();
			nvc["one"] = "1";
			nvc["two"] = "2";
			nvc["three"] = "3";

			int result = Kilo.Collections.GetValueAsType<int>(nvc, "two");

			Assert.AreEqual(2, result);
		}

		[TestMethod]
		public void Can_convert_object_to_dictionary()
		{
			var obj = new { id = 1, name = "Test Object" };

			IDictionary<string, object> result = Collections.ConvertObjectToDictionary(obj);

			Assert.AreEqual(2, result.Count);
			Assert.IsTrue(result.ContainsKey("id"));
			Assert.IsTrue(result.ContainsKey("name"));
			Assert.AreEqual(1, result["id"]);
			Assert.AreEqual("Test Object", result["name"]);
		}
	}
}
