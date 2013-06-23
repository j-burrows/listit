using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;

namespace Domain.Abstract
{
    public interface IItemRepository
    {
        IQueryable<Item> items { get; }
        void saveItem(Item adding);
        void deleteItem(Item removing);
        List<Item> itemsFromList(int listID);
    }
}
