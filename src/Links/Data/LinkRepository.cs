using System;
using System.Collections.Generic;
using System.Linq;
using Links.Models;
using Links.Models.View;
using Links.Utils;

namespace Links.Data
{
    public interface ILinkRepository
    {
        bool DeleteLink(long linkId);

        IEnumerable<LinkModel> GetAllLinks(string url);

        LinkModel GetLinkById(long linkId, string url);

        LinkModel GetLinkByUrlKey(string urlKey, string url);

        LinkModel SaveLink(Link link, string url);

        LinkModel UpdateLink(Link link, string url);
    }

    public class LinkRepository : ILinkRepository
    {
        private readonly LinkContext _context;

        public LinkRepository(LinkContext context)
        {
            _context = context;
        }

        public LinkModel GetLinkByUrlKey(string urlKey, string url)
        {
            Link link = _context.ShortLinks.FirstOrDefault(l => l.UrlKey == urlKey);
            return link == null ? null : LinkModel.FromEntity(link, url);
        }

        public LinkModel UpdateLink(Link link, string url)
        {
            var result = _context.ShortLinks.FirstOrDefault(x => x.Id == link.Id);
            if (result == null)
            {
                return null;
            }
            result.LongUrl = link.LongUrl;
            result.UrlKey = link.UrlKey;
            result.RedirectsCount = link.RedirectsCount;
            _context.SaveChanges();
            return LinkModel.FromEntity(result, url);
        }

        public IEnumerable<LinkModel> GetAllLinks(string url)
        {
            return _context.ShortLinks.Select(m => LinkModel.FromEntity(m, url.ToString()));
        }

        public LinkModel GetLinkById(long linkId, string url)
        {
            Link link = _context.ShortLinks.FirstOrDefault(link => link.Id == linkId);
            return LinkModel.FromEntity(link, url);
        }

        public LinkModel SaveLink(Link link, string url)
        {
            link.Id = 0;
            link.InsertionDate = DateTimeOffset.Now;
            if (string.IsNullOrWhiteSpace(link.UrlKey))
            {
                link.UrlKey = StringUtils.GenRandomString(6);
            }
            var result = _context.ShortLinks.Add(link);
            _context.SaveChanges();
            return LinkModel.FromEntity(result.Entity, url);
        }

        /// <summary>
        /// Удаляет ссылку из БД.
        /// </summary>
        /// <returns>true, если ссылка удалена и false, если такой ссылки нет в БД.</returns>
        public bool DeleteLink(long linkId)
        {
            Link link = _context.ShortLinks.FirstOrDefault(link => link.Id == linkId);

            if (link == null)
            {
                return false;
            }

            _context.ShortLinks.Remove(link);
            _context.SaveChanges();
            return true;
        }
    }
}