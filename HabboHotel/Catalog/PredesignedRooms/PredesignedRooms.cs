﻿using System;
using System.Collections.Generic;

namespace Neon.HabboHotel.Catalog.PredesignedRooms
{
    internal class PredesignedRooms
    {
        internal uint PredesignedId, RoomId;
        internal string RoomModel, CatalogItems;
        internal string[] FloorItems, WallItems, RoomDecoration;
        internal List<PredesignedFloorItems> FloorItemData;
        internal List<PredesignedWallItems> WallItemData;

        internal PredesignedRooms(uint predesignedId, uint roomId, string roomModel, string floorItems, string wallItems,
            string catalogItems, string roomDecoration)
        {
            PredesignedId = predesignedId;
            RoomId = roomId;
            RoomModel = roomModel;
            FloorItems = ((floorItems == string.Empty) ? null : floorItems.Split(';'));
            if (FloorItems != null)
            {
                FloorItemData = new List<PredesignedFloorItems>();
                foreach (string item in FloorItems)
                {
                    string[] itemsData = item.Split(new string[] { "$$$$" }, StringSplitOptions.None);
                    FloorItemData.Add(new PredesignedFloorItems(Convert.ToUInt32(itemsData[0]),
                        Convert.ToInt32(itemsData[1]), Convert.ToInt32(itemsData[2]),
                        Convert.ToInt32(itemsData[4]), Convert.ToDouble(itemsData[3]), itemsData[5]));
                }
            }

            WallItems = ((wallItems == string.Empty) ? null : wallItems.Split(';'));
            if (WallItems != null)
            {
                WallItemData = new List<PredesignedWallItems>();
                foreach (string item in WallItems)
                {
                    string[] itemsData = item.Split(new string[] { "$$$$" }, StringSplitOptions.None);
                    WallItemData.Add(new PredesignedWallItems(Convert.ToUInt32(itemsData[0]), itemsData[1], itemsData[2]));
                }
            }

            CatalogItems = catalogItems;
            RoomDecoration = roomDecoration.Split(';');
        }
    }
}



