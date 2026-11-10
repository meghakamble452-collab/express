using Driver.intern.hook;
using Driver.util;

namespace Driver.life;

internal class ActivityUtils
{
	private static string LOGIN_ACTIVITY_SEQUENCES;

	private static string AVL_ACTIVITY_SEQUENCES;

	internal string getRandomActivityData(HookEvent hookEvent, ChromeUser akamaiUser)
	{
		string result = null;
		switch (hookEvent)
		{
		case HookEvent.Webmtoken:
		case HookEvent.ValidateUser:
		{
			long num2 = System2.currentTimeMillis() - akamaiUser.getTimeBeginSession();
			result = string.Format(LOGIN_ACTIVITY_SEQUENCES, akamaiUser.getTimeBeginSession() + Random.nextInt(100, 400), akamaiUser.getTimeBeginSession() + 4L * num2 / 6L, akamaiUser.getTimeBeginSession() + 5L * num2 / 6L);
			break;
		}
		case HookEvent.AvlFarenquiry:
		{
			long num = System2.currentTimeMillis() - akamaiUser.getTimeBeginSession();
			akamaiUser.getTimeBeginSession();
			result = string.Format(AVL_ACTIVITY_SEQUENCES, akamaiUser.getTimeBeginSession() + 5L * num / 20L, akamaiUser.getTimeBeginSession() + 6L * num / 20L, akamaiUser.getTimeBeginSession() + 8L * num / 20L, akamaiUser.getTimeBeginSession() + 9L * num / 20L, akamaiUser.getTimeBeginSession() + 10L * num / 20L, akamaiUser.getTimeBeginSession() + 11L * num / 20L, akamaiUser.getTimeBeginSession() + 14L * num / 20L, akamaiUser.getTimeBeginSession() + 17L * num / 20L, akamaiUser.getTimeBeginSession() + 18L * num / 20L, akamaiUser.getTimeBeginSession() + 19L * num / 20L);
			break;
		}
		case HookEvent.Universal:
			result = "";
			break;
		}
		return result;
	}

	static ActivityUtils()
	{
		string lOGIN_ACTIVITY_SEQUENCES = "3,{0};2,{1};3,{2};";
		LOGIN_ACTIVITY_SEQUENCES = lOGIN_ACTIVITY_SEQUENCES;
		string aVL_ACTIVITY_SEQUENCES = "2,{0};3,{1};2,{2};3,{3};2,{4};3,{5};2,{6};3,{7};2,{8};3,{9};";
		AVL_ACTIVITY_SEQUENCES = aVL_ACTIVITY_SEQUENCES;
	}
}
