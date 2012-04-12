/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using SolidOpt.Services;
using SolidOpt.Services.Transformations.Multimodel;

using System;
using System.Diagnostics;
using Mono.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

public partial class MainWindow: Gtk.Window
{
  private PluginServiceContainer plugins = new PluginServiceContainer();
  private string[] fileNames = {};
  private AssemblyDefinition curAssembly = null;
  private ModuleDefinition curModule = null;
  private TypeDefinition curType = null;

	public MainWindow(): base(Gtk.WindowType.Toplevel)
	{
    // That's a hack because of the designer. If one needs to attach an event the designer attaches
    // it in the end of the file after the call to Initialize. Works for the most of the events
    // but not for events like Realize which happen in the initialization. This function is used
    // to attach the event handlers before the initialization part.
    PreBuild();
		Build();
	}
	
	protected void OnDeleteEvent(object sender, Gtk.DeleteEventArgs a)
	{
    SaveEnvironment();
		Gtk.Application.Quit();
		a.RetVal = true;
	}

	protected void OnOpenActionActivated(object sender, System.EventArgs e)
  {
    var fc = new Gtk.FileChooserDialog("Choose the file to open",
                                        this, Gtk.FileChooserAction.Open,
                                        "Cancel", Gtk.ResponseType.Cancel,
                                        "Open", Gtk.ResponseType.Accept);
    try {
      fc.SelectMultiple = true;
      fc.SetCurrentFolder(Environment.CurrentDirectory);
      if (fc.Run() == (int)Gtk.ResponseType.Accept) {
        fileNames = fc.Filenames;
      }
    } finally {
      fc.Destroy();
    }

    LoadFilesInTreeView();
  }

	protected void OnExitActionActivated(object sender, System.EventArgs e)
	{
    SaveEnvironment();
		Gtk.Application.Quit();
	}

  protected void OnRealized(object sender, System.EventArgs e)
  {
    LoadEnvironment();
    LoadRegisteredPlugins();
  }

  private void LoadEnvironment() {
    string curEnv = System.IO.Path.Combine(Environment.CurrentDirectory, "Current.env");
    if (System.IO.File.Exists(curEnv)) {
      fileNames = System.IO.File.ReadAllLines(curEnv);
      LoadFilesInTreeView();
    }
  }

  private void SaveEnvironment() {
    string curEnv = System.IO.Path.Combine(Environment.CurrentDirectory, "Current.env");
    System.IO.File.WriteAllLines(curEnv, fileNames);

    string pluginsEnv = System.IO.Path.Combine(Environment.CurrentDirectory, "Plugins.env");

    foreach(PluginInfo pInfo in plugins.plugins) {
      System.IO.File.AppendText(pluginsEnv).WriteLine(pInfo.codeBase);
    }
  }

  private void LoadFilesInTreeView() {

    Gtk.TreeViewColumn col = new Gtk.TreeViewColumn();

    Gtk.CellRendererText colTitleCell = new Gtk.CellRendererText();
    col.PackStart(colTitleCell, true);

    col.AddAttribute (colTitleCell, "text", 0);

    assemblyView.AppendColumn(col);

    Gtk.TreeStore store = new Gtk.TreeStore(typeof(string));
    foreach (string file in fileNames) {
      store.AppendValues(System.IO.Path.GetFileName(file));
    }

    assemblyView.Model = store;
    assemblyView.ShowAll();
  }

  private void LoadRegisteredPlugins() {
    string registeredPlugins =
        System.IO.Path.Combine(Environment.CurrentDirectory, "Plugins.env");
    if (System.IO.File.Exists(registeredPlugins)) {
      foreach (string s in System.IO.File.ReadAllLines(registeredPlugins))
        if (System.IO.File.Exists(s))
          plugins.AddPlugin(s);
    }

    plugins.LoadPlugins();
    //IDecompile<MethodDefinition, ControlFlowGraph> ILtoCfgTransformer =
    //   plugins.GetService<IDecompile<MethodDefinition, ControlFlowGraph>>();


  }

  protected virtual void PreBuild() {
    this.Realized += new global::System.EventHandler (this.OnRealized);
  }

  protected void OnAssemblyViewRowActivated (object o, Gtk.RowActivatedArgs args)
  {
    Gtk.TreeIter iter;
    assemblyView.Model.GetIter(out iter, args.Path);
    string s = (string) assemblyView.Model.GetValue(iter, 0);

    switch(args.Path.Depth) {
    case 1:
        foreach (string f in fileNames) {
          if (System.IO.Path.GetFileName(f) == s) {
            curAssembly = AssemblyDefinition.ReadAssembly(f);
            Debug.Assert(curAssembly != null, "Assembly cannot be null.");
            AttachSubTree(assemblyView.Model, iter, curAssembly.Modules.ToArray());
          }
        }
        break;
    case 2:
        foreach (ModuleDefinition mDef in curAssembly.Modules) {
          if (mDef.Name == s) {
            curModule = mDef;
            AttachSubTree(assemblyView.Model, iter, mDef.Types.ToArray());
          }
        }
        break;

    case 3:
        Debug.Assert(curModule != null, "CurModule is null!?");
        foreach (TypeDefinition tDef in curModule.Types) {
          if (tDef.FullName == s) {
            curType = tDef;
            //AttachSubTree(assemblyView.Model, iter, tDef.Fields.ToArray());
            AttachSubTree(assemblyView.Model, iter, tDef.Methods.ToArray());
            //AttachSubTree(assemblyView.Model, iter, tDef.Events.ToArray());
          }
        }
        break;
      case 4:
        Debug.Assert(curType != null, "CurType is null!?");
        foreach (MethodDefinition mDef in curType.Methods)
          if (mDef.ToString() == s)
            DumpMember(mDef);
        break;
    }
    assemblyView.ShowAll();
    ShowAll();
  }

  protected void AttachSubTree(Gtk.TreeModel model, Gtk.TreeIter parent, object[] elements)
  {
    Gtk.TreeStore store = model as Gtk.TreeStore;
    Debug.Assert(store != null, "TreeModel shouldn't be flat");
    for (uint i = 0; i < elements.Length; ++i) {
      store.AppendValues(parent, elements[i].ToString());
    }
  }

  protected void DumpMember(IMemberDefinition member)
  {
    disassemblyText.Buffer.Clear();

    Gtk.TextIter textIter = disassemblyText.Buffer.EndIter;
    MethodDefinition method = member as MethodDefinition;
    if (method != null) {
      disassemblyText.Buffer.Insert(ref textIter, ".method ");

      if (method.IsPublic)
        disassemblyText.Buffer.Insert(ref textIter, "public ");
      if (method.IsPrivate)
        disassemblyText.Buffer.Insert(ref textIter, "private ");
      if (method.IsHideBySig)
        disassemblyText.Buffer.Insert(ref textIter, "hidebysig ");
      if (method.IsStatic)
        disassemblyText.Buffer.Insert(ref textIter, "static ");
      else
        disassemblyText.Buffer.Insert(ref textIter, "instance ");
      //else if (method.is)

      disassemblyText.Buffer.Insert(ref textIter, method.ReturnType.ToString() + " ");
      disassemblyText.Buffer.Insert(ref textIter, method.Name + "(");
      if (method.Parameters.Count > 0) {
        disassemblyText.Buffer.Insert(ref textIter, method.Parameters[0].ParameterType + " ");
        disassemblyText.Buffer.Insert(ref textIter, method.Parameters[0].Name);
      }
      disassemblyText.Buffer.Insert(ref textIter, ") ");
      if (method.IsIL)
        disassemblyText.Buffer.Insert(ref textIter, "cil ");
      else if (method.IsNative)
        disassemblyText.Buffer.Insert(ref textIter, "native ");

      if (method.IsManaged)
        disassemblyText.Buffer.Insert(ref textIter, "managed ");
      else if (method.IsUnmanaged)
        disassemblyText.Buffer.Insert(ref textIter, "unmanaged ");

      disassemblyText.Buffer.Insert(ref textIter, "\n");

      if (method.Body.Variables.Count > 0) {
        disassemblyText.Buffer.Insert(ref textIter, ".locals init (");
        for (int i = 0; i < method.Body.Variables.Count; i++) {
            disassemblyText.Buffer.Insert(ref textIter, method.Body.Variables[i].VariableType.ToString() + " ");
            disassemblyText.Buffer.Insert(ref textIter, method.Body.Variables[i].ToString() + "\n");
        }
        disassemblyText.Buffer.Insert(ref textIter, ")\n");
      }

      foreach (Instruction inst in method.Body.Instructions) {
         disassemblyText.Buffer.Insert(ref textIter, inst.ToString() + "\n");
      }

      if (method.Body.HasExceptionHandlers) {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        foreach (var handler in method.Body.ExceptionHandlers) {
          if (handler.FilterStart != null) {
            sb.Append(".filter ");
            AppendLabel(sb, handler.FilterStart);
            sb.Append(" to ");
            AppendLabel(sb, handler.FilterEnd);
          }
          if (handler.TryStart != null) {
            sb.Append("\n.try ");
            AppendLabel(sb, handler.TryStart);
            sb.Append(" to ");
            AppendLabel(sb, handler.TryEnd);
          }
          if (handler.HandlerStart != null) {
            sb.Append(" .handler ");
            sb.Append(handler.HandlerType + " ");
            AppendLabel(sb, handler.HandlerStart);
            sb.Append(" to ");
            AppendLabel(sb, handler.HandlerEnd);
          }
        }

        disassemblyText.Buffer.Insert(ref textIter, sb.ToString() + "\n");

      }
      return;
    }

    EventDefinition evt = member as EventDefinition;
    if (evt != null) {
      foreach (MethodDefinition mDef in evt.OtherMethods) {
         disassemblyText.Buffer.Insert(ref textIter, mDef.ToString() + "\n");
      }
      return;
    }

    FieldDefinition field = member as FieldDefinition;
    if (field != null) {
      disassemblyText.Buffer.Insert(ref textIter, field.ToString() + "\n");
      return;
    }

  }

  private void AppendLabel (System.Text.StringBuilder builder, Instruction instruction) {
    builder.Append ("IL_");
    builder.Append (instruction.Offset.ToString ("x4"));
  }

  protected void OnCombobox6Changed (object sender, System.EventArgs e)
  {
    Gtk.TreeIter iter;
    combobox6.GetActiveIter(out iter);
    string val = (string)combobox6.Model.GetValue(iter, 0);
    if (val == "IL") {

    }
    else if (val == "CFG") {

    }

  }
}
