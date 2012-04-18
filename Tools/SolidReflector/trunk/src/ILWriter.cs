// /*
//  * $Id$
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */

using Gtk;
using System;

namespace SolidReflector
{
  public class ILWriter
  {
    private int indent = 0;
    private Gtk.TextBuffer Out = null;
    private Gtk.TextIter endIter;

    public ILWriter(Gtk.TextBuffer Out) {
      this.Out = Out;
      this.endIter = Out.EndIter;

      CreateTags(Out);
    }

    private void CreateTags(TextBuffer buffer) {
      TextTag tag = new TextTag("Keywords");
      tag.Foreground = "blue";
      buffer.TagTable.Add(tag);

      tag = new TextTag("MethodAttributes");
      tag.Foreground = "blue";
      buffer.TagTable.Add(tag);

      tag = new TextTag("Exceptions");
      tag.Foreground = "brown";
      buffer.TagTable.Add(tag);

      tag = new TextTag("Types");
      tag.Foreground = "darkgreen";
      buffer.TagTable.Add(tag);

      tag = new TextTag("Names");
      tag.Foreground = "black";
      tag.Weight = Pango.Weight.Bold;
      buffer.TagTable.Add(tag);

      tag = new TextTag("ImplementationAttributes");
      tag.Foreground = "darkred";
      buffer.TagTable.Add(tag);
    }

    public void Indent() {
      indent++;
    }

    public void Outdent() {
      indent--;
    }

    public void NewLine() {
      WriteNewLine();
    }

    public void WriteInstruction(string inst) {
      WriteIndent();
      Out.Insert(ref endIter, inst);
      WriteNewLine();
    }

    public void WriteKeyword(string keyword) {
      WriteIndent();
      Out.InsertWithTagsByName(ref endIter, keyword + " ", "Keywords");
    }

    public void WriteExceptionDirective(string except) {
      WriteIndent();
      Out.InsertWithTagsByName(ref endIter, except + " ", "Exceptions");
      WriteNewLine();
    }

    public void WriteMethodAttribute(string attr) {
      Out.InsertWithTagsByName(ref endIter, attr + " ", "MethodAttributes");
    }

    public void WriteImplementationAttribute(string attr) {
      Out.InsertWithTagsByName(ref endIter, attr + " ", "ImplementationAttributes");
    }

    public void WriteType(string type) {
      Out.InsertWithTagsByName(ref endIter, type + " ", "Types");
    }

    public void WriteName(string ident) {
      Out.InsertWithTagsByName(ref endIter, ident, "Names");
    }

    public void Write(string text) {
      Out.Insert(ref endIter, text);
    }

    private void WriteIndent() {
      for(uint i = 0; i < indent; i++) {
        Out.Insert(ref endIter, "\t");
      }
    }

    private void WriteNewLine() {
      Out.Insert(ref endIter, "\n");
    }
  }
}