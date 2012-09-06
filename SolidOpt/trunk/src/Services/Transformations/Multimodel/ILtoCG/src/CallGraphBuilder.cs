/*
 * $Id: $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;

using SolidOpt.Services.Transformations.CodeModel.CallGraph;

namespace SolidOpt.Services.Transformations.Multimodel.ILtoCG
{
  public class CallGraphBuilder
  {
    private readonly MethodDefinition rootMethod;
    private List<MethodDefinition> rawDefs = new List<MethodDefinition>();

    #region Constructors

    public CallGraphBuilder(MethodDefinition rootMethod)
    {
      this.rootMethod = rootMethod;
    }

    public CallGraph Create()
    {
      return Create(0);
    }

    public CallGraph Create(int maxDepth)
    {
      CGNode rootNode = new CGNode(rootMethod, null);
      VisitCGNode(rootNode, maxDepth);
      return new CallGraph(rootNode, maxDepth);
    }

    #endregion

    private void VisitCGNode(CGNode node, int maxDepth) {
      --maxDepth;
      if (maxDepth < 0)
        return;

      if (node.Method.HasBody) {
        MethodReference mRef;
        foreach (Instruction instr in node.Method.Body.Instructions) {
          if (instr.OpCode.FlowControl == FlowControl.Call) {
            mRef = (instr.Operand as MethodReference);
            CGNode callee = new CGNode(mRef.Resolve(), node);
            node.MethodCalls.Add(callee);
            if (!rawDefs.Contains(callee.Method)) {
              rawDefs.Add(node.Method);
              VisitCGNode(callee, maxDepth);
            }
          }
        }
      }
    }
  }
}

