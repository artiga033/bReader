using bReader.Shared.Models;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml.Linq;

namespace bReader.Shared.Models.Parser
{
    public static class Atom10FeedParser
    {
        private static readonly XNamespace _xml = XNamespace.Xml;

        public static bool TryParseAtom10Feed(XDocument document, out FeedDto parsedFeed)
        {
            parsedFeed = new();

            XElement? feedElement = null;
            XNamespace atom = XNamespace.None;
            foreach (var ns in Atom10Constants.RecognizedNamespaces)
            {
                atom = ns;
                feedElement = document?.Element(atom + "feed");
                if (feedElement != null)
                    break;
            }

            if (feedElement == null)
                return false;

            parsedFeed.Language = feedElement.Attribute(_xml + "lang")?.Value.Trim() ?? parsedFeed.Language;
            parsedFeed.Title = feedElement.Element(atom + "title")?.Value.Trim() ?? parsedFeed.Title;
            parsedFeed.Copyright = feedElement.Element(atom + "rights")?.Value.Trim() ?? parsedFeed.Copyright;
            parsedFeed.Description = feedElement.Element(atom + "subtitle")?.Value.Trim() ?? parsedFeed.Description;
            //parsedFeed.Base = feedElement.Attribute(_xml + "base")?.Value;
            //parsedFeed.Id = feedElement.Element(atom + "id")?.Value.Trim();

            if (TryParseAtom10Timestamp(feedElement.Element(atom + "updated"), out var parsedUpdated))
            {
                parsedFeed.LastUpdatedTime = parsedUpdated;
            }

            foreach (var authorElement in feedElement.Elements(atom + "author"))
            {
                if (TryParseAtom10Person(authorElement, atom, out var parsedPerson))
                {
                    parsedFeed.Authors.Add(parsedPerson);
                }
            }

            foreach (var linkElement in feedElement.Elements(atom + "link"))
            {
                if (TryParseAtom10Link(linkElement, out var parsedLink))
                {
                    parsedFeed.Links.AddIfNotNull(parsedLink);
                }
            }

            foreach (var categoryElement in feedElement.Elements(atom + "category"))
            {
                if (TryParseAtom10Category(categoryElement, out var parsedCategory))
                {
                    parsedFeed.Categories.Add(parsedCategory);
                }
            }

            foreach (var contributorElement in feedElement.Elements(atom + "contributor"))
            {
                if (TryParseAtom10Person(contributorElement, atom, out var parsedPerson))
                {
                    parsedFeed.Authors.Add(parsedPerson);
                }
            }

            parsedFeed.Generator = feedElement.Element(atom + "generator")?.Value ?? parsedFeed.Generator;

            var i = feedElement.Element(atom + "icon");
            var l = feedElement.Element(atom + "logo");
            if (l != null)
                parsedFeed.ImageUrl = new Uri(l.Value.Trim());
            else if (i != null)
                parsedFeed.ImageUrl = new Uri(i.Value.Trim());

            // entries
            foreach (var entryElement in feedElement.Elements(atom + "entry"))
            {
                if (TryParseAtom10Entry(entryElement, atom, out var parsedEntry))
                {
                    parsedFeed.Items.Add(parsedEntry);
                }
            }

            //// extensions
            //ExtensibleEntityParser.ParseXElementExtensions(feedElement, extensionManifestDirectory, parsedFeed);

            return true;
        }

        private static bool TryParseAtom10Entry(XElement entryElement, XNamespace atom,  out FeedItemDto parsedEntry)
        {
            parsedEntry = new();

            if (entryElement == null)
                return false;

            parsedEntry.Id = entryElement.Element(atom + "id")?.Value.Trim()??parsedEntry.Id;
            parsedEntry.Title = entryElement.Element(atom + "title")?.Value.Trim() ?? parsedEntry.Title;
            parsedEntry.Content = entryElement.Element(atom + "content")?.Value.Trim() ?? parsedEntry.Content;
            parsedEntry.Summary = entryElement.Element(atom + "summary")?.Value.Trim() ?? parsedEntry.Summary;

            if (TryParseAtom10Timestamp(entryElement.Element(atom + "updated"), out var parsedUpdated))
            {
                parsedEntry.LastUpdatedTime = parsedUpdated;
            }

            foreach (var authorElement in entryElement.Elements(atom + "author"))
            {
                if (TryParseAtom10Person(authorElement, atom, out var parsedPerson))
                {
                    parsedEntry.Authors.Add(parsedPerson);
                }
            }

            foreach (var linkElement in entryElement.Elements(atom + "link"))
            {
                if (TryParseAtom10Link(linkElement, out var parsedLink))
                {
                    parsedEntry.Links.AddIfNotNull(parsedLink);
                }
            }

            foreach (var categoryElement in entryElement.Elements(atom + "category"))
            {
                if (TryParseAtom10Category(categoryElement, out var parsedCategory))
                {
                    parsedEntry.Categories.Add(parsedCategory);
                }
            }

            foreach (var contributorElement in entryElement.Elements(atom + "contributor"))
            {
                if (TryParseAtom10Person(contributorElement, atom, out var parsedPerson))
                {
                    parsedEntry.Authors.Add(parsedPerson);
                }
            }

            if (TryParseAtom10Timestamp(entryElement.Element(atom + "published"), out var parsedPublished))
            {
                parsedEntry.PublishDate = parsedPublished;
            }

            //if (TryParseAtom10Text(entryElement.Element(atom + "rights"), out var parsedRights))
            //{
            //    parsedEntry.Rights = parsedRights;
            //}

            //if (TryParseAtom10Source(entryElement.Element(atom + "source"), atom, out var parsedSource))
            //{
            //    parsedEntry.Source = parsedSource;
            //}

            //// extensions
            //ExtensibleEntityParser.ParseXElementExtensions(entryElement, extensionManifestDirectory, parsedEntry);

            return true;
        }

        //private static bool TryParseAtom10Source(XElement sourceElement, XNamespace atom, out Atom10Source parsedSource)
        //{
        //    parsedSource = default;

        //    if (sourceElement == null)
        //        return false;

        //    parsedSource = new Atom10Source();

        //    parsedSource.Id = sourceElement.Element(atom + "id")?.Value.Trim();

        //    if (TryParseAtom10Text(sourceElement.Element(atom + "title"), out var parsedTitle))
        //    {
        //        parsedSource.Title = parsedTitle;
        //    }

        //    if (TryParseAtom10Timestamp(sourceElement.Element(atom + "updated"), out var parsedUpdated))
        //    {
        //        parsedSource.Updated = parsedUpdated;
        //    }

        //    return true;
        //}

        //private static bool TryParseAtom10Content(XElement contentElement, out Atom10Content parsedContent)
        //{
        //    parsedContent = default;

        //    if (contentElement == null)
        //        return false;

        //    parsedContent = new Atom10Content();

        //    parsedContent.Type = contentElement.Attribute("type")?.Value ?? "text";
        //    parsedContent.Src = contentElement.Attribute("src")?.Value;

        //    if (!TryParseValueByType(parsedContent.Type, contentElement, out var parsedValue))
        //        return false;

        //    parsedContent.Value = parsedValue;
        //    parsedContent.Lang = contentElement.Attribute(_xml + "lang")?.Value;
        //    parsedContent.Base = contentElement.Attribute(_xml + "base")?.Value;

        //    return true;
        //}

        private static bool TryParseAtom10Category(XElement categoryElement, out CategoryDto parsedCategory)
        {
            parsedCategory = new CategoryDto();
            if (categoryElement == null)
                return false;

            parsedCategory.Name = categoryElement.Attribute("term")?.Value;
            parsedCategory.Label = categoryElement.Attribute("label")?.Value;
            parsedCategory.Scheme = categoryElement.Attribute("scheme")?.Value;

            return true;
        }

        private static bool TryParseAtom10Link(XElement linkElement, out Uri? link)
        {
            link = null;

            if (linkElement == null)
                return false;
            string? href = linkElement.Attribute("href")?.Value;
            bool result = Uri.TryCreate(href,UriKind.RelativeOrAbsolute, out link);
            return result;
        }

        private static bool TryParseAtom10Person(XElement personElement, XNamespace atom, out PersonDto parsedPerson)
        {
            parsedPerson = new();

            if (personElement == null)
                return false;

            parsedPerson.Name = personElement.Element(atom + "name")?.Value.Trim();
            parsedPerson.Email = personElement.Element(atom + "email")?.Value.Trim();
            parsedPerson.Uri = personElement.Element(atom + "uri")?.Value.Trim();

            return true;
        }

        private static bool TryParseAtom10Timestamp(XElement? timestampElement, out DateTimeOffset parsedTimestamp)
        {
            parsedTimestamp = default;

            if (timestampElement == null)
                return false;

            if (!RelaxedTimestampParser.TryParseTimestampFromString(timestampElement.Value, out parsedTimestamp))
                return false;

            return true;
        }
    }

    /// <remarks>
    /// Spec: https://validator.w3.org/feed/docs/atom.html
    /// </remarks>
    internal static class Atom10Constants
    {
        public static readonly XNamespace Namespace = "http://www.w3.org/2005/Atom";
        public static readonly XNamespace XhtmlNamespace = "http://www.w3.org/1999/xhtml";

        public static readonly XNamespace[] RecognizedXhtmlNamespaces =
        {
            "http://www.w3.org/1999/xhtml",
            "https://www.w3.org/1999/xhtml",
        };

        public static readonly XNamespace[] RecognizedNamespaces =
        {
           XNamespace.Get("http://www.w3.org/2005/Atom"),
           XNamespace.Get("https://www.w3.org/2005/Atom"),
        };
    }
}