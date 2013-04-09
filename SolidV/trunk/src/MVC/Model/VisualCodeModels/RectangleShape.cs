/*
 * $Id:
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

namespace SolidV.MVC
{
  public class RectangleShape : Shape
  {
    public RectangleShape(RectangleShape rect) : base(rect)
    {
    }

    public RectangleShape() : base() {}
  }
}