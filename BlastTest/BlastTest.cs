﻿// Copyright (c) 2012 James Telfer, released under the Apache 2.0 license: 
// see http://www.apache.org/licenses/LICENSE-2.0.html.

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;

namespace BlastTests {

    [TestClass]
    public class BlastTest {

        [TestMethod]
        public void basic_decompression_from_example() {
            // setup
            byte[] input = { 0x00, 0x04, 0x82, 0x24, 0x25, 0x8f, 0x80, 0x7f };
            byte[] expected = Encoding.ASCII.GetBytes("AIAIAIAIAIAIA");

            var outp = new MemoryStream();

            // test
            var b = new Blast(new MemoryStream(input, writable: false), outp);
            b.Decompress();
            Console.WriteLine(Encoding.ASCII.GetString(outp.ToArray()));

            // assert
            CollectionAssert.AreEqual(expected, outp.ToArray());
        }

		[TestMethod]
		public void decompress_text_file()
		{
			// setup
			var baseFolder = GetTestFileFolder();

			using (var input = new FileStream(Path.Combine(baseFolder, "test.bin"), FileMode.Open, FileAccess.Read))
			using (var output = new FileStream(Path.Combine(baseFolder, "test.decomp.log"), FileMode.Create, FileAccess.Write))
			{

				// test
				var b = new Blast(input, output);
				b.Decompress();
			}

			// assert
		}

		[TestMethod]
		public void decompress_large_text_file()
		{
			// setup
			var baseFolder = GetTestFileFolder();
            var resultFile = Path.Combine(baseFolder, "large.decomp.log");

			using (var input = new FileStream(Path.Combine(baseFolder, "large.log.cmp"), FileMode.Open, FileAccess.Read))
			using (var output = new FileStream(resultFile, FileMode.Create, FileAccess.Write))
			{

				// test
				var b = new Blast(input, output);
				b.Decompress();
			}

			// assert
            AssertFile(Path.Combine(baseFolder, "large.log"), resultFile);
        }

		[TestMethod]
		public void decompress_binary_file()
		{
			// setup
            var baseFolder = GetTestFileFolder();

            var resultFile = Path.Combine(baseFolder, "blast.decomp.msg");

            using (var input = new FileStream(Path.Combine(baseFolder, "blast.msg.cmp"), FileMode.Open, FileAccess.Read))
			using (var output = new FileStream(resultFile, FileMode.Create, FileAccess.Write))
			{

				// test
				var b = new Blast(input, output);
				b.Decompress();
			}

			// assert
            AssertFile(Path.Combine(baseFolder, "blast.msg"), resultFile);
		}

        private void AssertFile(string expectedFileResult, string actualFileResult)
        {
            Assert.IsTrue(File.Exists(expectedFileResult), "Expected file result must exist");
            Assert.IsTrue(File.Exists(actualFileResult), "Actual file result must exist");

            var exp = new FileInfo(expectedFileResult);
            var act = new FileInfo(actualFileResult);

            Assert.AreEqual(exp.Length, act.Length, "File sizes must match");
        }

		private static string GetTestFileFolder()
		{
            var projDir = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).Parent.Parent.FullName;
            return Path.Combine(projDir, "test-files");
        }
	}
}
