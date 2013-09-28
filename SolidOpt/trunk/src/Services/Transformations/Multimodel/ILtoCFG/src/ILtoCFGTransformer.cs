/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

using SolidOpt.Services.Transformations.Multimodel;
using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;

using Mono.Cecil;
using Mono.Cecil.Cil;

namespace SolidOpt.Services.Transformations.Multimodel.ILtoCFG
{
  /// <summary>
  /// Description of CilToControlFlowGraph.
  /// </summary>
  public class CilToControlFlowGraph : DecompilationStep, ITransform<MethodDefinition, ControlFlowGraph<Instruction>>
  {    
    #region Constructors
    
    public CilToControlFlowGraph ()
    {
    }
    
    #endregion
    
    public override object Process (object codeModel)
    {
      return Process (codeModel as MethodBody);
    }

    public override Type GetSourceType()
    {
      return typeof(MethodDefinition);
    }

    public override Type GetTargetType()
    {
      return typeof(ControlFlowGraph<Instruction>);
    }

    public ControlFlowGraph<Instruction> Process (MethodBody source)
    {
      if (source == null)
        throw new ArgumentNullException ("method");
      if (!source.Method.HasBody)
        throw new ArgumentException();

      var builder = new ControlFlowGraphBuilder (source.Method);
      return builder.Create();
    }
    
    public ControlFlowGraph<Instruction> Transform(MethodDefinition source)
    {
      return Decompile(source);
    }
    
    public ControlFlowGraph<Instruction> Decompile(MethodDefinition source)
    {
      return Process(source.Body);
    }
  }
}
