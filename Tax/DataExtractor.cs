﻿using HtmlAgilityPack;
using System;
using System.Globalization;
using System.Linq;
using Tax.Model;

namespace Tax
{
	public class DataExtractor
	{
		public void Extract(Company company, HtmlDocument document)
		{
			company.Name = GetStringValue(document, "Virksomhedsnavn");
			company.Type = GetStringValue(document, "Selskabstype");
			company.Legislation = GetStringValue(document, "Den skattelov");

			if (document.DocumentNode.OuterHtml.Contains("Selskabet bliver sambeskattet med nedenstående administrationsselskab"))
			{
				company.IsSubsidiary = true;
				return;
			}

			company.TaxPaid = GetDecimalValue(document, "Selskabsskatten");
			company.Revenue = GetDecimalValue(document, "Skattepligtig");
			company.Losses = GetDecimalValue(document, "Underskud");
			company.Subsidiaries = GetSubsidiaryCvrNumbers(document);
		}

		private string GetStringValue(HtmlDocument document, string title)
		{
			var titleElement = document.DocumentNode.SelectNodes("//div[@class='SLoutput']").Single(x => x.InnerText.StartsWith(title));
			var valueElement = titleElement.NextSibling;
			return valueElement.InnerText.Trim();
		}

		private decimal? GetDecimalValue(HtmlDocument document, string title)
		{
			var value = GetStringValue(document, title).Replace("kr", "").Trim();
			decimal result;
			if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.GetCultureInfo("da"), out result))
			{
				return result;
			}
			return null;
		}

		private string GetSubsidiaryCvrNumbers(HtmlDocument document)
		{
			var links = document.DocumentNode.SelectNodes("//div[@class='SLoutputDataDaughter']/div/a");
			if (links != null)
			{
				return links.Select(x => x.Attributes["href"].Value.Split(new[] { "&x=" }, StringSplitOptions.RemoveEmptyEntries).Last()).Aggregate((x, y) => x + " " + y);
			}
			return null;
		}
	}
}
