using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;

namespace Domain.Abstract
{
    public interface IWListRepository
    {
        IQueryable<WList> wlists{get;}
        void saveList(WList saving_wlist);
        void removeList(WList removing_wlist);
        List<WList> listsAtUserDate(string username, DateTime date);
    }
}
