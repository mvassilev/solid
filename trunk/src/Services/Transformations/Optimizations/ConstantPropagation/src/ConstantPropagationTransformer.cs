﻿/*
 *
 * User: Vassil Vassilev
 * Date: 18.9.2009 г.
 * Time: 16:28
 * 
 */
using System;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Cecil.Decompiler.Ast;

using SolidOpt.Services.Transformations.Multimodel.AstMethodDefinitionModel;
using SolidOpt.Services.Transformations.Multimodel.ILtoAST;

using SolidOpt.Services.Transformations.Optimizations;

namespace SolidOpt.Services.Transformations.Optimizations.ConstantPropagation
{
	/// <summary>
	/// Description of ConstantPropagationTransformer.
	/// </summary>
	public class ConstantPropagationTransformer : BaseCodeTransformer, IOptimize<AstMethodDefinition>
	{
		
		private Dictionary<Expression, Expression> substitutions = new Dictionary<Expression, Expression>();
		
		public ConstantPropagationTransformer()
		{
		}
		
		public AstMethodDefinition Optimize(AstMethodDefinition source)
		{
			source.Block = (BlockStatement) Visit(source.Block);
			return source;
		}
		
		public override ICodeNode VisitAssignExpression(AssignExpression node)
		{
			return base.VisitAssignExpression(node);
//			if (node.Target is VariableReferenceExpression ||)
			
			
		}
		
		public override ICodeNode VisitArgumentReferenceExpression(ArgumentReferenceExpression node)
		{
			return base.VisitArgumentReferenceExpression(node);
		}
		
		public override ICodeNode VisitVariableReferenceExpression(VariableReferenceExpression node)
		{
			return base.VisitVariableReferenceExpression(node);
		}
		
		
	}
}
