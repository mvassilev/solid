/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;

using DataMorphose.Model.Meta;

namespace DataMorphose.Model
{
  /// <summary>
  /// Column - the smallest unit of the database. All the elements of the column have the same
  /// relations and properties.
  /// </summary>
  public class Column
  {
    private Filter filter;
    public Filter Filter {
      get { return filter; }
      set { filter = value; }
    }

    private MetaData meta;
    public MetaData Meta {
      get { return meta; }
      set { meta = value; }
    }

    private List<object> values = new List<object>();
    public List<object> Values {
      get { return this.values; }
      set { values = value; }
    }

    public Column(string name) : this() {
      meta.Name = name;
    }

    public Column() {
      meta = new MetaData(this);
    }
  }
}

