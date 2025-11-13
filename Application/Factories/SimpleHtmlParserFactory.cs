using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Enums;
using Application.Interfaces.Factories;
using Application.Interfaces.Parsers;
using Application.Parsers;

namespace Application.Factories
{
    internal class SimpleHtmlParserFactory : IHtmlParserFactory
    {
        Dictionary<string, Domens> _domens;
        SimpleHtmlParserFactory(Dictionary<string, Domens> domens)
        {
            _domens = domens;
        }
        private static IHtmlParser? _amazonParser;
        private static IHtmlParser AmazonParser
        {
            get
            {
                _amazonParser ??= new AmazonHtmlParser();

                return _amazonParser;
            }
        }

        Dictionary<Domens, IHtmlParser> _parsersForDomens = new()
        {
            { Domens.Amazon, AmazonParser}
        };

        public IHtmlParser Create(string url)
        {
            Domens domen = SelectDomen(url);

            if (domen is Domens.None)
            {
                string message = GetInvalidDomenMessage(url);

                throw new ArgumentException(message);
            }

            return GetHtmlParser(domen);
        }
        private IHtmlParser GetHtmlParser(Domens domen)
        {
            return _parsersForDomens[domen];
        }
        private Domens SelectDomen(string url)
        {
            var domen = Domens.None;

            var predicate = 
                (KeyValuePair<string, Domens> k)
                => url.Contains(k.Key);

            bool isValid = _domens.Any(predicate);

            if (isValid)
            {
                domen = _domens
                .FirstOrDefault
                (predicate).Value;
            }

            return domen;
        }
        private string GetInvalidDomenMessage(string url)
        {

            var message = new StringBuilder("Can`t scrape data from ");

            message = message.Append(url + ".");

            message.AppendLine("Available domens:");
            message.AppendLine();

            foreach (var key in _domens.Keys)
            {
                message.Append(key + ", ");
            }

            return message.ToString();
        }
    }
}
