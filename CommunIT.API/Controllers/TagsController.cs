using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunIT.Models.Interfaces;
using CommunIT.Shared.Portable.DTOs.Tag;
using CommunIT.Shared.Portable.DTOs.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommunIT.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class TagsController : Controller
    {
        private readonly ITagRepository _tagRepository;

        public TagsController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        [HttpGet]
        [Route("~/api/[controller]/basetags")]
        public ActionResult<IEnumerable<TagDetailDto>> GetBaseTags()
        {
            return _tagRepository.ReadBaseTags().ToList();
        }

        [HttpGet]
        [Route("~/api/[controller]/subtags")]
        public ActionResult<IEnumerable<TagDetailDto>> GetSubTags()
        {
            return _tagRepository.ReadSubTags().ToList();
        }

        [HttpGet("{communityId}")]
        [Route("~/api/[controller]/community/{communityId}")]
        public async Task<ActionResult<BaseSubTagDto>> GetBaseSubTagsByCommunityId(int communityId)
        {
            return await _tagRepository.ReadTagsForCommunityAsync(communityId);
        }
    }
}
