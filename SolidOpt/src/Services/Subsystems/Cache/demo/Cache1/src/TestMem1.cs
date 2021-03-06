/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;



namespace SolidOpt.Services.Subsystems.Cache.Demo.Cache1
{  
  /// <summary>
  /// Description of TestMem1.
  /// </summary>
  public class TestMem1: TestBase
  {
    public TestMem1(): base()
    {
      this.cache = new CacheManager<int,ResultClass>(
        new CacheManager<int,ResultClass>.ValidateDelegate(Validate),
        new CacheManager<int,ResultClass>.CalculateDelegate(Calculate),
        new CacheManager<int,ResultClass>.UpdateDelegate(Update)
      );
    }
    
    public virtual bool Validate(int key, CacheManager<int,ResultClass>.CacheItem item)
    {
      return key != 1;
    }
    
    public virtual CacheManager<int,ResultClass>.CacheItem Calculate(int key)
    {
      return new CacheManager<int,ResultClass>.MemoryCacheItem(ClassBuilder.BuildResult(0)); //key
    }
    
    public virtual void Update(int key, CacheManager<int,ResultClass>.CacheItem oldItem)
    {
      oldItem.Value.Text += "!";
    }
  }
}
