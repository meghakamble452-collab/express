using System;
using EAGetMail;

namespace NEXUS.APP_FRAMEWORK;

internal class MailNet2
{
	internal bool FetchEmailsByAccount(string email, string password)
	{
		bool result = false;
		try
		{
			string server = "outlook.live.com";
			MailServer mailServer = new MailServer(server, email, password, ServerProtocol.Imap4);
			string v = "TryIt";
			MailClient mailClient = new MailClient(v);
			mailServer.SSLConnection = true;
			mailServer.Port = 993;
			mailClient.Connect(mailServer);
			result = true;
		}
		catch (Exception)
		{
		}
		return result;
	}

	private Imap4Folder FindFolder(string folderPath, Imap4Folder[] folders)
	{
		int num = folders.Length;
		int num2 = 0;
		Imap4Folder imap4Folder;
		while (true)
		{
			if (num2 < num)
			{
				imap4Folder = folders[num2];
				if (string.Compare(imap4Folder.LocalPath, folderPath, ignoreCase: true) == 0)
				{
					break;
				}
				imap4Folder = FindFolder(folderPath, imap4Folder.SubFolders);
				if (imap4Folder == null)
				{
					num2++;
					continue;
				}
				return imap4Folder;
			}
			return null;
		}
		return imap4Folder;
	}
}
