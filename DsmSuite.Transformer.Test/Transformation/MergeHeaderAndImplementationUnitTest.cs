﻿using System.Reflection;
using DsmSuite.Analyzer.Model.Core;
using DsmSuite.Analyzer.Model.Interface;
using DsmSuite.Transformer.Transformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DsmSuite.Transformer.Test.Transformation
{
    [TestClass]
    public class MergeHeaderTransformationUnitTest
    {
        [TestMethod]
        public void MergeWhenImplementationDependsOnHeaderFileWithSameName()
        {
            DsiModel dataModel = new DsiModel("Test", Assembly.GetExecutingAssembly());

            IDsiElement elementCpp = dataModel.AddElement("namespace1.namespace1.element1Name.cpp", "class", "");
            IDsiElement elementH = dataModel.AddElement("namespace3.namespace4.element1Name.h", "class", "");

            dataModel.AddRelation(elementCpp.Name, elementH.Name, "", 1, "context");

            Assert.IsNotNull(dataModel.FindElementByName("namespace1.namespace1.element1Name.cpp"));
            Assert.IsNotNull(dataModel.FindElementByName("namespace3.namespace4.element1Name.h"));
            Assert.IsNull(dataModel.FindElementByName("namespace1.namespace1.element1Name.h"));
            
            MoveHeaderElementsAction transformation = new MoveHeaderElementsAction(dataModel, true,null);
            transformation.Execute();

            Assert.IsNotNull(dataModel.FindElementByName("namespace1.namespace1.element1Name.cpp"));
            Assert.IsNull(dataModel.FindElementByName("namespace3.namespace4.element1Name.h"));
            Assert.IsNotNull(dataModel.FindElementByName("namespace1.namespace1.element1Name.h"));
        }

        [TestMethod]
        public void DoNotMergeWhenImplementationDependsOnHeaderFileWithOtherName()
        {
            DsiModel dataModel = new DsiModel("Test", Assembly.GetExecutingAssembly());

            IDsiElement elementCpp = dataModel.AddElement("namespace1.namespace1.element1Name.cpp", "class", "");
            IDsiElement elementH = dataModel.AddElement("namespace3.namespace4.ELEMENT1NAME.h", "class", "");

            dataModel.AddRelation(elementCpp.Name, elementH.Name, "", 1, "context");

            Assert.IsNotNull(dataModel.FindElementByName("namespace1.namespace1.element1Name.cpp"));
            Assert.IsNotNull(dataModel.FindElementByName("namespace3.namespace4.ELEMENT1NAME.h"));
            Assert.IsNull(dataModel.FindElementByName("namespace1.namespace1.element1Name.h"));

            MoveHeaderElementsAction transformation = new MoveHeaderElementsAction(dataModel, true, null);
            transformation.Execute();

            Assert.IsNotNull(dataModel.FindElementByName("namespace1.namespace1.element1Name.cpp"));
            Assert.IsNotNull(dataModel.FindElementByName("namespace3.namespace4.ELEMENT1NAME.h"));
            Assert.IsNull(dataModel.FindElementByName("namespace1.namespace1.element1Name.h"));
        }

        [TestMethod]
        public void DoNotMergeWhenImplementationDoesNotDependOnHeaderFileWithSameName()
        {
            DsiModel dataModel = new DsiModel("Test", Assembly.GetExecutingAssembly());

            dataModel.AddElement("namespace1.namespace1.element1Name.cpp", "class", "");
            dataModel.AddElement("namespace3.namespace4.element1Name.h", "class", "");

            Assert.IsNotNull(dataModel.FindElementByName("namespace1.namespace1.element1Name.cpp"));
            Assert.IsNotNull(dataModel.FindElementByName("namespace3.namespace4.element1Name.h"));
            Assert.IsNull(dataModel.FindElementByName("namespace1.namespace1.element1Name.h"));

            MoveHeaderElementsAction transformation = new MoveHeaderElementsAction(dataModel, true, null);
            transformation.Execute();

            Assert.IsNotNull(dataModel.FindElementByName("namespace1.namespace1.element1Name.cpp"));
            Assert.IsNotNull(dataModel.FindElementByName("namespace3.namespace4.element1Name.h"));
            Assert.IsNull(dataModel.FindElementByName("namespace1.namespace1.element1Name.h"));
        }
    }
}
