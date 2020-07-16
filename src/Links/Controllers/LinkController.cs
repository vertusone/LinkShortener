using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Links.Data;
using Links.Models.View;
using Links.Utils;

namespace Links.Controllers
{
    [Route("link")]
    public class LinkController : ControllerBase
    {
        private readonly ILinkRepository _repo;
        private readonly ILogger<LinkController> _log;

        private string UrlPrefix =>
            new UriBuilder(Request.Scheme, Request.Host.Host, Request.Host.Port.Value, "l").ToString();

       
        public LinkController(ILinkRepository repo, ILogger<LinkController> logger)
        {
            _repo = repo;
            _log = logger;
        }

        [HttpGet("/l/{urlKey}")]
        public IActionResult Get(string urlKey)
        {
            try
            {
                LinkModel link = _repo.GetLinkByUrlKey(urlKey, UrlPrefix);

                if (link == null)
                {
                    _log.LogWarning("Ссылка не найдена");
                    return NotFound();
                }

                link.RedirectsCount++;
                LinkModel result = _repo.UpdateLink(link, UrlPrefix);
                if (result == null)
                {
                    return NotFound();
                }

                return Redirect(link.LongUrl);
            }
            catch (Exception ex)
            {
                _log.LogError("Ошибка доступа к БД");
                return Problem("Ошибка доступа к БД");
            }
        }

        [HttpGet("all")]
        [Produces("application/json")]
        public IActionResult Get()
        {
            try
            {
                return Ok(_repo.GetAllLinks(UrlPrefix));
            }
            catch (Exception)
            {
                _log.LogError("Ошибка доступа к БД");
                return Problem("Ошибка доступа к БД");
            }
        }

        [HttpGet("{linkId}")]
        [Produces("application/json")]
        public IActionResult GetShortLink(long linkId)
        {
            try
            {
                LinkModel link = _repo.GetLinkById(linkId, UrlPrefix);

                if (link == null)
                {
                    _log.LogError("Ссылка не найдена");
                    return NotFound();
                }

                return Ok(link);
            }
            catch (Exception)
            {
                _log.LogError("Ошибка доступа к БД");
                return Problem("Ошибка доступа к БД");
            }
        }

        [HttpPost]
        public IActionResult Save([FromBody]LinkModel link)
        {
            if (link == null)
            {
                _log.LogError("Не могу сохранить новую ссылку");
                return Problem("Не могу сохранить новую ссылку");
            }

            if (string.IsNullOrWhiteSpace(link.LongUrl.Trim()))
            {
                _log.LogError("Поле \"Адрес ссылки\" не было заполнено");
                return Problem(title: "Поле \"Адрес ссылки\" не было заполнено");
            }

            try
            {
                var result = _repo.SaveLink(link, UrlPrefix);
                return Ok(result);
            }
            catch (Exception)
            {
                _log.LogError("Не могу сохранить новую ссылку");
                return Problem("Не могу сохранить новую ссылку");
            }
        }

        [HttpPut]
        public IActionResult Update([FromBody]LinkModel link)
        {
            if (link == null)
            {
                _log.LogError("Не могу сохранить изменения");
                return Problem("Не могу сохранить изменения");
            }

            if (string.IsNullOrWhiteSpace(link.LongUrl.Trim()))
            {
                _log.LogError("Поле \"Адрес ссылки\" не было заполнено");
                return Problem("Поле \"Адрес ссылки\" не было заполнено");
            }

            try
            {
                if (string.IsNullOrWhiteSpace(link.UrlKey))
                {
                    link.UrlKey = StringUtils.GenRandomString(6);
                }
                var result = _repo.UpdateLink(link, UrlPrefix);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception)
            {
                _log.LogError("Не могу сохранить изменения");
                return Problem("Не могу сохранить изменения");
            }
        }

        [HttpDelete("{linkId}")]
        public IActionResult Delete(long linkId)
        {
            try
            {
                bool success = _repo.DeleteLink(linkId);

                if (!success)
                {
                    _log.LogError("Ссылка не найдена");
                    return NotFound();
                }
                return Ok(linkId);
            }
            catch (Exception)
            {
                _log.LogError("Не могу удалить ссылку");
                return Problem("Не могу удалить ссылку");
            }
        }
    }
}