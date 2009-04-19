//
// OpCodes.cs
//
// Author:
//   Jb Evain (jbevain@gmail.com)
//
// Generated by /CodeGen/cecil-gen.rb do not edit
// <%=Time.now%>
//
// (C) 2005 Jb Evain
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
<%
	oboc = Array.new
	tboc = Array.new
	$ops.each { |op|
		if op.op1 == "0xff"
			oboc.push(op)
		else
			tboc.push(op)
		end
	}
%>
namespace Mono.Cecil.Cil {

	public sealed class OpCodes {

		internal static readonly OpCode [] OneByteOpCode = new OpCode [<%=oboc[oboc.length - 1].op2%> + 1];
		internal static readonly OpCode [] TwoBytesOpCode = new OpCode [<%=tboc[tboc.length - 1].op2%> + 1];

<% $ops.each { |op| %>		public static readonly OpCode <%=op.field_name%> = new OpCode (
			<%=op.op1%>, <%=op.op2%>,
			Code.<%=op.field_name%>, <%=op.flowcontrol%>,
			<%=op.opcodetype%>, <%=op.operandtype%>,
			<%=op.stackbehaviourpop%>, <%=op.stackbehaviourpush%>);

<% } %>		OpCodes ()
		{
		}

		public static OpCode GetOpCode (Code code)
		{
			switch (code) {
<% $ops.each { |op| %>			case Code.<%=op.field_name%> : return OpCodes.<%=op.field_name%>;
<% } %>			default : return OpCodes.Nop;
			}
		}
	}
}
