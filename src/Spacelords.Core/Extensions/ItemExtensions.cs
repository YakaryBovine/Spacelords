using System;
using WCSharp.Shared.Data;
using static War3Api.Common;

namespace MacroTools.Extensions
{
  public static class ItemExtensions
  {
    /// <summary>
    /// Determines whether or not the item can be manually dropped.
    /// </summary>
    public static item SetDroppable(this item whichItem, bool canBeDropped)
    {
      SetItemDroppable(whichItem, canBeDropped);
      return whichItem;
    }

    public static bool IsDroppable(this item whichItem) => BlzGetItemBooleanField(whichItem, ITEM_BF_CAN_BE_DROPPED);

    public static item SetPosition(this item whichItem, float x, float y)
    {
      SetItemPosition(whichItem, x, y);
      return whichItem;
    }
    
    public static item SetPosition(this item whichItem, Point position)
    {
      SetItemPosition(whichItem, position.X, position.Y);
      return whichItem;
    }

    public static Point GetPosition(this item whichItem)
    {
      return new Point(GetItemX(whichItem), GetItemY(whichItem));
    }
  }
}