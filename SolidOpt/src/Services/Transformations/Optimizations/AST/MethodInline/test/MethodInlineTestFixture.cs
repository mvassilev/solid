/*
 * $Id: $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Diagnostics;
using System.IO;

using Mono.Cecil;

using NUnit.Framework;

using SolidOpt.Services.Transformations.CodeModel.AbstractSyntacticTree;
using SolidOpt.Services.Transformations.Multimodel.ILtoAST;
using SolidOpt.Services.Transformations.Multimodel.Test;
using SolidOpt.Services.Transformations.Optimizations.AST.MethodInline;

namespace SolidOpt.Services.Transformations.Optimizations.AST.MethodInline.Test
{
  [TestFixture]
  public class MethodInlineTestFixture 
    : BaseTestFixture<AstMethodDefinition, AstMethodDefinition, InlineTransformer>
  {
    private readonly string testCasesDirCache = Path.Combine("src",
                                                             "Services",
                                                             "Transformations",
                                                             "Optimizations",
                                                             "AST",
                                                             "MethodInline",
                                                             "test",
                                                             "TestCases");
    
    public MethodInlineTestFixture()
    {
      // Do not implement. NUnit uses reflection. Moreover the base class does things 
      // on static init.
    }

    protected override string GetTestCaseFileExtension()
    {
      return "cs";
    }
    
    protected override string GetTestCaseResultFileExtension()
    {
      return "cs.ast";
    }

    protected override string GetTestCaseDirOffset() {
      return testCasesDirCache;
    }

    private AstMethodDefinition[] getAstMethodDef(string testCaseName) {
      MethodDefinition mainMethodDef = LoadTestCaseMethod(testCaseName)[0];
      return new AstMethodDefinition[] {new ILtoASTTransformer().Decompile(mainMethodDef)};
    }
    
    [Test, TestCaseSource("GetTestCases")] /*Comes from the base class*/
    public void Cases(string filename)
    {
      // FIXME: the implementation is raw and throws many NotImplementedExceptions. Should go away
      // once it matures.
      AstMethodDefinition[] defs = null;
      try {
         defs = getAstMethodDef(filename);
      }
      finally {
        RunTestCase(filename, defs);
      }

    }
  }
}
