using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Tax.DataLoad
{
	public class ContentHelper
	{
		private const string _contentTemplate = "ScriptManager1=ctl18%24UpdatePanel1%7Cctl18%24SeekButton&clientoId=69073&clientvId=0&children=5&callId=107323&searchBox=1&__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=%2FwEPDwULLTE0NDUxMTc5MjMPZBYCAgIPZBYCAgMPZBYCAgUPZBYCAhEPZBYCAgIPZBYCZg8WAh4KQ2FsbGJhY2tJRAUNY3RsMjAkUmF0aW5nMWRkDvVDdyClT1PcBGHjSrX%2Blqt3bu0%3D&__EVENTVALIDATION=%2FwEWCALKqcb2CAKNoq7uDwKTwu62DALovt%2BiBALovpvxDgK74pi%2BDQLWy7bTBwLJ1O7NCVurpCLP3qS0zVMTlk%2FvqD%2BXFDO6&__refocus=&scrw=&scrh=&ctl04%24seek=&ctl18%24Seek1=&ctl18%24Seek2={0}&hiddenInputToUpdateATBuffer_CommonToolkitScripts=1&__ASYNCPOST=true&ctl18%24SeekButton=%20s%C3%B8g%20";
		private readonly Encoding _encoding;

		public ContentHelper()
		{
			_encoding = Encoding.UTF8;
		}

		public ContentHelper(Encoding encoding)
		{
			_encoding = encoding;
		}

		public HtmlDocument GetContent(int cvrNumber)
		{
			var contentString = string.Format(_contentTemplate, cvrNumber);
			var content = new StringContent(contentString);
			content.Headers.ContentType.MediaType = "application/x-www-form-urlencoded";

			return GetContent(content, 0);
		}

		private HtmlDocument GetContent(HttpContent content, int tries)
		{
			if (tries > 10)
			{
				throw new ContentUnavailableException(string.Format("Tried {0} times", tries));
			}

			using (var client = new HttpClientWrapper())
			{
				
				var result = client.HttpClient.PostAsync("SKAT.aspx?oId=69073", content).Result;
				if (result.IsSuccessStatusCode)
				{
					var contentBytes = result.Content.ReadAsByteArrayAsync().Result;
					var contentString = _encoding.GetString(contentBytes);
					var document = new HtmlDocument();
					document.LoadHtml(contentString);
					return document;
				}
				return GetContent(content, ++tries);
			}
		}
	}
}
