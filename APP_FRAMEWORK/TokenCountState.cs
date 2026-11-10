using System;

namespace APP_FRAMEWORK;

internal static class TokenCountState
{
	private static readonly object SyncRoot = new object();

	private static int currentCount;

	public static int CurrentCount
	{
		get
		{
			lock (SyncRoot)
			{
				return currentCount;
			}
		}
	}

	public static event EventHandler CountChanged;

	public static void SetCount(int count)
	{
		if (count < 0)
		{
			count = 0;
		}
		bool flag;
		lock (SyncRoot)
		{
			flag = currentCount != count;
			currentCount = count;
		}
		if (flag)
		{
			TokenCountState.CountChanged?.Invoke(null, EventArgs.Empty);
		}
	}
}
