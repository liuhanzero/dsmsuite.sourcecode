﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using DsmSuite.Analyzer.VisualStudio.Settings;
using DsmSuite.Analyzer.VisualStudio.Test.Util;
using DsmSuite.Analyzer.VisualStudio.VisualStudio;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DsmSuite.Analyzer.VisualStudio.Test.VisualStudio
{
    [TestClass]
    public class SourceFileTest
    {
        [TestMethod]
        public void TestFileInfo()
        {
            string testDataDirectory = TestData.TestDataDirectory;
            FileInfo fileInfo = new FileInfo(Path.Combine(testDataDirectory, @"DirA\ClassA1.cpp"));

            SourceFile sourceFile = new SourceFile(fileInfo, null, null);
            Assert.AreEqual(fileInfo, sourceFile.SourceFileInfo);
        }

        [TestMethod]
        public void TestNameIsFullPath()
        {
            string testDataDirectory = TestData.TestDataDirectory;
            FileInfo fileInfo = new FileInfo(Path.Combine(testDataDirectory, @"DirA\ClassA1.cpp"));

            SourceFile sourceFile = new SourceFile(fileInfo, null, null);
            Assert.AreEqual(fileInfo.FullName, sourceFile.Name);
        }

        [TestMethod]
        public void TestFileTypeIsExtension()
        {
            string testDataDirectory = TestData.TestDataDirectory;
            FileInfo fileInfo = new FileInfo(Path.Combine(testDataDirectory, @"DirA\ClassA1.cpp"));

            SourceFile sourceFile = new SourceFile(fileInfo, null, null);
            Assert.AreEqual("cpp", sourceFile.FileType);
        }

        [TestMethod]
        public void TestProjectFolder()
        {
            string testDataDirectory = TestData.TestDataDirectory;
            FileInfo fileInfo = new FileInfo(Path.Combine(testDataDirectory, @"DirA\ClassA1.cpp"));

            SourceFile sourceFile = new SourceFile(fileInfo, "ProjectFolderName", null);
            Assert.AreEqual("ProjectFolderName", sourceFile.ProjectFolder);
        }

        [TestMethod]
        public void TestClassWithSameNameAndSameContentHaveSameId()
        {
            string testDataDirectory = TestData.TestDataDirectory;
            FileInfo fileInfo1 = new FileInfo(Path.Combine(testDataDirectory, @"DirA\ClassA1.cpp"));
            FileInfo fileInfo2 = new FileInfo(Path.Combine(testDataDirectory, "DirClones/Identical/ClassA1.cpp"));
            SourceFile sourceFile1 = new SourceFile(fileInfo1, null, null);
            SourceFile sourceFile2 = new SourceFile(fileInfo2, null, null);
            Assert.AreEqual(sourceFile1.Id, sourceFile2.Id);
        }

        [TestMethod]
        public void TestClassWithSameNameAndOtherContentHaveOtherId()
        {
            string testDataDirectory = TestData.TestDataDirectory;
            FileInfo fileInfo1 = new FileInfo(Path.Combine(testDataDirectory, @"DirA\ClassA1.cpp"));
            FileInfo fileInfo2 = new FileInfo(Path.Combine(testDataDirectory, "DirClones/NotIdentical/ClassA1.cpp"));
            SourceFile sourceFile1 = new SourceFile(fileInfo1, null, null);
            SourceFile sourceFile2 = new SourceFile(fileInfo2, null, null);
            Assert.AreNotEqual(sourceFile1.Id, sourceFile2.Id);
        }

        [TestMethod]
        public void TestClassWithOtherNameAndSameContentHaveOtherId()
        {
            string testDataDirectory = TestData.TestDataDirectory;
            FileInfo fileInfo1 = new FileInfo(Path.Combine(testDataDirectory, @"DirA\ClassA1.cpp"));
            FileInfo fileInfo2 = new FileInfo(Path.Combine(testDataDirectory, "DirClones/Identical/CopyClassA1.cpp"));
            SourceFile sourceFile1 = new SourceFile(fileInfo1, null, null);
            SourceFile sourceFile2 = new SourceFile(fileInfo2, null, null);
            Assert.AreNotEqual(sourceFile1.Id, sourceFile2.Id);
        }
        
        [TestMethod]
        public void TestAnalyzeFindsIncludes()
        {
            string testDataDirectory = TestData.TestDataDirectory;
            FileInfo fileInfo1 = new FileInfo(Path.Combine(testDataDirectory, @"DirA\ClassA2.cpp"));
            List<string> projectIncludes = new List<string>
            {
                Path.Combine(testDataDirectory, "DirA"),
                Path.Combine(testDataDirectory, "DirB"),
                Path.Combine(testDataDirectory, "DirC"),
                Path.Combine(testDataDirectory, "DirD")
            };
            List<string> interfaceIncludes = new List<string>
            {
                Path.Combine(testDataDirectory, "DirInterfaces")
            };
            List<string> externalIncludes = new List<string>();
            projectIncludes.Add(Path.Combine(testDataDirectory, "DirExternal"));
            List<string> systemIncludes = AnalyzerSettings.CreateDefault().SystemIncludeDirectories;

            IncludeResolveStrategy includeResolveStrategy = new IncludeResolveStrategy(projectIncludes, interfaceIncludes, externalIncludes, systemIncludes);
            SourceFile sourceFile = new SourceFile(fileInfo1, null, includeResolveStrategy);
            sourceFile.Analyze();

            ImmutableHashSet<string> includes = sourceFile.Includes.ToImmutableHashSet();
            Assert.IsTrue(includes.Contains(Path.Combine(testDataDirectory, @"DirA\ClassA2.h")));
            Assert.IsTrue(includes.Contains(Path.Combine(testDataDirectory, @"DirA\ClassA1.h")));
            Assert.IsTrue(includes.Contains(Path.Combine(testDataDirectory, @"DirB\ClassB1.h")));
            Assert.IsTrue(includes.Contains(Path.Combine(testDataDirectory, @"DirC\ClassC1.h")));
            Assert.IsTrue(includes.Contains(Path.Combine(testDataDirectory, @"DirD\ClassD1.h")));
            Assert.IsTrue(includes.Contains(Path.Combine(testDataDirectory, @"DirExternal\External.h")));
            Assert.IsTrue(includes.Contains(Path.Combine(testDataDirectory, @"DirInterfaces\ClassA3.h")));
            Assert.IsTrue(includes.Contains(@"C:\Program Files (x86)\Windows Kits\8.1\Include\um\windows.h"));
            Assert.AreEqual(8, sourceFile.Includes.Count);
        }
    }
}
