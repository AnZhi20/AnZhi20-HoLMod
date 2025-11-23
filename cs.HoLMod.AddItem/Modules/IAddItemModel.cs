using System;
using System.Collections.Generic;

namespace cs.HoLMod.AddItem;

public interface IAddItemModel
{
    event Action OnFilteredPropsChanged;
    List<int> FilteredProps { get; }
    
    // 方法
    void FilterItems(int propClass = -1, string search = "");
    void AddCoins(int count);
    void AddGold(int count);
    void AddProp(int propId, int propCount);
    void AddStoriesBook(int bookId);

    void AddMansion(int junId, int xianId, string name);
    void AddFarm(int junId, int xianId, string area, string name);
    void AddFief(int junId);
    void AddFamily(int junId, int xianId);
}