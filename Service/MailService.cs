using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Repo.Enums;
using Repo.Models;

namespace Service;

public class MailService
{
 
	private static string From => "ndly12@yandex.ru";
	private static string Password => "Ytrjulf123";

	private static readonly string BaseDir = Directory.GetCurrentDirectory();

	private static List<(string, string)> ReplaceList => new()
	{
		("domain", "http://31.31.24.200:5051")
	};

	private static Dictionary<string, (string path, string caption)> EventList =>
		new()
		{
			{
				"registered", ($@"{BaseDir}\EmailTriggers\registered.html", "Welcome")
			},
			{
				"addReviewer",($@"{BaseDir}\EmailTriggers\addreviewer.html", "You were attached as a publication reviewer!")
			}
		};

	public async Task RegisterSuccess(SendRegisterMail request)
	{
		await SendEvent(request.Email, "registered", new List<KeyValuePair<string, string>>
		{
			new("Token", request.Token)
		});
	}

	public async Task SendEvent(string email, string eventName, List<KeyValuePair<string, string>> replaceValue)
	{
		var body = await GetTrigger(eventName);
		SendMail(email, EventList[eventName].caption, UpdateBody(body, replaceValue));
	}

	public static bool IsValidEmailAddress(string email) =>
		string.IsNullOrWhiteSpace(email) || Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

	private static void SendMail(string mailto, string caption, string message)
	{
		try
		{
			using var mail = new MailMessage
			{
				From = new MailAddress(From),
				Subject = caption,
				IsBodyHtml = true,
				Body = message
			};

			mail.To.Add(new MailAddress(Regex.Replace(mailto, @"\+(.*)\@", "@")));

			var client = new SmtpClient
			{
				Host = "smtp.yandex.ru",
				//#if DEBUG
				//EnableSsl = false,
				//#else
				EnableSsl = true,
				//#endif
				Port = 587,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(From, Password),
				//DeliveryMethod = SmtpDeliveryMethod.Network
			};
			client.Send(mail);
		}
		catch (Exception)
		{
			// ignored
		}
	}

	private static string UpdateBody(string message, List<KeyValuePair<string, string>> list)
	{
		list.ForEach(x => { message = message.Replace(ConvertKey(x.Key), x.Value); });
		ReplaceList.ForEach(x => { message = message.Replace(ConvertKey(x.Item1), x.Item2); });
		return message;
	}

	private static string ConvertKey(string key) => $"*|{key}|*";

	private static async Task<string> GetTrigger(string triggerName)
	{
		if (string.IsNullOrWhiteSpace(triggerName))
			throw new TirException(EnumErrorCode.TriggerIsNotFound);

		if (!EventList.ContainsKey(triggerName))
			throw new TirException(EnumErrorCode.TriggerIsNotSupported);

		using var reader = new StreamReader(EventList[triggerName].path);
		return await reader.ReadToEndAsync();
	}
}
