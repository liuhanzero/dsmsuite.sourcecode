﻿using DsmSuite.DsmViewer.Model.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DsmSuite.DsmViewer.Model.Test.Core
{
    [TestClass]
    public class ElementNameTest
    {
        [TestMethod]
        public void ElementNameDefaultConstructorTest()
        {
            ElementName elementName = new ElementName();
            Assert.AreEqual("", elementName.FullName);
            Assert.AreEqual("", elementName.ParentName);
            Assert.AreEqual("", elementName.LastNamePart);
            Assert.AreEqual(1, elementName.NamePartCount);
        }

        [TestMethod]
        public void ElementNameConstructorWithFullnameTest()
        {
            ElementName elementName = new ElementName("a.b.c");
            Assert.AreEqual("a.b.c", elementName.FullName);
            Assert.AreEqual("a.b", elementName.ParentName);
            Assert.AreEqual("c", elementName.LastNamePart);
            Assert.AreEqual(3, elementName.NamePartCount);
            Assert.AreEqual("a", elementName.NameParts[0]);
            Assert.AreEqual("b", elementName.NameParts[1]);
            Assert.AreEqual("c", elementName.NameParts[2]);
        }

        [TestMethod]
        public void ElementNameConstructorWithParentAndPartNameTest()
        {
            ElementName elementName = new ElementName("a.b", "c");
            Assert.AreEqual("a.b.c", elementName.FullName);
            Assert.AreEqual("a.b", elementName.ParentName);
            Assert.AreEqual("c", elementName.LastNamePart);
            Assert.AreEqual(3, elementName.NamePartCount);
            Assert.AreEqual("a", elementName.NameParts[0]);
            Assert.AreEqual("b", elementName.NameParts[1]);
            Assert.AreEqual("c", elementName.NameParts[2]);
        }

        [TestMethod]
        public void ElementNameAddPartToEmptyElementNameTest()
        {
            ElementName elementName = new ElementName();
            elementName.AddNamePart("a");
            Assert.AreEqual("a", elementName.FullName);
            Assert.AreEqual("", elementName.ParentName);
            Assert.AreEqual("a", elementName.LastNamePart);
            Assert.AreEqual(1, elementName.NamePartCount);
            Assert.AreEqual("a", elementName.NameParts[0]);
        }

        [TestMethod]
        public void ElementNameAddPartToNonEmptyElementNameTest()
        {
            ElementName elementName = new ElementName("a.b");
            elementName.AddNamePart("c");
            Assert.AreEqual("a.b.c", elementName.FullName);
            Assert.AreEqual("a.b", elementName.ParentName);
            Assert.AreEqual("c", elementName.LastNamePart);
            Assert.AreEqual(3, elementName.NamePartCount);
            Assert.AreEqual("a", elementName.NameParts[0]);
            Assert.AreEqual("b", elementName.NameParts[1]);
            Assert.AreEqual("c", elementName.NameParts[2]);
        }
    }
}
