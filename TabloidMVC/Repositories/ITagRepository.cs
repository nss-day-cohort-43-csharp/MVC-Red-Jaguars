using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ITagRepository
    {
        List<Tag> GetAllTags();
        Tag GetTagById(int id);
        void AddTag(Tag tag);
        void UpdateTag(Tag tag);
        void DeleteTag(int tagId);
        void AddTagToPost(int tagId, Post post);
        List<Tag> GetTagPostById(int id);
        void DeleteTagFromPost(int id);
        List<Tag> GetTagForDelete(int id);
    }
}
