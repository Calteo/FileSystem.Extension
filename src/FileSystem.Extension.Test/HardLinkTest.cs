using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileSystem.Extension.Test
{
    [TestClass]
    public class HardLinkTest
    {
        [TestMethod]
        public void TestCreate()
        {
            // Arrange
            const string filenameExisiting = "original.txt";
            const string filename = "hardlink.txt";

            using (var writer = new StreamWriter(filenameExisiting, false, Encoding.Default))
            {
                writer.WriteLine("test file for hardlink test");
            }

            if (File.Exists(filename))
                File.Delete(filename);

            // Act
            HardLink.Create(filename, filenameExisiting);

            // Assert
            Assert.IsTrue(File.Exists(filename), "hardlink not created");
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException), "missing source not detected")]
        public void TestSourceException()
        {
            // Arrange
            const string filenameExisiting = "original.txt";
            const string filename = "hardlink.txt";

            if (File.Exists(filenameExisiting))
                File.Delete(filenameExisiting);

            // Act
            HardLink.Create(filename, filenameExisiting);

            // Assert            
        }

        [TestMethod]
        [ExpectedException(typeof(Win32Exception), "existing target not detected")]
        public void TestTargetException()
        {
            // Arrange
            const string filenameExisiting = "original.txt";
            const string filename = "hardlink.txt";

            using (var writer = new StreamWriter(filenameExisiting, false, Encoding.Default))
            {
                writer.WriteLine("test file for hardlink test");
            }
            using (var writer = new StreamWriter(filename, false, Encoding.Default))
            {
                writer.WriteLine("test file for blocking hardlink");
            }

            // Act
            HardLink.Create(filename, filenameExisiting);

            // Assert            
        }

        [TestMethod]
        public void TestFileContent()
        {
            // Arrange
            const string filenameExisiting = "original.txt";
            const string filename = "hardlink.txt";

            const string content = "test file for hardlink test";

            using (var writer = new StreamWriter(filenameExisiting, false, Encoding.Default))
            {
                writer.WriteLine(content);
            }

            if (File.Exists(filename))
                File.Delete(filename);

            // Act
            HardLink.Create(filename, filenameExisiting);

            string text;
            using (var reader = new StreamReader(filename, Encoding.Default))
            {
                text = reader.ReadLine();
            }

            // Assert
            Assert.AreEqual(content, text, "file content differs");
        }

        [TestMethod]
        public void TestEnumerate()
        {
            // Arrange
            const string filenameExisiting = "original.txt";
            const string filename = "hardlink.txt";

            using (var writer = new StreamWriter(filenameExisiting, false, Encoding.Default))
            {
                writer.WriteLine("test file for hardlink test");
            }

            if (File.Exists(filename))
                File.Delete(filename);

            HardLink.Create(filename, filenameExisiting);

            // Act
            var files = HardLink.Enumerate(filenameExisiting)
                .Select(Path.GetFileName)
                .ToArray();

            // Assert
            Assert.AreEqual(files.Length, 2, "number of files not correct");
            Assert.IsTrue(files.Contains(filenameExisiting), "source file missing");
            Assert.IsTrue(files.Contains(filename), "target file missing");
        }

        [TestMethod]
        public void TestGet()
        {
            // Arrange
            const string filenameExisiting = "original.txt";
            const string filename = "hardlink.txt";

            using (var writer = new StreamWriter(filenameExisiting, false, Encoding.Default))
            {
                writer.WriteLine("test file for hardlink test");
            }

            if (File.Exists(filename))
                File.Delete(filename);

            HardLink.Create(filename, filenameExisiting);

            // Act
            var files = HardLink.GetLinks(filenameExisiting)
                .Select(Path.GetFileName)
                .ToArray();

            // Assert
            Assert.AreEqual(files.Length, 2, "number of files not correct");
            Assert.IsTrue(files.Contains(filenameExisiting), "source file missing");
            Assert.IsTrue(files.Contains(filename), "target file missing");
        }



        [TestMethod]
        public void TestEnumerateWithOutSelf()
        {
            // Arrange
            const string filenameExisiting = "original.txt";
            const string filename = "hardlink.txt";

            using (var writer = new StreamWriter(filenameExisiting, false, Encoding.Default))
            {
                writer.WriteLine("test file for hardlink test");
            }

            if (File.Exists(filename))
                File.Delete(filename);

            HardLink.Create(filename, filenameExisiting);

            // Act
            var files = HardLink.Enumerate(filenameExisiting, false)
                .Select(Path.GetFileName)
                .ToArray();

            // Assert
            Assert.AreEqual(files.Length, 1, "number of files not correct");
            Assert.IsTrue(files.Contains(filename), "target file missing");
        }
    }
}
