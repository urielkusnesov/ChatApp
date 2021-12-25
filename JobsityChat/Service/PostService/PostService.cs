using Model;
using Repository;
using System.Linq;
using System.Collections.Generic;

namespace Service.PostService
{
    public class PostService : IPostService
    {
        private readonly IRepositoryService repository;

        public PostService(IRepositoryService repository)
        {
            this.repository = repository;
        }

        public Post Add(Post post)
        {
            var result = repository.Add<Post>(post);
            repository.SaveChanges();
            return post;
        }

        public IList<Post> List(int page = 1, int pageSize = 50)
        {
            return repository.List<Post>().OrderByDescending(x => x.Date).Skip(pageSize * (page - 1)).Take(pageSize).ToList();
        }
    }
}
