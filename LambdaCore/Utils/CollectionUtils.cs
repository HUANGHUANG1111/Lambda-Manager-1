using System.Collections.Generic;
using LambdaManager.DataType;
using Action = LambdaManager.DataType.Action;

namespace LambdaManager.Utils;

internal class CollectionUtils
{
	public static object? Insert<T>(List<T?> list, int index, T? value)
	{
		object oldValue = null;
		if (index < list.Count)
		{
			oldValue = list[index];
			list[index] = value;
		}
		else
		{
			for (int i = list.Count; i < index; i++)
			{
				list.Add(default(T));
			}
			list.Add(value);
		}
		return oldValue;
	}

	public static void Insert2(List<KeyValuePair<Action, int>?> list, int index, KeyValuePair<Action, int> value)
	{
		if (index < list.Count)
		{
			list[index] = value;
			return;
		}
		for (int i = list.Count; i < index; i++)
		{
			list.Add(null);
		}
		list.Add(value);
	}
}
