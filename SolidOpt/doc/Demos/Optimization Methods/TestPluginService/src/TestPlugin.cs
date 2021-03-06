﻿/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

using SolidOpt.Services;

namespace SolidOpt.Documentation.Samples.TestPluginService
{
  /// <summary>
  /// Description of MyClass.
  /// </summary>
  public class TestPlugin : IAddService
  {    
    public int Add(int a, int b)
    {
      return a + b;
    }
  }
}