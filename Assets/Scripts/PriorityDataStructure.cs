using System.Collections.Generic;
using System.Linq;

public abstract class PriorityDataStructureBase
{
    public int priority;
}

public class intClass : PriorityDataStructureBase
{
    public int value;
}

public class stringClass : PriorityDataStructureBase
{
    public string value;
}

public class PriorityDataStructure<T> where T : PriorityDataStructureBase
{
    private List<T> priorityDatas;
    private bool isDirty;

    public PriorityDataStructure()
    {
        priorityDatas = new List<T>();
        isDirty = false;
    }

    public void UnInit()
    {
        priorityDatas.Clear();
        priorityDatas = null;
    }

    public void Add(T data)
    {
        if (priorityDatas == null)
            return;

        priorityDatas.Add(data);
        isDirty = true;
    }

    public void Delete(T data)
    {
        if (priorityDatas == null || priorityDatas.Count == 0)
            return;

        priorityDatas.Remove(data);
    }

    public bool TryGet(out T data)
    {
        data = null;

        if (priorityDatas.Count == 0)
            return false;

        if (isDirty)
        {
            QuickSortArray(0, priorityDatas.Count - 1);
            isDirty = false;
        }

        data = priorityDatas[0];
        return true;
    }

    private void QuickSortArray(int low, int high)
    {
        if (low < high)
        {
            // 获取分区索引
            // 基准值
            var pivot = priorityDatas[high];

            // i 是小于基准值区域的最后一个索引
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                if (priorityDatas[j].priority <= pivot.priority) // 按优先级排序
                {
                    i++;
                    var temp = priorityDatas[i];
                    priorityDatas[i] = priorityDatas[j];
                    priorityDatas[j] = temp;
                }
            }

            // 将基准值与 array[i + 1] 交换，使基准值处于正确位置
            var temp2 = priorityDatas[i + 1];
            priorityDatas[i + 1] = priorityDatas[high];
            priorityDatas[high] = temp2;

            int partitionIndex = i + 1;

            // 递归地对左子数组和右子数组进行排序
            QuickSortArray(low, partitionIndex - 1);
            QuickSortArray(partitionIndex + 1, high);
        }
    }
}