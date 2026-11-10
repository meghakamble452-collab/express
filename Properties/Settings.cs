using System.ComponentModel;
using System.Configuration;

namespace Properties;

internal sealed class Settings : ApplicationSettingsBase
{
	private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
	{
	}

	private void SettingsSavingEventHandler(object sender, CancelEventArgs e)
	{
	}
}
